using DMDProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMDProject.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            // Test
            List<Article> articles = new List<Article>();
            Article test = new Article();
            test.Title = "Some Title";
            test.PublicationID = 0;
            articles.Add(test);
            return View(articles);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}