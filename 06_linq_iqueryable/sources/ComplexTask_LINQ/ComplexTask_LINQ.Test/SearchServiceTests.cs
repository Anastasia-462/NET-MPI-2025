using ComplexTask_LINQ.Database;
using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Provider;
using ComplexTask_LINQ.Provider.CustomProvider;
using ComplexTask_LINQ.Services;
using ComplexTask_LINQ.Test.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;

namespace ComplexTask_LINQ.Test
{
    public class SearchServiceTests
    {
        [Fact]
        public void SearchBooks_ShouldProvidedCostBetween15And25_ReturnBooksInPriceRange()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Book>(
                new List<List<Book>>
                { new List<Book>
                {
                    new Book { Id = 1, Name = "Book1", Cost = 10 },
                    new Book { Id = 2, Name = "Book2", Cost = 20 },
                    new Book { Id = 2, Name = "Book3", Cost = 30 },
                } });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchBooks(15, 25);

            // Assert
            var expected = result.FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(expected.Name, "Book2");
            Assert.Equal(expected.Cost, 20);
            Assert.Equal(expected.Id, 2);
        }

        [Fact]
        public void SearchBooksSql_ShouldProvidedCostBetween15And25_ReturnSqlQuery()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Book>(
                new List<List<Book>>
                { new List<Book>
                {
                    new Book { Id = 1, Name = "Book1", Cost = 10 },
                    new Book { Id = 2, Name = "Book2", Cost = 20 },
                    new Book { Id = 2, Name = "Book3", Cost = 30 },
                } });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchBooksSql(15, 25);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, "SELECT * FROM Books WHERE Cost < 25 AND Cost > 15");
        }

        [Fact]
        public void SearchReaders_ShouldProvidedName_ReturnReader()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Reader>(
                new List<List<Reader>>
                { new List<Reader>
                {
                    new Reader { Id = 1, Name = "Pavel Smirnov" },
                    new Reader { Id = 2, Name = "Oleg Ivanov" },
                    new Reader { Id = 2, Name = "Masha Shane" },
                    new Reader { Id = 2, Name = "Nina Koval" },
                } });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchReaders("Oleg Ivanov");

            // Assert
            var expected = result.FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(expected.Name, "Oleg Ivanov");
            Assert.Equal(expected.Id, 2);
        }

        [Fact]
        public void SearchReadersSql_ShouldProvidedName_ReturnSqlQuery()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Reader>(
                new List<List<Reader>>
                { new List<Reader>
                {
                    new Reader { Id = 1, Name = "Pavel Smirnov" },
                    new Reader { Id = 2, Name = "Oleg Ivanov" },
                    new Reader { Id = 2, Name = "Masha Shane" },
                    new Reader { Id = 2, Name = "Nina Koval" },
                } });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchReadersSql("Oleg Ivanov");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, "SELECT * FROM Readers WHERE Name = 'Oleg Ivanov'");
        }
        
        [Fact]
        public void SearchTopFeedbacks_ShouldRequestIsValid_ReturnFeedbacks()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Feedback>(
                new List<List<Feedback>>
                {
                    new List<Feedback>
                    {
                        new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 1, Stars = 5 },
                        new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 1, Stars = 4 },
                        new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 1, Stars = 2 },
                    }
                });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchTopFeedbacks();

            // Assert
            var expected_1 = result.FirstOrDefault();
            var expected_2 = result.Skip(1).FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(result.Count, 2);
            Assert.Equal(expected_1.Comment, "Awesome book");
            Assert.Equal(expected_2.Comment, "Not bad book");
        }

        [Fact]
        public void SearchTopFeedbacksSql_ShouldRequestIsValid_ReturnSqlQuery()
        {
            // Arrange
            var fakeDatabase = new FakeDatabaseConnection<Feedback>(
                new List<List<Feedback>>
                {
                    new List<Feedback>
                    {
                        new Feedback { BookId = 2, Comment = "Awesome book", ReaderId = 1, Stars = 5 },
                        new Feedback { BookId = 3, Comment = "Not bad book", ReaderId = 1, Stars = 4 },
                        new Feedback { BookId = 4, Comment = "Not interesting book", ReaderId = 1, Stars = 2 },
                    }
                });

            var realProvider = new CustomQueryProvider(fakeDatabase);
            var searchService = new SearchService(realProvider);

            // Act
            var result = searchService.SearchTopFeedbacksSql();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, "SELECT * FROM Feedbacks WHERE Stars = 4 OR Stars = 5");
        }
    }
}