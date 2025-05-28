using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.DbModels
{
    public class Feedback
    {
        public long ReaderId { get; set; }

        public int BookId { get; set; }

        public int Stars { get; set; }

        public string Comment { get; set; }
    }
}
