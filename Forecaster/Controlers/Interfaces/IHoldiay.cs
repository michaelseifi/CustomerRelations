using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IHoldiay
    {
        Guid HOLIDAYID { get; set; }
        DateTime START { get; set; }
        DateTime END { get; set; }
        string DESCRIPTION { get; set; }
        void Delete();
        void Insert();
    }
}
