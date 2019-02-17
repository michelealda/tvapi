using System.Collections.Generic;
using LiteDB;
using TvApi.Core;
using TvApi.Models;

namespace TvApi.Infrastructure
{
    public class ShowRepository : IShowRepository
    {
        private const string ConnectionString = "ShowDatabase.db";

        public IEnumerable<Show> GetAll()
        {
            using (var db = new LiteRepository(ConnectionString))
            {
                return db
                    .Query<Show>()
                    .ToEnumerable();
            }
        }
        
        public void Insert(Show item)
        {
            using (var db = new LiteRepository(ConnectionString))
            {
                db.Upsert(item);
            }
        }
    }
}