using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using Parent = daisybrand.forecaster;
using daisybrand.forecaster.Controlers.Objects;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using WpfAnimatedGif;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class PerformanceCollection : ObservableCollection<daisybrand.forecaster.Controlers.Interfaces.IPerformance>
    {
        public PerformanceCollection()
        {
            
        }

        public PerformanceCollection(string weekId)
        {
            WEEKID = weekId;
        }
        public PerformanceCollection(List<daisybrand.forecaster.Controlers.Interfaces.IPerformance> list, string weekId)
            : base(list)
        {
            WEEKID = weekId;
        }
        public PerformanceCollection(IEnumerable<daisybrand.forecaster.Controlers.Interfaces.IPerformance> collection, string weekId)
            : base(collection)
        {
            WEEKID = weekId;
        }
        public PerformanceCollection(List<daisybrand.forecaster.Controlers.Interfaces.IPerformance> list)
            : base(list)
        {
            
        }
        public PerformanceCollection(IEnumerable<daisybrand.forecaster.Controlers.Interfaces.IPerformance> collection)
            : base(collection)
        {
            
        }

        public static PerformanceCollection Get(int customerId, string skuId)
        {
            try
            {
                PerformanceCollection coll = null;
                //List<datastores.VMI_Get_Performance_By_CustomerAccountNum_Result> data;
                //using (var context = new datastores.dbidbEntities())                
                //{
                //    data = context.VMI_Get_Performance_By_CustomerAccountNum(customerId.AddLeadingZeros(), skuId).ToList();
                //}


                //this service pulls performances from Performances table in forecaster db
                //the table get updated at 3:15 am and 12 noon daily by a sql job
                Services.Promotions.FORECASTER_PROMOTIONS[] data = null;
                using (var client = new Services.Promotions.ForecasterClient())
                {
                    var customerNum = customerId.AddLeadingZeros();
                    data = client.GetDataBySku(customerNum, skuId, null);
                }
                
                if (data != null && data.Count() > 0)
                {
                    var list = data.Select(x => new Performance
                    {
                        
                        CONFIRMED_BY = x.ConfirmedBy == null ? string.Empty : x.ConfirmedBy,
                        CONFIRMED_DATE = x.ConfirmedDate ?? new DateTime(1900, 1, 1),
                        CONFIRMED_DATE_STR = x.ConfirmedDate == null ? string.Empty : x.ConfirmedDate.ToString(),
                        CUSTOMER_ID = x.DirectNumber,
                        END_DATE = x.EndDate,
                        IS_CONFIRMED = x.ConfirmedDate != null,
                        TYPE = x.PerformanceType,
                        TYPE_ID = x.PerformanceTypeID,
                        PRICE = x.Price,
                        PROMOTION_NUMBER = x.PromotionNumber,
                        QUANTITY = x.Quantity,
                        SKU_ID = x.SKUID,
                        START_DATE = x.StartDate,
                        AD_NAME = x.Name,
                        //CR_ASSOCIATE = MainWindow.myIdentity.Name,
                        DOCUMENTOBJECTID = x.ObjectID,
                        STATUS = x.TradeStatusCode,
                        BANNERS = new string[] { x.IndirectName },
                        CREATED_BY = x.CreatedBy,
                        CREATED_DATE = x.CreatedDate,
                        MODIFIED_BY = x.LastModifiedBy ?? x.CreatedBy,
                        MODIFIED_DATE = x.LastModifiedDate ?? x.CreatedDate
                    });
                    coll = new PerformanceCollection(list.OrderByDescending(x => x.START_DATE), null);
                }

                return coll ?? new PerformanceCollection();
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                return new PerformanceCollection();
            }
        }

        public static PerformanceCollection New(IEnumerable<daisybrand.forecaster.Controlers.Interfaces.IPerformance> collection, string weekId, DateTime reportAsDate)            
        {
            if (collection == null || collection.Count() == 0) return new PerformanceCollection(weekId);
            var coll = new PerformanceCollection(collection, weekId); 
            coll.Update(x => x.WEEKID = weekId);
            coll.Update(x => x.REPORT_AS_OF_DATE = reportAsDate);
            coll.Update(x => x.IS_INCLUDED = true);
            coll.Update(x => x.IS_DEFAULT = true);            
            
            return coll;                         
        }

        public static PerformanceCollection New(IEnumerable<daisybrand.forecaster.Controlers.Interfaces.IPerformance> collection, string weekId, DateTime reportAsDate, IEnumerable<DateTime> dateColl)
        {
            if (collection == null || collection.Count() == 0) return new PerformanceCollection(weekId);
            PerformanceCollection perfColl = new PerformanceCollection(weekId);
            foreach (Controlers.Interfaces.IPerformance iPerf in collection)
            {
                foreach (DateTime date in dateColl)
                {
                    if (iPerf.START_DATE <= date && iPerf.END_DATE >= date)
                    {
                        perfColl.Add(iPerf);
                        break;
                    }
                }
            }
            return perfColl;
        }

        public static async void UpdateWithBannersAsync(string customerNumber, DataCollection data)
        {
            var a = await _UpdateWithBannersTask(customerNumber, data);
            if (!a)
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "System was unable to update banners. for " + data.SKUID.SKUID });
                //throw new ApplicationException("Unable to update banners");
        }

        private static Task<bool> _UpdateWithBannersTask(string customerNumber, DataCollection data)
        {
            return Task.Run<bool>(() => _UpdateWithBanners(customerNumber, data));
        }

        private static bool _UpdateWithBanners(string customerNumber, DataCollection data)
        {
            try
            {
                //GET BANNERS
                List<datastores.DBI_GetBannerByDirectCustomer_Result> list;
                using (var context = new datastores.DAX_PRODEntities(null))
                {
                    list = context.DBI_GetBannerByDirectCustomer(customerNumber).ToList();                    
                }
                if (list != null && list.Count() > 0)
                {
                    var bNames = list.Select(x => x.BannerName).ToArray();
                    foreach (var item in data.PERFORMANCES)
                        item.BANNERS = bNames;                    
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "_UpdateWithBanners");                
            }
            return false;
        }


        #region properties

        public IPerformance FIRST
        {
            get
            {
                var ads = this.Where(x => x.PERFORMANCE_TYPE == "AD" && x.IS_INCLUDED == true).ToList();
                if (ads != null && ads.Count() > 0) return ads.OrderBy(x => x.PRICE).FirstOrDefault();
                return this.Count() > 0 ? this.Where(x => x.IS_INCLUDED == true).OrderBy(x => x.PRICE).FirstOrDefault() : null;
            }
        }

        public string FIRST_DISPLAY
        {
            get
            {
                var frst = FIRST;
                if (frst == null) 
                    return "ERROR: Please contact Michael";
                return String.Format("{1} - {2} - {3} - ${4} - {5}({6})", frst.PROMOTION_NUMBER, frst.START_DATE.ToShortDateString(), frst.END_DATE.ToShortDateString(), frst.QUANTITY, frst.PRICE
                , frst.PERFORMANCE_TYPE
                , Performance.GetState(frst));
            }
        }

        public object FIRST_DISPLAY_OBJ
        {
            get
            {
                var frst = FIRST;
                if ((frst.IS_DEFAULT && frst.D_STATE == Enums.DState.Confirmed)
                    || (!frst.IS_DEFAULT && frst.M_STATE == Enums.MState.Confirmed))
                {
                    var stack = new StackPanel { Orientation = Orientation.Horizontal };
                    var txtBlc = new TextBlock { Text = String.Format("{1} - {2} - {3} - ${4} - {5} ", frst.PROMOTION_NUMBER, frst.START_DATE.ToShortDateString(), frst.END_DATE.ToShortDateString(), frst.QUANTITY, frst.PRICE, frst.PERFORMANCE_TYPE) };
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Presentation/Resources/Images/Checkmark.png");
                    image.EndInit();
                    Image img = new Image() { Width = 16, Height = 16, Source = image };
                    stack.Children.Add(txtBlc);
                    stack.Children.Add(img);
                    return stack;
                }
                return null;
            }
        }


        public string TOOLTIP
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                
                var col = this.Where(x => x.IS_INCLUDED);
                foreach (daisybrand.forecaster.Controlers.Interfaces.IPerformance perf in col)
                {
                    //var guid = perf.DOCUMENTOBJECTID == Guid.Parse("3D8E4C3F-C65B-4A24-AB37-6CA6F84C7DA1");
                    //if (guid)
                    //{
                        try
                        {
                            var state = Performance.GetState(perf);
                            var banner = perf.BANNERS == null ? string.Empty : perf.BANNERS.ElementAt<string>(0);
                            i++;
                            sb.Append(String.Format("{5} - {6}({1} - {2} - {3} - ${4} - {7}({8}) - (Banner: {9})\r",
                                perf.PROMOTION_NUMBER, perf.START_DATE.ToShortDateString(), perf.END_DATE.ToShortDateString(), perf.QUANTITY, perf.PRICE, i, perf.PERFORMANCE_TYPE,
                                perf.IS_DEFAULT ? perf.STATUS : "Manual", state, banner));
                           
                        }
                        catch (Exception ex)
                        {
                            LogManger.Insert1(ex, "Error getting promotions for tooltip");
                        }
                    //}
                }
                var s = sb.ToString();
                return s;
            }
        }

        public bool ANY
        {
            get
            {
                return this.Count() > 0;
            }
        }

        public string WEEKID { get; set; }
        #endregion

    }
}
