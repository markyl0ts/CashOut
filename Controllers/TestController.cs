﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CashOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [EnableCors]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { code = HttpStatusCode.OK, data = default(object), message = "Test success" });
        }
    }
}
