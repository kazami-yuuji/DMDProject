using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMDProject.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publication { get; set; }
        public string Description { get; set; }
        public string MDURL { get; set; }
        public string PDF { get; set; }
    }
}