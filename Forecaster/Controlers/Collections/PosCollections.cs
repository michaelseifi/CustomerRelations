using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using System.Windows;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class PosCollections : ObservableCollection<IPos>
    {
        public static CancellationTokenSource PosCancellationTokenSource;
        public static string[] KrogerDivisions;
        
        
        public PosCollections()
        {
            
        }
        public PosCollections(List<IPos> list)
            : base(list)
        {
            
        }
        public PosCollections(IEnumerable<IPos> collection)
            : base(collection)
        {
            
        }

        public static async void GetAsync(ITab tab)
        {
            PosCollections.PosCancellationTokenSource = new CancellationTokenSource();
            try
            {
                if (tab.myGraphViewModel == null) return;
                tab.myGraphViewModel.IS_POSCOLLECTION_LOADING = true;

                //if (KrogerDivisions != null && KrogerDivisions.Count() > 0)
                //{
                    if (tab == null) return;
                    //var a = await _GetTask(KrogerDivisions, tab.SKU.SKUID, PosCancellationTokenSource);
                    var a = await _GetFromServiceTask(tab.CUSTOMER_NUMBER, tab.SKU.SKUID, PosCancellationTokenSource);
                    if (tab == null || tab.myGraphViewModel == null) return;
                    tab.myGraphViewModel.POSCOLLECTION = a;

                    if (a == null)
                        tab.myGraphViewModel.SetWEEKLY_POS_CASES_CONTENT_ErrorText();

                    tab.myGraphViewModel.IS_POSCOLLECTION_LOADED = true;
                //}

                if (tab == null || tab.myGraphViewModel == null) return;
                tab.myGraphViewModel.IS_POSCOLLECTION_LOADING = false;
            }
            catch (Exception ex)
            {
                LogManger.Insert("Error", "Failed to run GetAsync in pos collection");
                LogManger.Insert(ex);
                if (ex.InnerException != null)
                    LogManger.Insert(ex.InnerException);
            }
        }

        //private static Task<PosCollections> _GetTask(string[] divisions, string sku, CancellationTokenSource cs)
        //{
        //    try
        //    {
        //        var t = Task.Run<PosCollections>(() => _Get(divisions, sku), cs.Token);
        //        return t;
        //    }
        //    catch (TaskCanceledException tx)
        //    {
        //        Log.Insert("Error", "Task canceled for _GetTask at POSCollection");
        //        Log.Insert(tx);
        //    }
        //    return null;
        //}

        //private static PosCollections _Get(string[] divisions, string sku)
        //{
        //    if (divisions.Count() > 0)
        //        return PosCollections._GetCollection(divisions, sku);                
        //    return null;
        //}

        //private static PosCollections _GetCollection(string[] divisionNumbers, string sku)
        //{
        //    try
        //    {
        //        PosCollections coll = new PosCollections();
        //        Dictionary<string, List<datastores.FORECASTER_GET_KROGER_CASES_BY_DIVISION_Result>> dic = new Dictionary<string, List<datastores.FORECASTER_GET_KROGER_CASES_BY_DIVISION_Result>>(divisionNumbers.Count());
        //        //using (var context = new datastores.DaisyDWEntities())
        //        using (var context = new datastores.ForecasterEntities(null))
        //        {
        //            foreach (string s in divisionNumbers)
        //            {
        //                //var list = context.FORECASTER_Get_KrogerPOSData(s).Where(x => x.SKU == sku).OrderByDescending(x => x.Date).GroupBy(x => x.Date);
        //                var list = context.FORECASTER_GET_KROGER_CASES_BY_DIVISION(s).Where(x => x.SKU == sku).OrderByDescending(x => x.Date).ToList();
        //                dic.Add(s, list);
                        
        //            }
        //        }
        //        if(dic.Count() > 0)
        //            foreach (var item in dic)
        //            {
        //                var groups = item.Value.GroupBy(x => x.Date);
        //                foreach(var group in groups)
        //                coll.Add(new Pos
        //                {
        //                    CASES = Convert.ToDecimal(group.Sum(x => x.SalesQuantityCases) ?? 0),
        //                    DATE = group.Key ?? new DateTime(1990, 1, 1),
        //                    //DIVISION_NAME = group.Select(x => x.KrogerDivisionName).FirstOrDefault(),
        //                    //DIVISION_NUMBER = item.Key, //group.Select(x => x.KrogerDivisionNumber).FirstOrDefault(),
        //                    FIRST_DAY_OF_WEEK = group.Key == null ? new DateTime(1990, 1, 1) : Convert.ToDateTime(group.Key).StartOfWeek(System.DayOfWeek.Sunday),
        //                });
        //            }
        //        return coll;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Insert("Error", "Failed to run _GetCollection in pos collection");
        //        Log.Insert(ex);                
        //        return null;
        //    }
        //}

        private static Task<PosCollections>  _GetFromServiceTask(string customerNum, string sku, CancellationTokenSource cs)
        {
            try
            {
                return Task.Run<PosCollections>(() => _GetFromService(customerNum, sku), cs.Token);
            }
            catch (TaskCanceledException tx)
            {
                LogManger.Insert("Error", "Task canceled for _GetTask at POSCollection");
                LogManger.Insert(tx);
            }
            return null;
        }

        private static PosCollections _GetFromService(string customerNum, string sku)
        {
            var coll = new PosCollections();
            var client = new Services.Forecaster.ForecasterClient();
            try
            {
                var list = client.GetKrogerPOSData(customerNum, sku, Environment.UserName, null).GroupBy(i => i.SKU);
                if (list == null || list.Count() == 0) return null;
                foreach (var gSku in list)
                {
                    var gDates = gSku.OrderByDescending(i=>i.Date).GroupBy(i => i.Date);
                    foreach (var gDate in gDates)
                    {
                        if (gDate.Key != null)
                        {
                            coll.Add(new Pos
                            {
                                CASES = Convert.ToDecimal(gDate.Sum(x => x.SalesQuantityCases) ?? 0),
                                DATE = gDate.Key ?? new DateTime(1990, 1, 1),
                                FIRST_DAY_OF_WEEK = gDate.Key == null ? new DateTime(1990, 1, 1) : Convert.ToDateTime(gDate.Key).StartOfWeek(System.DayOfWeek.Sunday),
                            });
                        }
                    }
                }
                return coll;

            }
            catch (Exception ex)
            {
                LogManger.Insert("Error", "Failed to run _GetCollection in pos collection");
                LogManger.Insert(ex);
                return null;
            }
            finally
            {
                if (client.State != System.ServiceModel.CommunicationState.Closed)
                    client.Close();
            }
        }
        public static Task<string[]> GetDivisionsTask(int customerNumber)
        {
            return Task.Run<string[]>(() => _GetDivisions(customerNumber));
        }

        private static string[] _GetDivisions(int customerNumber)
        {

            try
            {
                List<datastores.FORECASTER_KROGER_DIVISION_LIST> list;
                using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_KROGER_DIVISION_LISTTableAdapters.FORECASTER_KROGER_DIVISION_LISTTableAdapter())
                {
                    list = context.FORECASTER_KROGER_DIVISION_LIST.Where(i => i.TP == customerNumber).ToList();
                }

                if (list != null && list.Count() > 0)
                    KrogerDivisions = list.Select(i => i.MARKET).ToArray();
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get the list of kroger division");
            }
            return KrogerDivisions;
        }

    }
}
