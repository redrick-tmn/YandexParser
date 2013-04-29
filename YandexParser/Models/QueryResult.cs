using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YandexParser.Models
{
    public class QueryResult
    {
        public virtual int Position { get; set; }
        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        
    }
}