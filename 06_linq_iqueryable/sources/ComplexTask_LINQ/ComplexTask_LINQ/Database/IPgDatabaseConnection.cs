using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Test
{
    public interface IPgDatabaseConnection
    {
        IEnumerable<T> ExecuteQuery<T>(string query);
    }
}
