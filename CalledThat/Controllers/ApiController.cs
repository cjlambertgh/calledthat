using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAPI;
using Data;
using DataAPI.Implementations;
using Data.Models;

namespace CalledThat.Controllers
{
    public class ApiController : Controller
    {
        

        // GET: Api
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminInit()
        {
            
            

            return View();
        }
    }
}