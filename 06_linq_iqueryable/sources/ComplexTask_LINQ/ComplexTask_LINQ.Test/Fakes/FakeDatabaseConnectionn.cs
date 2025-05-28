using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Provider;
using ComplexTask_LINQ.Services;
using ComplexTask_LINQ.Test.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Test.Fakes
{
    public class FakeDatabaseConnectionn : IPgDatabaseConnection
    {
        private readonly Dictionary<Type, IEnumerable<object>> _data;

        public FakeDatabaseConnectionn(Dictionary<Type, IEnumerable<object>> data)
        {
            _data = data;
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            return _data.Cast<T>().ToList();
        }
    }
}


//[Fact]
//public void SearchBookFeedbacks_ShouldRequestIsValid_ReturnFeedbacks()
//{
//    // Arrange
//    var fakeDatabase = new FakeDatabaseConnection<Book>(
//        new List<List<Book>>
//        { new List<Book>
//                {
//                    new Book { Id = 1, Name = "Book1", Cost = 10 },
//                    new Book { Id = 2, Name = "Book2", Cost = 20 },
//                    new Book { Id = 2, Name = "Book3", Cost = 30 },
//                } });

//    var fakeDatabase2 = new FakeDatabaseConnection<Feedback>(
//        new List<List<Feedback>>
//        {
//                    new List<Feedback>
//                    {
//                        new Feedback { BookId = 1, Comment = "Good book", ReaderId = 1, Stars = 5 },
//                        new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 1, Stars = 5 },
//                        new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 1, Stars = 4 },
//                        new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 1, Stars = 2 },
//                    }
//        });

//    var realProvider = new CustomQueryProvider(fakeDatabase);
//    var realProvider2 = new CustomQueryProvider(fakeDatabase2);
//    var searchService = new SearchService(realProvider);

//    // Act
//    var result = searchService.SearchBookFeedbacks();

//    // Assert
//    var expected = result.FirstOrDefault();
//    Assert.NotNull(result);
//    Assert.Equal(expected.Name, "Oleg Ivanov");
//    Assert.Equal(expected.Id, 2);
//}

//[Fact]
//public void SearchFeedbacksSql_ShouldProvidedName_ReturnSqlQuery()
//{
//    // Arrange
//    var fakeData = new Dictionary<Type, IEnumerable<object>>
//    {
//        [typeof(Book)] = new List<Book>
//                {
//                    new Book { Id = 1, Name = "Book1", Cost = 100 },
//                    new Book { Id = 2, Name = "Book2", Cost = 200 }
//                },
//        [typeof(Feedback)] = new List<Feedback>
//                {
//                    new Feedback { BookId = 1, Stars = 5, Comment = "Great book!" },
//                    new Feedback { BookId = 2, Stars = 4, Comment = "Good read!" }
//                }
//    };

//    var fakeDatabase = new FakeDatabaseConnectionn(fakeData);
//    var realProvider = new CustomQueryProvider(fakeDatabase);
//    var searchService = new SearchService(realProvider);

//    // Act
//    var result = searchService.SearchBookFeedbacksSql();

//    // Assert
//    Assert.NotNull(result);
//    Assert.Equal(result, "SELECT * FROM Readers WHERE Name = 'Oleg Ivanov'");
//}