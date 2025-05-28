using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Provider
{
    public static class TypeSystem
    {
        public static Type GetElementType(Type seqType)
        {
            if (seqType.IsGenericType && seqType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return seqType.GetGenericArguments()[0];
            }

            var iface = seqType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return iface?.GetGenericArguments()[0] ?? seqType;
        }
    }
}
