using LiteDB;
using System.Linq.Expressions;

namespace REST_Task6_CatalogService.Context
{
    public interface ILiteContext
    {
        public List<T> GetCollection<T>();

        public void Insert<T>(T item);

        public int InsertWithId<T>(T item);

        public void Update<T>(T item);

        public void Delete<T>(int id);

        public void DeleteMany<T>(Expression<Func<T, bool>> predicate);
    }
}
