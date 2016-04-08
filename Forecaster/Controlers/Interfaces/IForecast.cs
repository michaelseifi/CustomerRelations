using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IForecast
    {
        int FORECASTID { get; set; }
        DateTime DATE_ENTERED { get; set; }
        string ENTERED_BY { get; set; }
        int VALUE { get; set; }
        int QS { get; set; }
        int QW { get; set; }
        int QO { get; set; }
        int QA { get; set; }

        int QC { get; set; }
        int QD { get; set; }
        string WEEKID { get; set; }

        bool ACTIVE { get; set; }

        string FORMULA { get; set; }

        string L4WKAVGTRN { get; set; }
        string L13WKAVGTRN { get; set; }

        int BASELINE { get; set; }
    }
}
