using System;
using System.Collections.Generic;
using System.Linq;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Order: IOrder
    {

        #region IOrder Members

        private DateTime _WEEK_DATE_DELIVERY;
        private DateTime _WEEK_DATE;
        private DateTime _DELIVERY_DATE;
        private DateTime _ORDER_DATE;

        public DateTime WEEK_DATE
        {
            get
            {
                return _WEEK_DATE;
            }            
        }

        public DateTime WEEK_DATE_DELIVERY
        {
            get
            {
                return _WEEK_DATE_DELIVERY;
            }
        }
        public string SALES_ID { get; set; }

        public string CUSTOMER_ID { get; set; }

        public string ITEM_UPC { get; set; }

        public DateTime ORDER_DATE
        {
            get
            {
                return _ORDER_DATE;
            }
            set
            {
                _ORDER_DATE = value;
                _WEEK_DATE = value.StartOfWeek(_Date.DayOfWeek(this.CUSTOMER_ID));
            }
        }

        public DateTime DELIVERY_DATE
        {
            get
            {
                return _DELIVERY_DATE;
            }
            set
            {
                _DELIVERY_DATE = value;
                _WEEK_DATE_DELIVERY = value.StartOfWeek(_Date.DayOfWeek(this.CUSTOMER_ID));
            }
        }

        public decimal QUANTITY
        {
            get
            {
                if (LINE_ITEM_SALES_STATUS == 1)
                    return SALES_QTY;
                return SALES_QTY - REMAINSALESPHYSICAL;
            }
        }

        public decimal NET_WEIGHT { get; set; }

        public decimal GROSS_WEIGHT { get; set; }

        public string PURCHASE_ORDER { get; set; }

        public int SALES_STATUS { get; set; }
        public string SALES_STATUS_NAME { get; set; }
        public decimal REMAINSALESPHYSICAL { get; set; }

        public int LINE_ITEM_SALES_STATUS { get; set; }

        public string LINE_ITEM_SALES_STATUS_NAME { get; set; }

        public int LINE_ITEM_DETAILED_STATUS { get; set; }

        public string LINE_ITEM_DETAILED_STATUS_NAME { get; set; }

        public int SALES_DETAILED_STATUS { get; set; }

        public string SALES_DETAILED_STATUS_NAME { get; set; }

        public decimal SALES_QTY { get; set; }
        #endregion



    }
}
