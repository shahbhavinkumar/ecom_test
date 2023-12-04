

using Ecom.Shared;
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
        void SearchBookInFile(string key);
    }

    public class BooksService : IBooksService
    {
        public void SearchBookInFile(string key)
        {

        }

    
        public async Task<List<TotalWorksByDate>?> GetTotalNumberOfWorks()
        {
            List<TotalWorksByDate>? list = new();

        
            using (var client = new HttpClient())
            {

                    UriBuilder builder = new UriBuilder("https://localhost:5010/api/books/gettotalnumberofworks");

                    var response = await client.GetAsync(builder.Uri);

                    list = await response.Content.ReadFromJsonAsync<List<TotalWorksByDate>>();
            }   
         

            return list;
        }

        public async Task<List<string>?> GetWorkKeysBasedOnRating(int rating)
        {

            List<string?> result = null;

            // Build the query string
            string queryString = $"?rating={rating}";

            // Combine the API endpoint with the query string
            string fullUrl = $"{"https://localhost:5010/api/books/getworksbyrating"}{queryString}";

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response content
                        result = await response.Content.ReadFromJsonAsync<List<string>>();

                       // result = JsonConvert.DeserializeObject<List<string?>>(responseBody);

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }

            }
            catch (Exception ex)
            {
                //implement logging and error handling
            }

            return result;

        }


        
    }
}
