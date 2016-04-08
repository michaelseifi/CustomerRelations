using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Helpers;
using datastores = CRDataStore;


namespace daisybrand.forecaster.Controlers.Collections
{
    public class SkuCollection : ObservableCollection<ISku>
    {
        public SkuCollection()
        {
            
        }
        public SkuCollection(List<ISku> list)
            : base(list)
        {
            
        }
        public SkuCollection(IEnumerable<ISku> collection)
            : base(collection)
        {
            
        }

        public void AddRange(IEnumerable<ISku> collection)
        {
            foreach (ISku s in collection)
            {                
                this.Add(s);
            }
        }

        public static SkuCollection Get()
        {
            return VMI_Get_Customers_SkuIds();
        }
        private static SkuCollection VMI_Get_Customers_SkuIds()
        {
            SkuCollection s = new SkuCollection();
            try
            {
                List<datastores.VMI_Get_Customers_SkuIds_Result> list;
                using (var context = new datastores.dbidbEntities())
                //new DataStores.VMI_Get_Customers_SkuIdsTableAdapters.VMI_Get_Customers_SkuIdsTableAdapter())
                {

                    list = context.VMI_Get_Customers_SkuIds(DateTime.Now.Year - 1).ToList();
                }
                foreach (var row in list)
                {
                    Sku sku = new Sku();
                    ((ISku)sku).SHIPTO = row.ShipToTP;
                    ((ISku)sku).SKUID = row.ItemUPC;
                    s.Add(sku);
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to obtain a list of skus");
            }
            return s;
        }

        public static SkuCollection OrderByDisplayName(SkuCollection coll)
        {
            return new SkuCollection(coll.OrderBy(i => i.DISPLAY_VALUE));
        }

        /// <summary>
        /// ADDS EACH CUSTOMER'S SKUS TO THE CUSTOMER'S SKU PROPERTY
        /// </summary>
        public static async void GetUsersSkusAsync()
        {
            var a = await _GetUsersSkusTask(10);
            if (!a)
                throw new ApplicationException("Unable to retrieve user's skus");
        }

        private static Task<bool> _GetUsersSkusTask(int seconds)
        {
            return Task.Run<bool>(() => _GetUsersSkus(seconds));
        }

        private static bool _GetUsersSkus(int seconds)
        {
            //DateTime later = DateTime.Now.AddSeconds(seconds);
            //while (DateTime.Now < later)
            //{

            //}
            try
            {
                var users = MainWindow.myUsers;
                var skus = MainWindow.mySkus;

                foreach (IUser u in users)
                {
                    var cs = from c in u.CUSTOMERS
                             select int.Parse(c.ACCOUNTNUM).ToString();
                    u.SKUS.AddRange(skus.Where(x => cs.Contains(x.SHIPTO)));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DuelCollection<int, string> GetListOfExceptions()
        {
            var coll = new DuelCollection<int, string>();
            try
            {
                List<datastores.FORECASTER_SKU_EXCEPTIONS> list;
                using (var context = new datastores.ForecasterEntities(null))
                //new DataStores.FORECASTER_SKU_EXCEPTIONSTableAdapters.FORECASTER_SKU_EXCEPTIONSTableAdapter())
                {
                    list = context.FORECASTER_SKU_EXCEPTIONS.ToList();
                }
                if (list != null && list.Count() > 0)
                    foreach (var item in list)
                    {
                        coll.Add(new Duel<int, string>()
                        {
                            KEY = item.CUSTOMER_ID,
                            VALUE = item.SKU
                        });
                    }
                return coll;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "");
            }
            return coll;
        }

        public void AddToExceptions(ISku sku)
        {
            if (!Tools.IsInEditMode()) return;
            using (var context = new datastores.ForecasterEntities(null))
            {
                var item = new datastores.FORECASTER_SKU_EXCEPTIONS
                {
                    CUSTOMER_ID = int.Parse(sku.SHIPTO),
                    SKU = sku.SKUID
                };
                context.FORECASTER_SKU_EXCEPTIONS.Add(item);
                context.SaveChanges();
            }
        }

        public void RemoveFromExceptions(ISku sku)
        {
            if (!Tools.IsInEditMode()) return;
            using (var context = new datastores.ForecasterEntities(null))
            {
                int custId = int.Parse(sku.SHIPTO);
                var items = context.FORECASTER_SKU_EXCEPTIONS.Where(i => i.CUSTOMER_ID == custId && i.SKU == sku.SKUID);
                foreach (var i in items)
                    context.FORECASTER_SKU_EXCEPTIONS.Remove(i);
                context.SaveChanges();
            }
        }
    }
}
