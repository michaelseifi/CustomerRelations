using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using daisybrand.forecaster.Controlers.Fields;
using daisybrand.forecaster.Controlers.Collections;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IDailyData
    {
        uint INDEX { get; set; }
        ICommand REFRESH_COMMAND { get; }
        bool CANEXECUTE { get; set; }
        void REFRESH_ACTION();
        IEnumerable<IDailyData> DAILY_DATA { get; set; }
        HolidayCollection HOLIDAYS { get; set; }
        void HolidayAdding(IHoldiay item);
        void HolidayRemoving(IHoldiay item);
        bool HAS_HOLIDAY { get; set; }
        string HOLIDAY_TOOLTIP { get; set; }
        //int CUSTOMER_ID { get; set; }
        //string SKU_ID { get; set; }
        //Guid DOCUMENTOBJECTID { get; set; }
        string SHIP_TO_TP { get; set; }
        int QP { get; set; }
        int QA { get; set; }
        ICellComment QA_COMMENT { get; set; }
        int QS { get; set; }

        int WTD_QS { get; set; }
        ICellComment QS_COMMENT { get; set; }
        int QO { get; set; }

        int QONumberOfNoneZeros { get; set; }
        ICellComment QO_COMMENT { get; set; }
        
        int QW { get; set; }
        ICellComment QW_COMMENT { get; set; }
        int LYQS { get; set; }
        ICellComment LY_COMMENT { get; set; }
        int LYQW { get; set; }
        string UOM { get; set; }
        DateTime REPORT_AS_OF_DATE { get; set; }
        IEnumerable<DateTime> ALL_REPORT_AS_OF_DATE { get; set; }
        IEnumerable<DateTime> ALL_DAYS_IN_WEEK { get; set; }
        DateTime FIRST_DAY_OF_WEEK { get; set; }
        string DAY_OF_WEEK { get; }
        int WEEK_NUMBER { get; set; }
        string WEEK_ID { get; }        
        
        DateTime LAST_YEAR_DATE { get; }
        //string COMPANY_NAME { get; set; }
        string ITEM_UPC { get; set; }
        int YEAR { get; set; }

        //int LAST_FORECAST { get; set; }
        ForecastCollection FORECASTS { get; set; }
        //string FORECAST_TOOLTIP { get; set; }
        //int LAST_YEAR_QS { get; }

        //DateTime START_DATE { get; set; }
        //DateTime END_DATE { get; set; }
        //decimal PRICE { get; set; }
        //bool IS_AD_OR_TPR { get; set; }

        //bool IS_CONFIRMED { get; set; }
        IFsi FSI { get; set; }
        IQcQd QC { get; set; }
        IQcQd QD { get; set; }
        IComment COMMENT { get; set; }

        BaseLine BASE_LINE { get; set; }
        DaysOfSupplies DAYS_OF_SUPPLIES
        {
            get;set;
        }
        BaseIndex BASE_INDEX { get; set; }
        decimal ORDER_DELIVERY { get; set; }
        ICellComment ORDER_DELIVERY_COMMENT { get; set; }
        PerformanceCollection PERFORMANCES { get; set; }
        bool HAS_PERFORMANCES { get; }

        bool IS_FUTURE { get; set; }
        /// <summary>
        /// UPDATES THE HAS_PERFORMANCES PROPERTY
        /// </summary>
        //void UPDATE_PERFORMANCES();

        void AddPerformance(IPerformance item);
        void DeletePerformance(IPerformance item);
        void OnPropertyChanged(string name);
    }
}
