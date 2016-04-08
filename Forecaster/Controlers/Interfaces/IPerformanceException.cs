using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IPerformanceException
    {
         System.Guid GUID { get; set; }
         int CUSTOMER_ID { get; set; }
         string TYPE { get; set; }
         System.DateTime FROM { get; set; }
         System.DateTime TO { get; set; }
         int QUANTITY { get; set; }
         double PRICE { get; set; }
         string DESCRIPTION { get; set; }
         List<string> TypeArray { get; }

    }
}
