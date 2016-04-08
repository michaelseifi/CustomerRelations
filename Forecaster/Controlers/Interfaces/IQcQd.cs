using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IQcQd
    {
        int ID { get; set; }
        string WEEKID { get; set; }
        string VALUE { get; set; }

        string FORMULA { get; set; }
        void SetValue(string value);
        int GetValue();
        DateTime DATE_ENTERED { get; set; }
        string FIELD { get; set; }
        string COMMENT { get; set; }
        string SHIP_TO_TP { get; set; }
        string ENTERED_BY { get; set; }
        string ITEM_UPC { get; set; }
    }
}
