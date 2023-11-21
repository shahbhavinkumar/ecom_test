

using EcomProject.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using static System.Net.WebRequestMethods;

namespace EcomProject.Pages
{
    partial class Books
    {
        [Inject] private IBooksService _bookService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                // Specify the full path to the text file on the D: drive
                string filePath = @"D:\searchFile.txt";

                // var mardownFile = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\sample-data\searchFile.txt"}");

                var xx = System.IO.Directory.GetCurrentDirectory();

                var byteofthefile = await _client.GetByteArrayAsync("sample-data/searchFile.txt");


            }
            catch (Exception ex)
            {

            }

        }
     }
}
