using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IComment
    {
        int COMMENTID { get; set; }
        DateTime DATE_ENTERED { get; set; }
        string ENTERED_BY { get; set; }
        string VALUE { get; set; }
        string WEEKID { get; set; }
        string SHIP_TO_TP { get; set; }
        string ITEM_UPC { get; set; }

    }
}
