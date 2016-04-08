using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IOrder
    {
        string SALES_ID { get; set; }
        string CUSTOMER_ID { get; set; }
        string ITEM_UPC { get; set; }
        DateTime ORDER_DATE { get; set; }
        DateTime DELIVERY_DATE { get; set; }
        decimal SALES_QTY { get; set; }
        decimal QUANTITY { get; }
        decimal REMAINSALESPHYSICAL { get; set; }
        decimal NET_WEIGHT { get; set; }
        decimal GROSS_WEIGHT { get; set; }
        string PURCHASE_ORDER { get; set; }

        DateTime WEEK_DATE { get;}
        DateTime WEEK_DATE_DELIVERY { get; }

        int SALES_STATUS { get; set; }
        string SALES_STATUS_NAME { get; set; }
        int SALES_DETAILED_STATUS { get; set; }
        string SALES_DETAILED_STATUS_NAME { get; set; }
        int LINE_ITEM_SALES_STATUS { get; set; }
        string LINE_ITEM_SALES_STATUS_NAME { get; set; }
        int LINE_ITEM_DETAILED_STATUS { get; set; }
        string LINE_ITEM_DETAILED_STATUS_NAME { get; set; }

    }
}
