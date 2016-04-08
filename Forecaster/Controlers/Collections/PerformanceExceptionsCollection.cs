using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class TypeItemString
    {
        public string VALUE { get; set; }
        public override string ToString()
        {
            return VALUE;
        }
    }
    public class PerformanceExceptionsCollection : ObservableCollection<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS>, IDisposable
    {

        public PerformanceExceptionsCollection(ITab tab)
        {
            TAB = tab;
        }
        public PerformanceExceptionsCollection(List<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS> list, ITab tab)
            : base(list)
        {
            TAB = tab;            
            this.CollectionChanged += PerformanceExceptionsCollection_CollectionChanged;
        }

        public PerformanceExceptionsCollection(ITab tab, bool attachCollectionChangedEvent)            
        {
            TAB = tab;
            this.CollectionChanged += PerformanceExceptionsCollection_CollectionChanged;
        }
        //public PerformanceExceptionsCollection(IEnumerable<daisybrand.forecaster.datastores.PerformanceException> collection)
        //    : base(collection)
        //{
        //    this.CollectionChanged += PerformanceExceptionsCollection_CollectionChanged;
        //}


        //public static List<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS> Convert(List<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS> list)
        //{
        //    return list.Select(i => new datastores.FORECASTER_PERFORMANCES_EXCEPTIONS
        //    {
        //        GUID = i.GUID,
        //        CUSTOMER_ID = i.CUSTOMER_ID,
        //        DESCRIPTION = i.DESCRIPTION,
        //        FROM = i.FROM,
        //        INCLUDE = i.INCLUDE,
        //        IS_DEFAULT = i.IS_DEFAULT,
        //        PRICE = i.PRICE,
        //        QUANTITY = i.QUANTITY,
        //        SKU = i.SKU,
        //        TO = i.TO,
        //        TYPE = i.TYPE,
               
        //    }).ToList();
        //}
        void PerformanceExceptionsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                
                //var newItems = e.NewItems;
                //this.IsAdding = true;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var old = e.OldItems[0] as datastores.FORECASTER_PERFORMANCES_EXCEPTIONS;       
                TAB.myWeeklyData.RemovePerformance(old.GUID);
                TAB.myPerformanceExceptions.RemoveGuid(old.GUID);
            }
        }

        public void RemoveGuid(Guid guid)
        {
            var items = this.Where(x => x.GUID == guid);
            RemoveRange(items);
        }

        public void RemoveRange(IEnumerable<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS> collection)
        {
            var coll = new PerformanceExceptionsCollection(null);
            foreach (var item in collection)
            {
                coll.Add(item);
            }
            foreach (var p in coll)
            {
                this.Remove(p);
            }
        }

        #region properties

        private ITab TAB { get; set; }
        private datastores.FORECASTER_PERFORMANCES_EXCEPTIONS _LastItemAdded;
        private bool _IsAdding;

       
        public bool IsAdding
        {
            get
            {
                return _IsAdding;
            }
            set
            {
                _IsAdding = value;
            }
        }
        public datastores.FORECASTER_PERFORMANCES_EXCEPTIONS LastItemAdded
        {
            get
            {
                return _LastItemAdded;
            }
            set
            {
                _LastItemAdded = value;
            }
        }
        
        #endregion
        #region Async
        /// <summary>
        /// GETS THE PERFORMANCE EXCEPTIONS FOR THE ENTIRE COLLECTION
        /// </summary>
        public static async Task GetAsync(ITab tab)
        {
            var a = await _GetTask(tab);
            if (a != null)
            {
                tab.myPerformanceExceptions = a;
                //MainWindow.myPerformanceExceptions = a;
                ////UPDATE DAILY PERFORMACE RECORDS 
                //if (tab.myDailyData != null && !tab.myDailyData.isUpdating_PERFORMANCES && !tab.myDailyData.IsUpdated_PERFORMANCES)
                //{
                //    tab.myDailyData.UpdatePerformances(a);
                //}
                ////UPDATE WEEKLY PERFORMANCE RECORDS
                //if (tab.myWeeklyData != null && !tab.myWeeklyData.isUpdating_PERFORMANCES && !tab.myWeeklyData.IsUpdated_PERFORMANCES)
                //{
                //    tab.myWeeklyData.UpdatePerformances(a);
                //}
            }
        }

        private static Task<PerformanceExceptionsCollection> _GetTask(ITab tab)
        {
            return Task.Run<PerformanceExceptionsCollection>(() => _Get(tab));
        }

        private static PerformanceExceptionsCollection _Get(ITab tab)
        {
            try
            {
                List<datastores.FORECASTER_PERFORMANCES_EXCEPTIONS> list;
                var custId = int.Parse(tab.CUSTOMER_NUMBER);
                using (var data = new datastores.ForecasterEntities(null))
                {
                    list = data.FORECASTER_PERFORMANCES_EXCEPTIONS.Where(i => i.CUSTOMER_ID == custId && i.SKU.ToUpper() == tab.SKU.SKUID).ToList();
                }

                if (list != null && list.Count() > 0)
                    return new PerformanceExceptionsCollection(list, tab);

            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, String.Format("Unable to collect performance exceptions for {0} customer {1}", tab.SKU.SKUID, tab.CUSTOMER_NUMBER));
            }
            return new PerformanceExceptionsCollection(tab, true);
        }
        #endregion


        public void OnStateChanged(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS item)
        {
            if (!Tools.IsInEditMode()) return;
            using (var data = new datastores.ForecasterEntities(null))
            {
                var items = data.FORECASTER_PERFORMANCES_EXCEPTIONS.Where(i => i.GUID == item.GUID);
                if (items != null && items.Count() > 0)
                {
                    foreach (var i in items)
                    {
                        if (i.STATE != item.STATE)
                        {
                            i.STATE = item.STATE ?? 0;
                            i.STATE_LAST_UPDATED = DateTime.Now;
                            i.STATE_USER = Environment.UserName;
                        }
                        //if (item.STATE == 0 && i.IS_DEFAULT && i.INCLUDE)
                        //    data.FORECASTER_PERFORMANCES_EXCEPTIONS.Remove(i);
                    }
                }
                else
                {
                    item.STATE_LAST_UPDATED = DateTime.Now;
                    item.STATE_USER = Environment.UserName;
                    item.INCLUDE = true;
                    data.FORECASTER_PERFORMANCES_EXCEPTIONS.Add(item);
                }             
                data.SaveChanges();
            } 
        }
        public void DeleteItem(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS item)
        {
            if (!Tools.IsInEditMode()) return;  
            using (var data = new datastores.ForecasterEntities(null))
            {
                var items = data.FORECASTER_PERFORMANCES_EXCEPTIONS.Where(i => i.GUID == item.GUID);
                //data.FORECASTER_DELETE_PERFORMANCES_EXCEPTIONS(item.GUID);
                foreach (var i in items)
                    data.FORECASTER_PERFORMANCES_EXCEPTIONS.Remove(i);
                data.SaveChanges();
            }            
        }

        public void InsertOrUpdateItem(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS item)
        {
            if (!Tools.IsInEditMode()) return;                      
            using (var data = new datastores.ForecasterEntities(null))
            {
                var items = data.FORECASTER_PERFORMANCES_EXCEPTIONS.Where(i => i.GUID == item.GUID);
                if (items != null && items.Count() > 0)
                    foreach (var i in items)
                    {
                        i.CUSTOMER_ID = item.CUSTOMER_ID;
                        i.SKU = item.SKU;
                        i.TYPE = item.TYPE;
                        i.FROM = item.FROM;
                        i.TO = item.TO;
                        i.QUANTITY = item.QUANTITY;
                        i.PRICE = item.PRICE;
                        i.DESCRIPTION = item.DESCRIPTION;
                        i.INCLUDE = item.INCLUDE;
                        i.IS_DEFAULT = item.IS_DEFAULT;
                        if (i.STATE != item.STATE)
                        {
                            i.STATE = item.STATE ?? 0;
                            i.STATE_LAST_UPDATED = DateTime.Now;
                            i.STATE_USER = Environment.UserName;
                        }

                    }
                else
                {
                    item.STATE_USER = Environment.UserName;
                    item.STATE_LAST_UPDATED = DateTime.Now;
                    data.FORECASTER_PERFORMANCES_EXCEPTIONS.Add(item);
                }
                var errors = data.GetValidationErrors();
                if (errors.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var ers in errors)
                        foreach (var er in ers.ValidationErrors)
                            sb.Append(er.ErrorMessage + "\r");
                    LogManger.RaiseErrorMessage(new Objects.Message { MESSAGE = sb.ToString() });
                }
                else
                    data.SaveChanges();
            }
        }

        #region IDisposable Members
        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                LastItemAdded = null;
            }

            // Free any unmanaged objects here. 
            //
            this.CollectionChanged -= PerformanceExceptionsCollection_CollectionChanged;
            
            disposed = true;
        }
        #endregion
       
    }



}
