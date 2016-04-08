using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface ISku
    {
        string DISPLAY_VALUE { get; set; }
        string SELECTED_VALUE { get; set; }
        string SKUID { get; set; }
        string SHIPTO { get; set; }
        
        Sku.Setting SETTING { get; set; }
    }
}
