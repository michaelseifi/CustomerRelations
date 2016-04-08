using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface ICustomer
    {
        string EMPLID { get; set; }
        string ACCOUNT_NAME { get; set; }
        string ACCOUNTNUM { get; set; }
        string NAME { get; set; }
        int PARTYID { get; set; }
        string INVOICEACCOUNT { get; set; }
        string CUSTGROUP { get; set; }
        string PAYMTERM_ID { get; set; }
        string PAYMDAY_ID { get; set; }
        string DLVTERM { get; set; }
        string INVENT_LOCATION { get; set; }
        string ADDRESS { get; set; }
        string ZIPCODE { get; set; }
        string CITY { get; set; }
        SkuCollection SKUS { get; set; }
        SkuCollection SKUS_EXCEPTIONS { get; set; }
        string COMPANY_NAME { get; set; }
        string CLASSIFICATION_ID { get; set; }
        string REGIONAL_MANGER { get; set; }
        string CR_ASSOCIATE { get; set; }

        Customer.Setting SETTING { get; set; }

        void SetSETTING(Customer.Setting setting);
        ISku FocusedSku { get; set; }
        
    }
}
