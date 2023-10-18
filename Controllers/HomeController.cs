using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLocker.Software.API.Services.Interfaces;
using SmartLocker.Software.Backend.Constants;
//using SmartLocker.Software.Backend.Models;

namespace SmartLocker.Software.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ITestService _testService;

        public HomeController(ITestService _testService)
        {
            this._testService = _testService;
        }
        public string GenerateOrderID()
        {
            Random rnd = new Random();
            Int64 s1 = rnd.Next(000000, 999999);
            Int64 s2 = Convert.ToInt64(DateTime.Now.ToString("ddMMyyyyHHmmss"));
            string s3 = s1.ToString() + "" + s2.ToString();
            return s3;
        }
        // GET api/home
        [HttpGet("")]
        public ActionResult Getstrings()
        {

            var data = GenerateOrderID();
            return Ok(data);
        }

        // GET api/home/5
        [HttpGet("{id}")]
        public ActionResult<string> GetstringById(int id)
        {
            return null;
        }

        // POST api/home
        [HttpPost("")]
        public void Poststring(string value)
        {
        }

        // PUT api/home/5
        [HttpPut("{id}")]
        public void Putstring(int id, string value)
        {
        }

        // DELETE api/home/5
        [HttpDelete("{id}")]
        public void DeletestringById(int id)
        {
        }
    }
}