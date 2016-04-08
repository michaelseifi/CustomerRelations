using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Extensions;

namespace daisybrand.forecaster.Controlers.Objects
{
    public class OrderCase : daisybrand.forecaster.Controlers.Interfaces.IOrderCase
    {
        public string BK_ORDER_NUMBER { get; set; }
        public DateTime DATE_KEY { get; set; }
        public DateTime REQUESTED_DELIVERY_DATE_KEY { get; set; }
        public DateTime CONFIRMED_DELIVERY_DATE_KEY { get; set; }
        public DateTime REQUESTED_SHIP_DATE_KEY { get; set; }
        public DateTime CONFIRMED_SHIP_DATE_KEY { get; set; }
        public DateTime ACTUAL_SHIP_DATE_KEY { get; set; }
        public string PRODUCT_CODE  { get; set; }
        public string PLANT { get; set; }
        public string CUSTOMER_KEY { get; set; }
        public decimal ORDER_GROSS_WEIGHT { get; set; }
        public decimal ORDER_NET_WEIGHT { get; set; }
        public decimal ORDER_GROSS_AMOUNT { get; set; }
        public decimal ORDER_DISCOUNT { get; set; }
        public decimal ORDER_NET_AMOUNT { get; set; }
        public double ORDER_CASE { get; set; }
        public DateTime ACTUAL_DELIVERY_DATE_KEY { get; set; }
        public int ORDER_DELIVERY_STATUS { get; set; }


        #region IOrder Members

        public DateTime FIRST_DAY_OF_WEEK
        {
            get;
            set;
        }

        public string DAY_OF_WEEK
        {
            get { return this.DATE_KEY.DayOfWeek.ToString(); }
        }

        public int WEEK_NUMBER
        {
            get { return this.DATE_KEY.WeekNumber(_Date.DayOfWeek(this.CUSTOMER_KEY)); }
        }

        //public string WEEK_ID
        //{
        //    get { return String.Format("{0}{1}{2}{3}", WEEK_NUMBER, DATE_KEY.Year, CUSTOMER_KEY, PRODUCT_CODE); }
        //}

        #endregion
    }
}
