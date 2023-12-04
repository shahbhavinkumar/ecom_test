using System.Data;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;
using Ecom.Shared;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecom.Api.Services
{
    public interface IBooksService
    {
        public Task<string> SearchBookAsync(BookSearch book);
        Task<List<TotalWorksByDate>> GetTotalNumberOfWorksAsync();
        Task<List<string>> GetWorkKeyForRatings(int rating);
    }

    public class BooksService: IBooksService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<BooksService> _logger;

        public string cacheKey = "booksCache";
        List<BookSearch> booksCache;
        IConfigurationRoot _configuration;

        BookInformation bookInfoObject = new BookInformation();

        public BooksService(IMemoryCache memoryCache, ILogger<BooksService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            
            _configuration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json")
                                        .Build();
        }


        private string SaveDataToJsonFile(BookInformation bookInfoObject)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(bookInfoObject);
            var url = _configuration.GetValue<string>(@"Data:SaveJsonFilePath") + Guid.NewGuid() + ".json";
            File.WriteAllText(url, json);
            return url;
        }

        private async Task<List<BookSearch>> PopulateCacheAsync()
        {
            if (!_memoryCache.TryGetValue(cacheKey, out booksCache))
            {
                booksCache = GetBooksList();

                _memoryCache.Set(cacheKey, booksCache,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            }
            return booksCache;
        }

        private List<BookSearch> GetBooksList()
        {
            List<BookSearch> books = new List<BookSearch>();

            try
            {
                DataTable datatable = new DataTable();

                var fileName = _configuration.GetValue<string>(@"Data:DataURL");

                if(fileName == "")
                {
                    _logger.LogError("Missing Path from the Configuration in GetBooksList()");
                }

                StreamReader streamreader = new StreamReader(fileName);

                char[] delimiter = new char[] { '\t' };
                string[] columnheaders = new string[] { "WorkKey", "Edition", "Rating", "Date" };

                foreach (string columnheader in columnheaders)
                {
                    datatable.Columns.Add(columnheader); 
                }

                while (streamreader.Peek() > 0)
                {
                    DataRow datarow = datatable.NewRow();
                    datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                    datatable.Rows.Add(datarow);
                }

                books = (from DataRow dr in datatable.Rows
                               select new BookSearch()
                               {
                                   Rating = dr["Rating"] == null ? null : Convert.ToInt32(dr["Rating"]),
                                   WorkKey = dr["WorkKey"] == null ? null : dr["WorkKey"].ToString(),
                                   Edition = dr["Edition"] == null ? null : dr["Edition"].ToString(),
                                   Date = Convert.ToDateTime(dr["Date"].ToString())
                               }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetBooksList() : " + ex.Message);
                throw;
            }

            return books;
        }

        public async Task<List<TotalWorksByDate>> GetTotalNumberOfWorksAsync()
        {
            List<TotalWorksByDate> totalWorksByDate = new List<TotalWorksByDate>();
            
            if (booksCache == null)
            {
                await PopulateCacheAsync();
            }

            List<TotalWorksByDate> result =  booksCache!.GroupBy(item => item.Date)
                             .Select(grouped => new TotalWorksByDate { Date = grouped.Key, CountOfWorks = grouped.Count() }).ToList();

            return result;
        }

        public async Task<List<string>> GetWorkKeyForRatings(int rating)
        {
            if (booksCache == null)
            {
                await PopulateCacheAsync();
            }

            return booksCache!
                .Where(item => item.Rating == rating)
                .Select(item => item.WorkKey!.Split('/').Last()).ToList();
        }

        public async Task<string> SearchBookAsync(BookSearch book)
        {
            if (booksCache == null)
            {
                await PopulateCacheAsync();
            }

            if (book.WorkKey == null)
            {
                return "Work Key is required.";
            }

            BookSearch? bookPresent = booksCache?.FirstOrDefault(x => x.WorkKey == book.WorkKey);


            if (bookPresent == null)
            {
                return "Work Not Found";
            }

            /*if(bookPresent.Edition != null)
            {
                bookPresent = booksCache?.FirstOrDefault(x => x.Edition == book.Edition);
            }
            */
            var endPointUrl = _configuration.GetValue<string>("Data:EndPointURL") ?? "https://openlibrary.org";
            var bookUrl = $"{endPointUrl}{bookPresent.WorkKey}.json";

            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(bookUrl);

            if (!response.IsSuccessStatusCode)
            {
                return "Error fetching book information.";
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonContent);
            var root = jsonDocument.RootElement;

            var bookInfoObject = new BookInformation(); // Assuming you have a BookInfo class to store book information.

            bookInfoObject.Title = root.GetProperty("title").GetString();
            bookInfoObject.FirstSubject = GetFirstElementValue(root, "subjects");
            bookInfoObject.AuthorName = GetAuthorName(root, "authors", httpClient).Result;

            string fileNamePath = SaveDataToJsonFile(bookInfoObject);

            return fileNamePath;
        }

        private string? GetFirstElementValue(JsonElement root, string propertyName)
        {
            if (root.TryGetProperty(propertyName, out var subjectsElement) && subjectsElement.ValueKind == JsonValueKind.Array)
            {
                var firstSubjectElement = subjectsElement.EnumerateArray().FirstOrDefault();

                return firstSubjectElement.GetString();
            }

            return null;
        }

        private async Task<string?> GetAuthorName(JsonElement root, string propertyName, HttpClient httpClient)
        {
            if (root.TryGetProperty(propertyName, out var authorsElement) && authorsElement.ValueKind == JsonValueKind.Array)
            {
                var authorURL = authorsElement.EnumerateArray().FirstOrDefault();

                if ( authorURL.TryGetProperty("author", out var authorKeyElement) && authorKeyElement.ValueKind == JsonValueKind.Object)
                {
                    string authorKey = authorKeyElement.GetProperty("key").GetString();

                    if (!string.IsNullOrEmpty(authorKey))
                    {
                        var aURL = _configuration.GetValue<string>("Data:EndPointURL") ?? "https://openlibrary.org";
                        var authorUrl = $"{aURL}{authorKey}.json";

                        using var aResponse = await httpClient.GetAsync(authorUrl);

                        if (aResponse.IsSuccessStatusCode)
                        {
                            var ajsonContent = await aResponse.Content.ReadAsStringAsync();
                            var ajsonDocument = JsonDocument.Parse(ajsonContent);
                            var aroot = ajsonDocument.RootElement;

                            return aroot.GetProperty("personal_name").GetString();
                        }
                    }
                }
            }

            return null;
        }

    }
}
