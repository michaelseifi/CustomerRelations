using System;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface ICellComment
    {
        int CELL_COMMENT_ID { get; set; }
        DateTime DATE_ENTERED { get; set; }
        string ENTERED_BY { get; set; }
        Controlers.Objects.CellComment.Field FIELD { get; set; }
        //string ITEM_UPC { get; set; }
        //string SHIP_TO_TP { get; set; }
        string VALUE { get; set; }
        string WEEK_ID { get; set; }
    }
}
