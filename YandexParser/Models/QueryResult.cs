using System;
using System.Data.Entity;

namespace YandexParser.Models
{
    public class QueryResult
    {
        public virtual int Id { get; set; }
        public virtual int Position { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

        public virtual int QueryId { get; set; }
        public virtual Query Query { get; set; }

        public virtual int SiteId { get; set; }
        public virtual Site Site { get; set; }
    }

    public class Site 
    {
        public virtual int Id { get; set; }
        public virtual string Url { get; set; }
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
        public DbSet<Site> Sites { get; set; }
    }
}