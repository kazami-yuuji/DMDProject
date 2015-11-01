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

        public ActionResult Index(string searchString, string searchMode)
        {
            searchString = HttpUtility.HtmlEncode(searchString);
            // Opening connection
            NpgsqlConnection connection = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=" + PASSWORD + ";Database=project;");
            connection.Open();
            // Find required articles
            List<Article> articles = new List<Article>();
            if (searchString != string.Empty & searchString != null)
            {
                // Select search mode from drop down list
                string mode = string.Empty;
                switch(searchMode)
                {
                    case "All":
                        mode = "WHERE articles.title LIKE '%" + searchString + "%' " +
                                "OR authors.name LIKE '%" + searchString + "%' " +
                                "OR publications.name LIKE '%" + searchString + "%' " +
                                "GROUP BY articles.id, articles.title, publications.name";
                        break;
                    case "Author":
                        mode = "GROUP BY articles.id, articles.title, publications.name " +
                               "HAVING string_agg(authors.name,';') LIKE '%" + searchString + "%'";
                        break;
                    case "Publication":
                        mode = "WHERE publications.name LIKE '%" + searchString + "%' " +
                               "GROUP BY articles.id, articles.title, publications.name";
                        break;
                    case "Article":
                        mode = "WHERE articles.title LIKE '%" + searchString + "%' " +
                               "GROUP BY articles.id, articles.title, publications.name";
                        break;
                }
                // Execute querry
                NpgsqlCommand command = new NpgsqlCommand("SELECT  articles.id, articles.title, publications.name, string_agg(authors.name,';') " +
                                                          "FROM articles " +
                                                          "INNER JOIN authors_of_article ON articles.id = authors_of_article.article_id " +
                                                          "INNER JOIN authors ON authors_of_article.author_id = authors.id " +
                                                          "INNER JOIN publications ON articles.publication_id = publications.id " +
                                                          mode, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                // Read the data
                while(reader.Read())
                {
                    Article current = new Article();
                    current.ID = (int)reader[0];
                    current.Title = (string)reader[1];
                    current.Publication = (string)reader[2];
                    current.Authors = (string)reader[3];
                    articles.Add(current);
                }
            }
            return View(articles);
        }

        public ActionResult Details(int id)
        {
            // Opening connection
            NpgsqlConnection connection = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=" + PASSWORD + ";Database=project;");
            connection.Open();
            // Getting required article
            Article current = new Article();
            NpgsqlCommand command = new NpgsqlCommand("SELECT articles.id, articles.title, publications.name, articles.description, articles.mdurl, articles.pdf, string_agg(authors.name,';')" +
                                                      "FROM articles " +
                                                      "INNER JOIN authors_of_article ON articles.id = authors_of_article.article_id " +
                                                      "INNER JOIN authors ON authors_of_article.author_id = authors.id " +
                                                      "INNER JOIN publications ON articles.publication_id = publications.id " +
                                                      "WHERE articles.id = " + id + " " +
                                                      "GROUP BY articles.id, articles.title, publications.name, articles.description, articles.mdurl, articles.pdf", connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                current.ID = (int)reader[0];
                current.Title = (string)reader[1];
                current.Publication = (string)reader[2];
                current.Description = (string)reader[3];
                current.MDURL = (string)reader[4];
                current.PDF = (string)reader[5];
                current.Authors = (string)reader[6];
            }
            ViewBag.Article = current;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}