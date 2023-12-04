using Ecom.Api.Services;
using Ecom.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;

namespace Ecom.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {

        IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }


        [HttpGet, Route("gettotalnumberofworks")]
        public async Task<List<TotalWorksByDate>> GetTotalNumberofWorks()
        {
            var result =  await _booksService.GetTotalNumberOfWorksAsync();
            return result;
        }

      

        [HttpGet, Route("getworksbyrating")]
        public async Task<List<string>> Get([FromQuery] int rating)
        {
            return await _booksService.GetWorkKeyForRatings(rating);
        }

        [HttpGet, Route("searchBook")] ///works/OL1629179W
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBook(string works)
        {
            
            BookSearch bookSearchObject = new BookSearch()
            {
                WorkKey = works,
            };

            await _booksService.SearchBookAsync(bookSearchObject);
            return Ok();
        }
    }
}
