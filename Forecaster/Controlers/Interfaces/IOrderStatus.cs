using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IOrderStatus
    {
        string ENUMID { get; set; }
        int VALUE { get; set; }
        string NAME { get; set; }

    }
}
