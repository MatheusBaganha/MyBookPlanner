﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBookPlannerAPI.Controllers
{

    [ApiController]
    [Route("")]
    [AllowAnonymous]

    // Health Check API
    // Verify if API is online or not.
    public class HomeController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "API Online"
            });
        }
    }
}
