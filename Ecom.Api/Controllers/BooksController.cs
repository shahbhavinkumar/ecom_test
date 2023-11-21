using Ecom.Api.Services;
using Ecom.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;

namespace Ecom.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {

        IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        /* //This method will take in the file which has data about keys, edition etc. 
         //and populate it in the cache object
         [HttpGet, Route("populateCache")]
         public IActionResult PopulateCache()
         {

             _booksService.PopulateCacheAsync();
             return Ok();
         }
        */

        [HttpGet, Route("gettotalnumberofworks")]
        public async Task<IActionResult> GetTotalNumberofWorks()
        {
         
            await _booksService.GetTotalNumberOfWorksAsync();
            return Ok();
        }



        [HttpGet, Route("searchBook")]
        public async Task<IActionResult> SearchBook(string? works, int? rating)
        {
            BookSearch bookSearchObject = new BookSearch()
            {
                WorkKey = works,
                Rating = rating
            };

            await _booksService.SearchBookAsync(bookSearchObject);
            return Ok();
        }
    }
}
