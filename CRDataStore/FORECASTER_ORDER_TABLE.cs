//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRDataStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class FORECASTER_ORDER_TABLE
    {
        public System.Guid ORDERID { get; set; }
        public System.DateTime ORDER_DATE { get; set; }
        public System.DateTime DELIVERY_DATE { get; set; }
        public string WEEK_ID { get; set; }
        public System.DateTime WEEK_OF { get; set; }
        public string PO_NUMBER { get; set; }
        public string SALES_ORDER_ID { get; set; }
        public int SHIP_TO { get; set; }
        public string ITEM_UPC { get; set; }
        public int QA_BEGINNING { get; set; }
        public bool WILL_LAST { get; set; }
        public int QP_ONORDER { get; set; }
        public System.DateTime QA_RECEIPT { get; set; }
        public int QA_RECENT { get; set; }
        public int FORECAST { get; set; }
        public double ORDER_SUGGESTED { get; set; }
        public double ORDER_ACTUAL { get; set; }
        public Nullable<double> SAFETY_STOCK_DAYS { get; set; }
        public System.DateTime INSERT_DATE { get; set; }
        public Nullable<bool> INCLUDE { get; set; }
        public Nullable<int> QS_UP_TO_DATE { get; set; }
        public string COMMENT { get; set; }
    }
}
