using ComplexTask_LINQ.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComplexTask_LINQ.Test.Fakes
{
    //public class FakeDbConnection : IPgDatabaseConnection
    //{
    //    private readonly Dictionary<string, object> _fakeData;

    //    public FakeDbConnection()
    //    {
    //        _fakeData = new Dictionary<string, object>
    //        {
    //            // Ключ — это ваш запрос (или условное имя), значение — коллекция подходящего типа
    //            {
    //                "SELECT * FROM Books ", new Book[]
    //                {
    //                    new Book { Id = 1, Name = "C# in Depth", Cost = 20, Author = "Jon Skeet" },
    //                    new Book { Id = 2, Name = "Effective C#", Cost = 10, Author = "Bill Wagner" },
    //                    new Book { Id = 2, Name = "C#", Cost = 30, Author = "Bill Wagner" },
    //                }
    //            }
    //        };
    //    }

    //    public IEnumerable<T> ExecuteQuery<T>(string query)
    //    {
    //        var t = _fakeData.GetValueOrDefault(query);
    //        if (_fakeData.TryGetValue(query, out var result))// && result is IEnumerable<T> fakeResult)
    //        {
    //            var i = result is IEnumerable<T>;
    //            var s = result.GetType();
    //            return null;
    //        }

    //        return null;
    //    }
    //}
    public class FakeDatabaseConnection<T> : IPgDatabaseConnection
    {
        private readonly IEnumerable<IEnumerable<T>> _data;

        public FakeDatabaseConnection(IEnumerable<IEnumerable<T>> data)
        {
            _data = data;
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            return _data.Cast<T>().ToList();
        }

        //public IEnumerable<T> ExecuteQuery<T>(string query)
        //{
        //    var t = _data.GetType();
        //    var s = typeof(T);

        //    IEnumerable<T> tr = _data.Cast<T>();
        //    var tw = tr.GetType();
        //    object obj = _data;
        //    Type itemType = typeof(Book);
        //    var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(itemType);
        //    var enumerable = castMethod.Invoke(null, new object[] { obj });
        //    return enumerable;
        //}

        //public IEnumerable<T> ExecuteQuery<T>(string query)
        //{
        //    Type targetType = typeof(T);

        //    // Если T является IEnumerable<SomeType>, извлекаем SomeType
        //    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        //    {
        //        Type itemType = targetType.GetGenericArguments()[0];

        //        if (_data.TryGetValue(itemType, out var rawData))
        //        {
        //            // Приводим элементы к itemType и затем к IEnumerable<T>
        //            var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(itemType);
        //            var castedData = castMethod.Invoke(null, new object[] { rawData });

        //            var toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(itemType);
        //            var list = toListMethod.Invoke(null, new object[] { castedData });

        //            return (IEnumerable<T>)list;
        //        }
        //    }
        //    else
        //    {
        //        if (_data.TryGetValue(targetType, out var rawData))
        //        {
        //            return rawData.Cast<T>();
        //        }
        //    }

        //    throw new InvalidOperationException($"Данные для типа {targetType.Name} не найдены.");
        //}

        /// <summary>
        /// Выполняет запрос и преобразует данные в требуемый тип T
        /// </summary>
        //public IEnumerable<T> ExecuteQuery<T>(string query)
        //{
        //    var baseType = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(IEnumerable<>)
        //                    ? typeof(T).GetGenericArguments()[0]
        //                    : typeof(T);

        //    Console.WriteLine($"ExecuteQuery: Ищем данные для типа {baseType.Name}");

        //    if (_data.TryGetValue(baseType, out var rawData))
        //    {
        //        return ForceCastToEnumerable<T>(rawData);
        //    }

        //    throw new InvalidOperationException($"Данные для типа {baseType.Name} не найдены.");
        //}

        private IEnumerable<T> ForceCastToEnumerable<T>(IEnumerable<object> rawData)
        {
            if (rawData == null)
            {
                throw new InvalidOperationException("rawData равно null или пуста.");
            }

            var targetType = typeof(T);

            // Если T — это IEnumerable<SomeType>
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var itemType = targetType.GetGenericArguments()[0];

                var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(itemType);
                var toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(itemType);

                var casted = castMethod.Invoke(null, new object[] { rawData });
                var list = toListMethod.Invoke(null, new object[] { casted });

                return (IEnumerable<T>)list;
            }
            else
            {
                // Если T — это отдельный тип, например, Book
                return rawData
                    .Where(obj => obj != null && targetType.IsAssignableFrom(obj.GetType()))
                    .Select(obj => (T)obj)
                    .ToList();
            }
        }
    }
}
