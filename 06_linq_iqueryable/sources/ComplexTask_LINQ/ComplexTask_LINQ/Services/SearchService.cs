using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Provider.CustomProvider;

namespace ComplexTask_LINQ.Services
{
    public class SearchService
    {
        public readonly ICustomQueryProvider _provider;
        public SearchService(ICustomQueryProvider provider)
        {
            _provider = provider;
        }

        public List<Book> SearchBooks(double minCost, double maxCost)
        {
            var books = _provider.GetQueryable<Book>().ToList();

            var query = books.Where(book => book.Cost < maxCost && book.Cost > minCost);

            return query.ToList();
        }

        public string SearchBooksSql(double minCost, double maxCost)
        {
            var books = _provider.GetQueryable<Book>();

            var query = books.Where(book => book.Cost < maxCost && book.Cost > minCost);
            return query.ToString();
        }

        public List<Reader> SearchReaders(string name)
        {
            var readers = _provider.GetQueryable<Reader>().ToList();

            var query = readers.Where(reader => reader.Name == name);

            return query.ToList();
        }

        public string SearchReadersSql(string name)
        {
            var readers = _provider.GetQueryable<Reader>();

            var query = readers.Where(reader => reader.Name == name);

            return query.ToString();
        }

        public List<Feedback> SearchTopFeedbacks()
        {
            var feedbacks = _provider.GetQueryable<Feedback>().ToList();

            var query = feedbacks.Where(f => f.Stars == 4 || f.Stars == 5).ToList();

            return query;
        }

        public string SearchTopFeedbacksSql()
        {
            var feedbacks = _provider.GetQueryable<Feedback>();

            var query = feedbacks.Where(f => f.Stars == 4 || f.Stars == 5);

            return query.ToString();
        }

        //public List<BookFeedback> SearchBookFeedbacks()
        //{
        //    var books = _provider.GetQueryable<Book>().ToList();
        //    var feedbacks = _provider.GetQueryable<Feedback>().ToList();

        //    var query = books.Join(feedbacks, b => b.Id, f => f.BookId, (b, f) => new BookFeedback { Id = b.Id, Name = b.Name, Feedback = f.Comment, Stars = f.Stars }).ToList();

        //    return query;
        //}

        //public string SearchBookFeedbacksSql()
        //{
        //    var books = _provider.GetQueryable<Book>();
        //    var feedbacks = _provider.GetQueryable<Feedback>();

        //    var query = books.Join(feedbacks, b => b.Id, f => f.BookId, (b, f) => new BookFeedback { Id = b.Id, Name = b.Name, Feedback = f.Comment, Stars = f.Stars }).ToList();

        //    return query.ToString();
        //}
    }
}
