using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Exceptions
{
    public class InsertException:ApplicationException
    {
        public InsertException(string message)
            : base(message)
        {

        }
    }
}
