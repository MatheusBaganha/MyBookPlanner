using System;
using Microsoft.AspNetCore.Mvc;


namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        [Route("/books")]
        public string Get()
        {
            return "Hello ueaah";
        }
    }
}

