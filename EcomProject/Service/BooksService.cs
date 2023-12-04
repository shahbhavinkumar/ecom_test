

using Ecom.Shared;
using EcomProject.Pages;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;

namespace EcomProject.Service
{
    public interface IBooksService
    {
        Task<List<TotalWorksByDate>?> GetTotalNumberOfWorks();
        Task<List<string>?> GetWorkKeysBasedOnRating(int rating);
        Task<string> SearchBookInFile(string key);
    }

    public class BooksService : IBooksService
    {
        private readonly string url = "https://localhost:5010/";
        public async Task<string> SearchBookInFile(string works)
        {
            string fileName = default!;


            string queryString = $"?works={works}";

            string fullUrl = $"{url+"api/books/searchBook"}{queryString}";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    fileName = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }


            return fileName;
        }


        public async Task<List<TotalWorksByDate>?> GetTotalNumberOfWorks()
        {
            List<TotalWorksByDate>? list = new();

            using (var client = new HttpClient())
            {

                    UriBuilder builder = new UriBuilder(url+"api/books/gettotalnumberofworks");

                    var response = await client.GetAsync(builder.Uri);

                    list = await response.Content.ReadFromJsonAsync<List<TotalWorksByDate>>();
            }   
         

            return list;
        }

        public async Task<List<string>?> GetWorkKeysBasedOnRating(int rating)
        {

            List<string?> result = null;

            string queryString = $"?rating={rating}";

            string fullUrl = $"{url+"api/books/getworksbyrating"}{queryString}";

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadFromJsonAsync<List<string>>();

                       // result = JsonConvert.DeserializeObject<List<string?>>(responseBody);

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }

          

            return result;

        }


        
    }
}
