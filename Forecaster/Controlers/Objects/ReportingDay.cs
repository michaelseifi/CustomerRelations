using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class ReportingDay
    {
        #region properties
        //public string ITEM_UPC { get; set; }
        private DayOfWeek _REPORT_END_DayOfWeek;
        private DayOfWeek _REPORT_START_DayOfWeek;
        private string _REPORT_END_DAY;
        private string _REPORT_START_DAY;
        private int _REPORT_DAYS;
        public int REPORT_DAYS
        {
            get
            {
                return _REPORT_DAYS;
            }
            set
            {
                _REPORT_DAYS = value;
            }
        }
        public string REPORT_START_DAY
        {
            get
            {
                return _REPORT_START_DAY;
            }
            set
            {
                REPORT_START_DayOfWeek = value.GetDayOfWeek(DayOfWeek.Sunday);
                _REPORT_START_DAY = REPORT_START_DayOfWeek.ToString();
            }
        }
        public string REPORT_END_DAY
        {
            get
            {
                return _REPORT_END_DAY;
            }
            set
            {
                REPORT_END_DayOfWeek = value.GetDayOfWeek(DayOfWeek.Saturday);
                _REPORT_END_DAY = REPORT_END_DayOfWeek.ToString();
            }
        }
        public DayOfWeek REPORT_START_DayOfWeek
        {
            get
            {
                return _REPORT_START_DayOfWeek;
            }
            set
            {
                _REPORT_START_DayOfWeek = value;
            }
            
        }

        public DayOfWeek REPORT_END_DayOfWeek
        {
            get
            {
                return _REPORT_END_DayOfWeek;
            }
            set
            {
                _REPORT_END_DayOfWeek = value;
            }
        }
        #endregion

        #region async
        //public static async void GetCollectionAsync()
        //{

        //    var a = await _GetCollectionTask();
        //    if (a != null)
        //    {
        //        App.myReportingDaysCollection = Thread.GetNamedDataSlot("ReportingDays");
        //        Thread.SetData(App.myReportingDaysCollection, a);
        //    }
        //}

        //static Task<List<ReportingDay>> _GetCollectionTask()
        //{
        //    return Task.Run<List<ReportingDay>>(() => _GetReportingDays());
        //}

        //static List<ReportingDay> _GetReportingDays()
        //{
        //    List<ReportingDay> list = null;
        //    using (var dt = new forecaster.datastores.FORECASTER_REPORTING_DAYSTableAdapters.FORECASTER_REPORTING_DAYSTableAdapter())
        //    {
        //        list = dt.GetData().Select(x => new ReportingDay
        //        {
        //            DAYS = x.DAYS,
        //            END = x.END,
        //            START = x.START,
        //            ITEM_UPC = x.ITEM_UPC
        //        }).ToList();

        //    }
        //    return list;
        //}
        #endregion



    }
}
