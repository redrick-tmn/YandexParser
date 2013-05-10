using System.Data.Entity;
using YandexParser.Models;

namespace YandexParser.Controllers.Helpers
{
    public class DbInitializer : CreateDatabaseIfNotExists<QueryModel>
    {
        protected override void Seed(QueryModel context)
        {
        }
    }
}