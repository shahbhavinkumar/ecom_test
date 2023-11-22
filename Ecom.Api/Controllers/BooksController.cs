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


        [HttpGet, Route("gettotalnumberofworks")]
      
        public async Task<IActionResult> GetTotalNumberofWorks()
        {
         
            var result =  await _booksService.GetTotalNumberOfWorksAsync();
            return Ok(result);
        }

        [HttpGet, Route("getworksbyrating")]
        [ProducesResponseType(typeof(TotalWorksByDate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWorksByRating(int rating)
        {
            if (rating > 0)
            {
                var works = await _booksService.GetWorkKeyForRatings(rating);
                return Ok(works);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet, Route("searchBook")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBook(string? works, int? rating)
        {
            if(works == null & rating == null)
            {
                return BadRequest();
            }

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
