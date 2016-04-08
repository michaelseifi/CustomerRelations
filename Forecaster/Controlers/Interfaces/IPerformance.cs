using System;
using System.Collections.Generic;
using daisybrand.forecaster.Controlers.Enums;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IPerformance
    {
        string BANNERS_TOOLTIP { get; set; }
        string FIRST_BANNER { get; set; }
        string CONFIRMED_BY { get; set; }
        DateTime CONFIRMED_DATE { get; set; }
        string CONFIRMED_DATE_STR { get; set; }
        string CUSTOMER_ID { get; set; }
        Guid DOCUMENTOBJECTID { get; set; }
        DateTime END_DATE { get; set; }
        bool IS_CONFIRMED { get; set; }
        string PERFORMANCE_TYPE { get; set; }
        int PERFORMANCE_TYPEID { get; set; }
        double PRICE { get; set; }
        string PROMOTION_NUMBER { get; set; }
        double QUANTITY { get; set; }
        string SKU_ID { get; set; }
        DateTime START_DATE { get; set; }
        string WEEKID { get; set; }
        DateTime REPORT_AS_OF_DATE { get; set; }
        string AD_NAME { get; set; }
        string TP_NAME { get; set; }
        string CR_ASSOCIATE { get; set; }
        string REGIONAL_MANAGER { get; set; }
        IEnumerable<string> BANNERS { get; set; }
        bool IS_INCLUDED { get; set; }
        bool IS_DEFAULT { get; set; }
        string STATUS { get; set; }

        DState D_STATE { get; set; }
        MState M_STATE { get; set; }

        string STATE_USER { get; set; }
        DateTime STATE_LAST_UPDATED { get; set; }
    }
}