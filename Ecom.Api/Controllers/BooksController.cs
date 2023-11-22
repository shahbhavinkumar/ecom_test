﻿using Ecom.Api.Services;
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
         
            await _booksService.GetTotalNumberOfWorksAsync();
            return Ok();
        }

        [HttpGet, Route("getworksbyrating")]
        public async Task<IActionResult> GetWorksByRating(int rating)
        {

            await _booksService.GetWorkKeyForRatings(rating);
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
