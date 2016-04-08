using System;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IOrderCase
    {
        DateTime ACTUAL_DELIVERY_DATE_KEY { get; set; }
        DateTime ACTUAL_SHIP_DATE_KEY { get; set; }
        string BK_ORDER_NUMBER { get; set; }
        DateTime CONFIRMED_DELIVERY_DATE_KEY { get; set; }
        DateTime CONFIRMED_SHIP_DATE_KEY { get; set; }
        string CUSTOMER_KEY { get; set; }
        DateTime DATE_KEY { get; set; }
        double ORDER_CASE { get; set; }
        int ORDER_DELIVERY_STATUS { get; set; }
        decimal ORDER_DISCOUNT { get; set; }
        decimal ORDER_GROSS_AMOUNT { get; set; }
        decimal ORDER_GROSS_WEIGHT { get; set; }
        decimal ORDER_NET_AMOUNT { get; set; }
        decimal ORDER_NET_WEIGHT { get; set; }
        string PLANT { get; set; }
        string PRODUCT_CODE { get; set; }
        DateTime REQUESTED_DELIVERY_DATE_KEY { get; set; }
        DateTime REQUESTED_SHIP_DATE_KEY { get; set; }
        DateTime FIRST_DAY_OF_WEEK { get; set; }
        string DAY_OF_WEEK { get; }
        int WEEK_NUMBER { get; }
        //string WEEK_ID { get; }

    }
}
