using ComplexTask_LINQ.Database;
using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Services;
using ComplexTask_LINQ.Test;
using Dapper;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Provider.CustomProvider
{
    public class CustomQueryProvider : ICustomQueryProvider
    {
        private readonly IPgDatabaseConnection _dbConnection;

        public CustomQueryProvider(IPgDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);

            Type queryableType = typeof(CustomQueryable<>).MakeGenericType(elementType);
            return (IQueryable)Activator.CreateInstance(queryableType, this, expression)!;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CustomQueryable<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return Execute<object>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return TranslateAndExecute<TResult>(expression).FirstOrDefault();
        }

        private IEnumerable<T> TranslateAndExecute<T>(Expression expression)
        {
            var translator = new QueryTranslator();
            string query = translator.Translate<T>(expression);

            return ExecuteQuery<T>(query);
        }

        private IEnumerable<T> ExecuteQuery<T>(string query)
        {
            var result = _dbConnection.ExecuteQuery<T>(query);
            return result;
        }

        public IQueryable<T> GetQueryable<T>()
        {
            return new CustomQueryable<T>(this);
        }
    }
}
