using System;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IFsi
    {
        DateTime DATE_ENTERED { get; set; }
        string ENTERED_BY { get; set; }
        int FSIID { get; set; }
        string VALUE { get; set; }
        string WEEKID { get; set; }
        string ITEM_UPC { get; set; }
        string SHIP_TO_TP { get; set; }

    }
}
