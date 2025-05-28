using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Provider;
using ComplexTask_LINQ.Provider.CustomProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Test.Fakes
{
    public class CustomQueryProviderWithHardcodedData : ICustomQueryProvider
    {
        private readonly List<Book> _predefinedBooks;
        private readonly List<Reader> _predefinedReaders;
        private readonly List<Feedback> _predefinedFeedbacks;

        // Передаем фиксированные данные
        public CustomQueryProviderWithHardcodedData()
        {
            _predefinedBooks = new List<Book>
            {
                new Book { Id = 1, Name = "Book1", Cost = 10 },
                new Book { Id = 2, Name = "Book2", Cost = 20 },
                new Book { Id = 3, Name = "Book3", Cost = 30 }
            };

            _predefinedReaders = new List<Reader>
            {
                new Reader { Id = 1, Name = "Pavel Smirnov" },
                new Reader { Id = 2, Name = "Oleg Ivanov" },
                new Reader { Id = 2, Name = "Masha Shane" },
                new Reader { Id = 2, Name = "Nina Koval" },
            };

            _predefinedFeedbacks = new List<Feedback>
            {
                new Feedback { BookId = 1, Comment = "Good book", ReaderId = 1, Stars = 5 },
                new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 1, Stars = 5 },
                new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 1, Stars = 4 },
                new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 1, Stars = 2 },

                new Feedback { BookId = 1, Comment = "Good book", ReaderId = 2, Stars = 5 },
                new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 2, Stars = 5 },
                new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 2, Stars = 4 },
                new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 2, Stars = 2 },

                new Feedback { BookId = 1, Comment = "Good book", ReaderId = 3, Stars = 5 },
                new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 3, Stars = 5 },
                new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 3, Stars = 4 },
                new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 3, Stars = 2 },

                new Feedback { BookId = 1, Comment = "Good book", ReaderId = 4, Stars = 5 },
                new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 4, Stars = 5 },
                new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 4, Stars = 4 },
                new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 4, Stars = 2 },
            };
        }

        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            // Используем IQueryable для фильтрации LINQ-запросов
            return _predefinedBooks.AsQueryable().Provider.CreateQuery<T>(expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return _predefinedBooks.AsQueryable().Provider.CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            // Выполнение запросов без фильтрации
            return _predefinedBooks.AsQueryable().Provider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            //Console.WriteLine($"[Execute<TResult>] Processing expression: {expression}");

            //if (expression is MethodCallExpression methodCall)
            //{
            //    Console.WriteLine($"Method: {methodCall.Method.Name}");

            //    if (methodCall.Method.Name == "Where")
            //    {
            //        // Разбираем аргументы для Where
            //        var lambda = (LambdaExpression)((UnaryExpression)methodCall.Arguments[1]).Operand;
            //        var compiled = (Func<Book, bool>)lambda.Compile();

            //        // Фильтруем данные вручную
            //        var filteredData = _predefinedBooks.Where(compiled);
            //        return (TResult)(object)filteredData;
            //    }
            //}

            //// Для других запросов
            //return (TResult)_predefinedBooks.AsQueryable().Provider.Execute(expression);
            Console.WriteLine($"[CustomQueryProvider] Executing Expression: {expression}");

            // Метод для обработка простых запросов (только для интеграции в тестах)
            if (typeof(TResult) == typeof(IEnumerable<Book>))
            {
                var data = _predefinedBooks.AsQueryable().Provider.Execute<IEnumerable<Book>>(expression);
                return (TResult)(object)data;
            }

            throw new NotSupportedException($"Expression not supported: {expression}");
        }

        public IQueryable<T> GetQueryable<T>()
        {
            // Определяем, какая коллекция (данные) связаны с типом T
            if (typeof(T) == typeof(Book))
            {
                return new CustomQueryable<T>(this, _predefinedBooks.AsQueryable().Expression);
            }
            else if (typeof(T) == typeof(Reader))
            {
                return new CustomQueryable<T>(this, _predefinedReaders.AsQueryable().Expression);
            }

            throw new NotSupportedException($"The type {typeof(T).Name} is not supported by this provider.");
        }
    }
}
