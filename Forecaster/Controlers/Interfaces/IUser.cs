using System;
using daisybrand.forecaster.Controlers.Collections;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IUser
    {
        CustomerCollection CUSTOMERS { get; set; }
        CustomerCollection CUSTOMER_EXCEPTIONS { get; set; }
        SkuCollection SKUS { get; set; }
        
        string EMPLID { get; set; }
        string STATUS { get; set; }
        string SELECTED_VALUE { get; set; }
        string DISPLAY_VALUE { get; set; }
    }
}
