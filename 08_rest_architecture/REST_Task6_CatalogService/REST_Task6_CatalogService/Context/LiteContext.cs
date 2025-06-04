using LiteDB;
using System;
using System.Linq.Expressions;

namespace REST_Task6_CatalogService.Context
{
    public class LiteContext : ILiteContext
    {
        private readonly string _connectionString;

        public LiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<T> GetCollection<T>()
        {
            var t = typeof(T).Name;
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);

            return col.Query().ToList();
        }

        public void Insert<T>(T item)
        {
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);
            col.Insert(item);
        }

        public int InsertWithId<T>(T item)
        {
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);
            return (int)col.Insert(item).RawValue;
        }

        public void Update<T>(T item)
        {
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);
            col.Update(item);
        }

        public void Delete<T>(int id)
        {
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);
            col.Delete(id);
        }

        public void DeleteMany<T>(Expression<Func<T, bool>> predicate)
        {
            using var db = new LiteDatabase(_connectionString);
            var col = db.GetCollection<T>(typeof(T).Name);
            col.DeleteMany(predicate);
        }
    }
}
