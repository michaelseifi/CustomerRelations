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
    
    public partial class FORECASTER_GET_ACTUAL_ORDERS_BY_SALES_ORDER_ID_Result
    {
        public string SALESID { get; set; }
        public string ship_to_tp { get; set; }
        public string item_upc { get; set; }
        public System.DateTime order_date { get; set; }
        public decimal quantity { get; set; }
        public decimal netWeight { get; set; }
        public decimal gorssWeight { get; set; }
        public int LINEITEMDETAILEDSATATUS { get; set; }
        public int LINEITEMSALESSTATUS { get; set; }
        public decimal REMAINSALESPHYSICAL { get; set; }
        public System.DateTime DELIVERYDATE { get; set; }
        public string PURCHORDERFORMNUM { get; set; }
        public int SALESSTATUS { get; set; }
        public int DETAILEDSTATUS { get; set; }
    }
}
