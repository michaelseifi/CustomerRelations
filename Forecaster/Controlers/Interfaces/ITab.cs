using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface ITab
    {
        bool IsLoaded { get; set; }
        object CAPTION { get; set; }

        bool HAS_ERROR { get; set; }

        string ERROR_COMMENT { get; set; }
        ISku SKU { get; set; }

        string CUSTOMER_NUMBER { get; set; }
        object CONTENT { get; set; }

        string L4WKAVGTRN { get; }
        string L13WKAVGTRN { get; }

        DataCollection myDailyData { get; set; }
        DataCollection myWeeklyData { get; set; }
        DataCollection myFirst120Data { get; set; }
        PerformanceExceptionsCollection myPerformanceExceptions { get; set; }
        ViewModels.Graph myGraphViewModel { get; set; }
        ViewModels.Percentage myPercentageViewModel { get; set; }

        Objects.Settings.Customer.Tab Setting { get; set; }
        void SetError(string tooltip);
        void Dispose();
        
    }
}
