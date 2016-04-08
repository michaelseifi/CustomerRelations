using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
namespace daisybrand.forecaster.Controlers.Objects
{
  
    public class Settings
    {
        public Settings()
        {
            CALCULATOR = new Calculator();
            PERCENTAGE = new Percentage();
            VOLATILITY = new Volatility();
            GRAPH = new Graph();
            CUSTOMERS = new List<Customer>();
        }
        #region properties
        public string VERSION { get; set; }
        public string EMPLID { get; set; }
        public double WIDTH { get; set; }
        public double HEIGHT { get; set; }
        public double TOP { get; set; }
        public double LEFT { get; set; }
        
        public Calculator CALCULATOR { get; set; }

        public Percentage PERCENTAGE { get; set; }

        public Volatility VOLATILITY { get; set; }
        public Graph GRAPH { get; set; }

        public List<Customer> CUSTOMERS { get; set; }
        #endregion


        public Customer GetCustomer(string customerId)
        {
            IEnumerable<Customer> custs;
            if (!this.CUSTOMERS.Any(c => c.CUSTOMERID == customerId))
                this.CUSTOMERS.Add(new Customer { CUSTOMERID = customerId });                
            
            custs = this.CUSTOMERS.Where(c => c.CUSTOMERID == customerId);
            return custs.First();            
        }
        //public object GetPropertyValue(string propertyName)
        //{
        //    return this.GetType().GetProperty(propertyName).GetValue(this, null);
        //}

        //public void SaveProperty<T>(string propertyName, object value)
        //{
        //    var property = this.GetType().GetProperty(propertyName);
        //    var val = (T)property.GetValue(this, null);

        //}

        

        public class Calculator
        {
            #region properties                 
            public double TOP { get; set; }
            public double LEFT { get; set; }
            #endregion
        }

        public class Percentage
        {
            #region properties
            public double TOP { get; set; }
            public double LEFT { get; set; }

            #endregion
        }

        public class Volatility
        {
            #region properties
            public double TOP { get; set; }
            public double LEFT { get; set; }

            #endregion
        }

        public class Graph
        {
            #region properties
            public double WIDTH { get; set; }
            public double HEIGHT { get; set; }
            public double TOP { get; set; }
            public double LEFT { get; set; }
            #endregion
        }

        public class Customer
        {
            public Customer()
            {
                TABS = new List<Tab>();
            }
            #region properties
            public string CUSTOMERID { get; set; }
            public List<Tab> TABS { get; set; }
            #endregion

            public Tab GetTab(string skuid)
            {
                if (!this.TABS.Any(t => t.SkuId == skuid))
                    this.TABS.Add(new Tab() { SkuId = skuid, IsDailyDataExpanded = true });
                return this.TABS.Where(t => t.SkuId == skuid).First();
            }
            public class Tab
            {
 
                #region properties
                public string SkuId { get; set; }
                public bool IsDailyDataExpanded { get; set; }
                #endregion
            }
        }

  
    }
}
