using DataAPI.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class HomeController : AsyncController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApiTest()
        {
            var data = new DataService();
            var task = data.GetCompetition();
            var result = task;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}