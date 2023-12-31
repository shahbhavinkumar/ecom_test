﻿

using Ecom.Shared;
using EcomProject.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using static System.Net.WebRequestMethods;

namespace EcomProject.Pages
{
    partial class Books
    {
        [Inject] private IBooksService _bookService { get; set; } = default!;
        public int? Ratings { get; set; }
        public string? WorkKey { get; set; }
        public string? DisplayMessage { get; set; }
        public List<TotalWorksByDate>? listOfTotalWorks { get; set; }
        public List<string>? listOfKeys { get; set; }
        public string? fileName { get; set; }

        private async Task SubmitChanges()
        {
            listOfTotalWorks = null;

            try
            {
                if(Ratings == null && WorkKey == null)
                {
                    listOfTotalWorks = await _bookService.GetTotalNumberOfWorks();
                }
                else if(Ratings != null && WorkKey == null)
                {
                    listOfKeys = await _bookService.GetWorkKeysBasedOnRating(int.Parse(Ratings.Value.ToString()));
                }
                else if(Ratings == null && WorkKey != null)
                {
                    fileName = await _bookService.SearchBookInFile(WorkKey.ToString());
                    WorkKey = null; //some ui issues with blazor so have to set it to null
                }
                else if(Ratings != null && WorkKey != null)
                {
                    throw new IOException("Cannot have both the fields populated");
                }


            }

            catch (Exception ex)
            {
                DisplayMessage = ex.Message;
            }
        }
     }
}
