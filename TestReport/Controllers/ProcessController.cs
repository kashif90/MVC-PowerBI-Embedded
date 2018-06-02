using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestReport.Controllers
{
    public class ProcessController : Controller
    {
        // GET: Process
        public ActionResult Landing()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}