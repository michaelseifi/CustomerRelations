using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class ActualOrderCollection:ObservableCollection<IOrder>
    {
        public ActualOrderCollection()
        {
            
        }
        public ActualOrderCollection(List<IOrder> list)
            : base(list)
        {
            
        }
        public ActualOrderCollection(IEnumerable<IOrder> collection)
            : base(collection)
        {

        }

        public void AddRange(IEnumerable<IOrder> collection)
        {
            this.Clear();
            foreach (var o in collection)
                this.Add(o);
        }
        #region async
        public static async void GetAync(ICustomer customer, ITab tab)
        {
            try
            {
                //MainWindow.myMainViewModel.SetMESSAGE("Loading actual orders from DAX ...");
                MainWindow.myTopMenuViewModel.IsActualOrderRereshing = true;
                var orders = await _GetItemUpcTask(customer, tab.SKU.SKUID);
                _UpdateItem(orders, tab);
                MainWindow.myTopMenuViewModel.IsActualOrderRereshing = false;
            }
            catch (Exception ex)
            {

            }
        }

        //public static async void GetItemUpcAync(ICustomer customer, string itemUpc)
        //{
        //    MainWindow.myMainViewModel.SetMESSAGE(String.Format("Loading actual orders for {0} from DAX ...", itemUpc));
        //    MainWindow.myMainViewModel.IsActualOrderRereshing = true;
        //    var orders = await GetItemUpcTask(customer, itemUpc);
        //    UpdateItem(orders, itemUpc);
        //    MainWindow.myMainViewModel.IsActualOrderRereshing = false;
        //}

        private static Task<ActualOrderCollection> _GetTask(ICustomer customer)
        {
            return Task.Run<ActualOrderCollection>(() => _Get(customer));
        }

        private static Task<ActualOrderCollection> _GetItemUpcTask(ICustomer customer, string itemUpc)
        {
            return Task.Run<ActualOrderCollection>(() => _GetItemUpc(customer, itemUpc));
        }

        public static Task<bool> UpdateCollectionTask(ActualOrderCollection orders)
        {
            return Task.Run<bool>(() => _UpdateCollection(orders));
        }

        public static Task<bool> UpdateItemTask(ActualOrderCollection orders, ITab tab)
        {
            return Task.Run<bool>(() => _UpdateItem(orders, tab));
        }
        private static ActualOrderCollection _Get(ICustomer customer)
        {
            try
            {
                List<datastores.FORECASTER_GET_ACTUAL_ORDERS_Result> list;
                using (var context = new datastores.DAX_PRODEntities(null))
                //new forecaster.datastores.FORECASTER_GET_ACTUAL_ORDERSTableAdapters.FORECASTER_GET_ACTUAL_ORDERSTableAdapter())
                {
                    list = context.FORECASTER_GET_ACTUAL_ORDERS(customer.ACCOUNTNUM, DateTime.Now.Year - 2).ToList();
                }
                if (list != null && list.Count() > 0)
                {
                    List<IOrderStatus> SalesOrderStatusList = OrderStatus.SalesOrderStatusList;
                    return new ActualOrderCollection(list.Select(x => new Order
                     {
                         SALES_ID = x.SALESID,
                         CUSTOMER_ID = x.ship_to_tp,
                         DELIVERY_DATE = x.DELIVERYDATE,
                         ITEM_UPC = x.item_upc,
                         ORDER_DATE = x.order_date,
                         PURCHASE_ORDER = x.PURCHORDERFORMNUM,
                         SALES_QTY = x.quantity,
                         NET_WEIGHT = x.netWeight,
                         GROSS_WEIGHT = x.gorssWeight ?? 0,
                         SALES_STATUS = x.SALESSTATUS,
                         SALES_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.Salesstatus.ToString().ToUpper() && s.VALUE == x.SALESSTATUS).NAME,
                         SALES_DETAILED_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.jsSalesOrderDetailStatus.ToString().ToUpper() && s.VALUE == x.DETAILEDSTATUS).NAME,
                         //SALES_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.Salesstatus, x.SALESSTATUS),
                         //SALES_DETAILED_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.jsSalesOrderDetailStatus, x.DETAILEDSTATUS),
                         SALES_DETAILED_STATUS = x.DETAILEDSTATUS,
                         LINE_ITEM_DETAILED_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.jsSalesLineDetailStatus.ToString().ToUpper() && s.VALUE == x.LINEITEMDETAILEDSATATUS).NAME,
                         //LINE_ITEM_DETAILED_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.jsSalesLineDetailStatus, x.LINEITEMDETAILEDSATATUS),
                         LINE_ITEM_DETAILED_STATUS = x.LINEITEMDETAILEDSATATUS,
                         LINE_ITEM_SALES_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.Salesstatus.ToString().ToUpper() && s.VALUE == x.LINEITEMSALESSTATUS).NAME,
                         //LINE_ITEM_SALES_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.Salesstatus, x.LINEITEMSALESSTATUS),
                         LINE_ITEM_SALES_STATUS = x.LINEITEMSALESSTATUS,
                         REMAINSALESPHYSICAL = x.REMAINSALESPHYSICAL
                     }));
                }
            }
            catch (SqlException se)
            {
                LogManger.Insert1(se, "ActualOrderCollection");
            }
            return null;
        }

        private static ActualOrderCollection _GetItemUpc(ICustomer customer, string itemUpc)
        {
            try
            {
                List<datastores.FORECASTER_GET_ACTUAL_ORDERS_Result> list;
                using (var context = new datastores.DAX_PRODEntities(null))
                //new forecaster.datastores.FORECASTER_GET_ACTUAL_ORDERSTableAdapters.FORECASTER_GET_ACTUAL_ORDERSTableAdapter())
                {
                    list = context.FORECASTER_GET_ACTUAL_ORDERS(customer.ACCOUNTNUM, DateTime.Now.Year - 2).Where(x => x.item_upc == itemUpc).ToList();
                }
                if (list != null && list.Count() > 0)
                {
                    List<IOrderStatus> SalesOrderStatusList = OrderStatus.SalesOrderStatusList;
                    return new ActualOrderCollection(list.Select(x => new Order
                   {
                       SALES_ID = x.SALESID,
                       CUSTOMER_ID = x.ship_to_tp,
                       DELIVERY_DATE = x.DELIVERYDATE,
                       ITEM_UPC = x.item_upc,
                       ORDER_DATE = x.order_date,
                       PURCHASE_ORDER = x.PURCHORDERFORMNUM,
                       SALES_QTY = x.quantity,
                       NET_WEIGHT = x.netWeight,
                       GROSS_WEIGHT = x.gorssWeight ?? 0,
                       SALES_STATUS = x.SALESSTATUS,
                       SALES_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.Salesstatus.ToString().ToUpper() && s.VALUE == x.SALESSTATUS).NAME,
                       SALES_DETAILED_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.jsSalesOrderDetailStatus.ToString().ToUpper() && s.VALUE == x.DETAILEDSTATUS).NAME,
                       //OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.Salesstatus, x.SALESSTATUS),
                       //SALES_DETAILED_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.jsSalesOrderDetailStatus, x.DETAILEDSTATUS),
                       SALES_DETAILED_STATUS = x.DETAILEDSTATUS,
                       LINE_ITEM_DETAILED_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.jsSalesLineDetailStatus.ToString().ToUpper() && s.VALUE == x.LINEITEMDETAILEDSATATUS).NAME,
                       //LINE_ITEM_DETAILED_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.jsSalesLineDetailStatus, x.LINEITEMDETAILEDSATATUS),
                       LINE_ITEM_DETAILED_STATUS = x.LINEITEMDETAILEDSATATUS,
                       LINE_ITEM_SALES_STATUS_NAME = SalesOrderStatusList.Find(s => s.ENUMID.ToUpper() == OrderStatus.ENUMIDenum.Salesstatus.ToString().ToUpper() && s.VALUE == x.LINEITEMSALESSTATUS).NAME,
                       //LINE_ITEM_SALES_STATUS_NAME = OrderStatus.GetNameFromMainDispatcher(OrderStatus.ENUMIDenum.Salesstatus, x.LINEITEMSALESSTATUS),
                       LINE_ITEM_SALES_STATUS = x.LINEITEMSALESSTATUS,
                       REMAINSALESPHYSICAL = x.REMAINSALESPHYSICAL
                   }));
                }
            }
            catch (SqlException se)
            {
                LogManger.Insert1(se, "ActualOrderCollection");
                LogManger.RaiseErrorMessage(new Message { MESSAGE = se.Message });
            }
            return null;
        }
        #endregion

        #region privates

        static bool _UpdateCollection(ActualOrderCollection coll)
        {
            
            return false;
        }

        static bool _UpdateItem(ActualOrderCollection coll, ITab tab)
        {
            //FILTER ACTUAL ORDERS
            var filteredOrders = coll.Filter();
            tab.myWeeklyData.Update(x => x.ORDER_DELIVERY =
                filteredOrders.Where(o => o.WEEK_DATE_DELIVERY == x.REPORT_AS_OF_DATE && o.ITEM_UPC == x.ITEM_UPC)
                    .Select(o => o.QUANTITY).Sum());
            
            return false;
        }

  
        #endregion



    }

    public static class _ActualOrderCollection
    {
        /// <summary>
        /// <para>FILTER OUT:</para>
        /// <para>-o.LINE_ITEM_DETAILED_STATUS of 3</para>
        /// <para>-o.LINE_ITEM_DETAILED_STATUS of 4</para>
        /// <para>-o.SALES_DETAILED_STATUS of 3</para>
        /// <para>-o.SALES_DETAILED_STATUS of 15</para>
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IEnumerable<IOrder> Filter(this ActualOrderCollection coll)
        {
            return coll.Where(o => o.LINE_ITEM_DETAILED_STATUS != 3 &&
                o.LINE_ITEM_DETAILED_STATUS != 4 &&
                o.SALES_DETAILED_STATUS != 3 &&
                o.SALES_DETAILED_STATUS != 15);
        }
    }
}
