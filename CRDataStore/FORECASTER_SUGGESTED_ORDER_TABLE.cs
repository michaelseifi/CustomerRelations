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
    
    public partial class FORECASTER_SUGGESTED_ORDER_TABLE
    {
        public System.Guid SUGGESTED_ORDER_ID { get; set; }
        public System.Guid ORDERID { get; set; }
        public double ORDER_SUGGESTED { get; set; }
        public string FORMULA { get; set; }
        public int ORDER_TO_DELIVERY { get; set; }
        public int FORECAST1 { get; set; }
        public int FORECAST2 { get; set; }
        public int FORECAST3 { get; set; }
        public int FORECAST4 { get; set; }
        public int FORECAST5 { get; set; }
        public double SAFETY_STOCK { get; set; }
        public double I_SAFETY_STOCK_DAYS { get; set; }
        public double H_ON_HAND_DAY_OF_ORDER { get; set; }
        public double I_ON_ORDER_DAY_OF_ORDER { get; set; }
        public int II_DAYS_852_IS_REPORTED { get; set; }
        public int III_DAYS_QS_INCLUDING_AND_AFTER_ORDER_DAY { get; set; }
        public int IV_DAYS_QS_BEFORE_AND_INCLUDING_ORDER_DELIVERY_DAY { get; set; }
        public int V_DAYS_QS_AFTER_ORDER_DELIVERY_DAY_AT_DC { get; set; }
        public double A_PULLS_FROM_CURRENT_FORECAST { get; set; }
        public double D_PULLS_FROM_SECOND_WEEK_FORECAST { get; set; }
        public double E_PULLS_FROM_THIRD_WEEK_FORECAST { get; set; }
        public double F_PULLS_FROM_FORTH_WEEK_FORECAST { get; set; }
        public System.DateTime INSERT_DATE { get; set; }
    }
}
