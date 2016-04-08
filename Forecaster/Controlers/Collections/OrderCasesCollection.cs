using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;

namespace daisybrand.forecaster.Controlers.Collections
{
    public class OrderCasesCollection : ObservableCollection<daisybrand.forecaster.Controlers.Interfaces.IOrderCase>
    {
        public OrderCasesCollection()
        {
            
        }
        public OrderCasesCollection(List<daisybrand.forecaster.Controlers.Interfaces.IOrderCase> list)
            : base(list)
        {
            
        }
        public OrderCasesCollection(IEnumerable<daisybrand.forecaster.Controlers.Interfaces.IOrderCase> collection, System.DayOfWeek dayOfWeek)
            : base(collection)
        {
            DAY_OF_WEEK = dayOfWeek;
        }

        #region properties
        public System.DayOfWeek DAY_OF_WEEK { get; set; }
        #endregion


        public static async void GetAsync(ITab tab)
        {
            var a = await _GetTask(tab.CUSTOMER_NUMBER, tab.SKU.SKUID);
            if (a == null)
                throw new ApplicationException("Unable to retrieve customer's order data");
            if (tab.myGraphViewModel != null)
            {
                tab.myGraphViewModel.ORDERCOLLECTION = a;
                tab.myGraphViewModel.IS_ORDERCOLLECTION_LOADED = true;
            }

        }

        private static Task<OrderCasesCollection> _GetTask(string customerNumber, string sku)
        {
            return Task.Run<OrderCasesCollection>(() => _Get(customerNumber, sku));
        }

        private static OrderCasesCollection _Get(string customerNumber, string sku)
        {
            var coll = new OrderCasesCollection() { DAY_OF_WEEK = _Date.DayOfWeek(customerNumber) };
            try
            {
                using (var context = new datastores.DAX_PRODEntities(null))
                {
                    var orders = context.VMI_GetOrderCases(customerNumber, sku).OrderByDescending(x => x.RequestedShipDateKey.ToDateTime().StartOfWeek(coll.DAY_OF_WEEK));
                    foreach (var row in orders)
                    {
                        coll.Add(new daisybrand.forecaster.Controlers.Objects.OrderCase
                        {
                            ACTUAL_DELIVERY_DATE_KEY = row.ActualDeliveryDateKey.ToDateTime(),
                            ACTUAL_SHIP_DATE_KEY = row.ActualShipDateKey.ToDateTime(),
                            BK_ORDER_NUMBER = row.BKOrderNumber,
                            CONFIRMED_DELIVERY_DATE_KEY = row.ConfirmedDeliveyDateKey.ToDateTime(),
                            CONFIRMED_SHIP_DATE_KEY = row.ConfirmedShipDateKey.ToDateTime(),
                            CUSTOMER_KEY = row.CustomerKey,
                            DATE_KEY = row.DateKey.ToDateTime(),
                            ORDER_CASE = (double)row.OrderCases,
                            ORDER_DELIVERY_STATUS = row.OrderDeliveryStatus ?? 0,
                            ORDER_DISCOUNT = row.OrderDiscounts ?? 0,
                            ORDER_GROSS_AMOUNT = row.OrderGrossAmnt ?? 0,
                            ORDER_GROSS_WEIGHT = row.OrderGrossWeight ?? 0,
                            ORDER_NET_AMOUNT = row.OrderNetAmount ?? 0,
                            ORDER_NET_WEIGHT = row.OrderNetWeight ?? 0,
                            PLANT = row.Plant,
                            PRODUCT_CODE = row.ProductCode,
                            REQUESTED_DELIVERY_DATE_KEY = row.RequestedDeliveryDateKey.ToDateTime(),
                            REQUESTED_SHIP_DATE_KEY = row.RequestedShipDateKey.ToDateTime(),
                            FIRST_DAY_OF_WEEK = row.RequestedShipDateKey.ToDateTime().StartOfWeek(coll.DAY_OF_WEEK)
                        });
                    }
                    return coll;
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get order case collection");
                return null;
            }
            
        }
    }
}
