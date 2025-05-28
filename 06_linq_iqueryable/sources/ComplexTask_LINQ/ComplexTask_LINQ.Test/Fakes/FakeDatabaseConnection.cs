using ComplexTask_LINQ.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComplexTask_LINQ.Test.Fakes
{
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
    }
}
