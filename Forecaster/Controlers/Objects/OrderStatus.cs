using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Helpers;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class OrderStatus:IOrderStatus
    {
        public enum ENUMIDenum
        {
            CustTransRefType,
            CustVendPaymStatus,
            DocumentStatus,
            InventTransferStatus,
            InventTransPostingType,
            InventTranstype,
            jsDBContactPriority,
            jsDBContactRole,
            jsDBCustInvoiceMethod,
            jsDBSettlementStatus,
            jsSalesLineDetailStatus,
            jsSalesOrderDetailStatus,
            LedgerAccountType,
            LedgerColumnTypeDim,
            LedgerJournalType,
            LedgerPostingType,
            LedgerTransType,
            OriginalDocument,
            Salesstatus,
            SalesType,
            SalesUpdate,
            StatusIssue,
            Weekdays
        }
        //public static CancellationTokenSource GetNameFromMainDispatcherCancellationTokenSource;
        public static List<IOrderStatus> SalesOrderStatusList;
        #region IOrderStatus Members

        public string ENUMID { get; set; }

        public int VALUE { get; set; }

        public string NAME { get; set; }

        #endregion
        #region publics
        /// <summary>
        /// RETURNS NAME OR EMPTY STRING
        /// </summary>
        /// <param name="enumId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //public static string GetName(ENUMIDenum enumId, int value)
        //{
        //    LocalDataStoreSlot reader = Thread.GetNamedDataSlot("myOrderStatusList");
        //    var list = (Thread.GetData(reader) as IEnumerable<IOrderStatus>).ToList();
        //    var statuses = list.Where(x => x.ENUMID.ToUpper() == enumId.ToString().ToUpper() && x.VALUE == value);
        //    if (statuses != null && statuses.Count() > 0)
        //        return statuses.FirstOrDefault().NAME;
        //    return string.Empty;
        //}


        public static List<IOrderStatus> GetNameFromMainDispatcher()
        {
            try
            {
                List<IOrderStatus> list = new List<IOrderStatus>();                
                var op = App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    LocalDataStoreSlot reader = Thread.GetNamedDataSlot("myOrderStatusList");
                    list.AddRange(Thread.GetData(reader) as IEnumerable<IOrderStatus>);
                   
                }));
                op.Completed += op_Completed;
                App.myDispatchOperations.Add(op);
                
                return list;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }
        /// <summary>
        /// RETURNS NAME OR EMPTY STRING
        /// </summary>
        /// <param name="enumId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNameFromMainDispatcher(ENUMIDenum enumId, int value)
        {
            try
            {
                string name = string.Empty;
                //GetNameFromMainDispatcherCancellationTokenSource = new CancellationTokenSource();
                var op = App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                                               {
                                                   LocalDataStoreSlot reader = Thread.GetNamedDataSlot("myOrderStatusList");
                                                   var list = (Thread.GetData(reader) as IEnumerable<IOrderStatus>).ToList();
                                                   var statuses = list.Where(x => x.ENUMID.ToUpper() == enumId.ToString().ToUpper() && x.VALUE == value);
                                                   if (statuses != null && statuses.Count() > 0)
                                                       name = statuses.FirstOrDefault().NAME;

                                               }));
                op.Completed += op_Completed;
                App.myDispatchOperations.Add(op);  
                return name;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        static void op_Completed(object sender, EventArgs e)
        {
            var op = sender as DispatcherOperation;
            App.myDispatchOperations.Remove(op);
        }

        /// <summary>
        /// RETURNS LIST OR NULL
        /// </summary>
        /// <param name="enumId"></param>
        /// <returns></returns>
        public static IEnumerable<IOrderStatus> GetList(ENUMIDenum enumId)
        {
            try
            {
                LocalDataStoreSlot reader = Thread.GetNamedDataSlot("myOrderStatusList");
                var thread = Thread.GetData(reader);
                var list = (thread as IEnumerable<IOrderStatus>).ToList();
                return list.Where(x => x.ENUMID.ToUpper() == enumId.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get the list of order statuses from local data store");
            }
            return null;
        }

        /// <summary>
        /// RETURNS LIST OR NULL
        /// </summary>
        /// <param name="enumId"></param>
        /// <returns></returns>
        //public static IEnumerable<IOrderStatus> GetListFromMainDispatcher(ENUMIDenum enumId)
        //{
        //    IEnumerable<IOrderStatus> ilist = null;
        //    try
        //    {                
        //        App.Current.Dispatcher.Invoke(() =>
        //                        {
        //                            LocalDataStoreSlot reader = Thread.GetNamedDataSlot("myOrderStatusList");
        //                            var thread = Thread.GetData(reader);
        //                            var list = (thread as IEnumerable<IOrderStatus>).ToList();
        //                            ilist = list.Where(x => x.ENUMID.ToUpper() == enumId.ToString().ToUpper());                   
        //                        });

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Insert(ex, "Unable to get the list of order statuses from local data store");
        //    }
        //    return ilist;
        //}
        #endregion

    }
}
