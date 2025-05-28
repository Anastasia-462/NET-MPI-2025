using ComplexTask_LINQ.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Provider.CustomProvider
{
    public interface ICustomQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression);

        public object Execute(Expression expression);

        public TResult Execute<TResult>(Expression expression);

        public IQueryable<T> GetQueryable<T>();
    }
}
