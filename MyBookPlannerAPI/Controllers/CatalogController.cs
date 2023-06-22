using System;
using Microsoft.AspNetCore.Mvc;


namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class CatalogController : ControllerBase
    {
        [HttpGet]
        [Route("/catalog")]
        public string Get()
        {
            return "Hello ueaah";
        }
    }
}

