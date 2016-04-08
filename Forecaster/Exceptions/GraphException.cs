using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Exceptions
{
    public class GraphException:ApplicationException
    {
        public GraphException(string message)
            : base(message)
        {

        }
    }
}
