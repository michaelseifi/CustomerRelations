using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class OrderStatusCollection:ObservableCollection<IOrderStatus>
    {
        public OrderStatusCollection()
        {
            
        }
        public OrderStatusCollection(List<IOrderStatus> list)
            : base(list)
        {
            
        }
        public OrderStatusCollection(IEnumerable<IOrderStatus> collection)
            : base(collection)
        {
            
        }

        #region async
        public static async void GetAync()
        {
            var list = await _GetTask();
            if (list != null)
            {
                App.myOrderStatusList = Thread.GetNamedDataSlot("myOrderStatusList");
                Thread.SetData(App.myOrderStatusList, list);
            }
            //test
            //var t = Tie.GetTiedOrderValue("RC0320", 1987.25M);
        }

        private static Task<OrderStatusCollection> _GetTask()
        {
            return Task.Run<OrderStatusCollection>(() => _Get());
        }

        private static OrderStatusCollection _Get ()
        {
            try
            {

                var list = new List<datastores.FORECASTER_GET_ORDERS_STATUSES_Result>();
                using (var context = new datastores.DAX_PRODEntities(null))
                //new forecaster.datastores.FORECASTER_GET_ORDERS_STATUSESTableAdapters.FORECASTER_GET_ORDERS_STATUSESTableAdapter())
                {
                    list = context.FORECASTER_GET_ORDERS_STATUSES().ToList();
                }
                if (list != null && list.Count() > 0)
                {
                    return new OrderStatusCollection(list.Select(x => new OrderStatus
                    {
                        ENUMID = x.ENUMID,
                        NAME = x.NAME,
                        VALUE = x.VALUE
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return null;
        }
        #endregion


    }
}
