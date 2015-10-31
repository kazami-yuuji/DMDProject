using DMDProject.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMDProject.Controllers
{
    public class SearchController : Controller
    {
        static readonly string PASSWORD = "password";

        public ActionResult Index(string searchString)
        {
            searchString = HttpUtility.HtmlEncode(searchString);
            // Opening connection
            NpgsqlConnection connection = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=" + PASSWORD + ";Database=project;");
            connection.Open();
            // Find required articles
            List<Article> articles = new List<Article>();
            if (searchString != string.Empty & searchString != null)
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT title, description, name " +
                                                          "FROM articles AR, authors AU, authors_of_article AA " +
                                                          "WHERE AU.name LIKE '%" + searchString + "%' " +
                                                          "AND AA.author_id = AU.id " +
                                                          "AND AA.article_id = AR.id ", connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Article current = new Article();
                    current.Title = (string)reader[0];
                    current.Description = (string)reader[1];
                    current.Author = (string)reader[2];
                    articles.Add(current);
                }
            }
            return View(articles);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}