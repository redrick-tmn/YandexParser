using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace YandexParser.Models
{
    public class QueryResult
    {
        public virtual int Id { get; set; }
        public virtual int Position { get; set; }
        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int QueryId { get; set; }
      //  [ForeignKey("QueryId")]
        public virtual Query Query { get; set; }

    }
    
    public class Query
    {
        public virtual int Id { get; set; }
        public virtual string QueryText { get; set; }
        public virtual DateTime Date { get; set; }
    }

    public class QueryModel : DbContext
    {
        public QueryModel()
            : base("name=QueryDatabase")
        {
        }

        public DbSet<Query> Queries { get; set; }
        public DbSet<QueryResult> QueryResults { get; set; }
    }
}