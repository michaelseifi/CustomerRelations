using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Helpers;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class SettingsCollection : ObservableCollection<Settings>
    {
        public SettingsCollection()
        {
            
        }
        public SettingsCollection(List<Settings> list)
            : base(list)
        {
            
        }
        public SettingsCollection(IEnumerable<Settings> collection)
            : base(collection)
        {
            
        }

        public static SettingsCollection Get()
        {
            var path = Caching.GetFilePath(Caching.Folder.xml, "settings");
            if (!File.Exists(path))
            {
                var setColl = new SettingsCollection(new List<Settings>() { new Settings() { EMPLID = Environment.UserName } });
                setColl.Save();
                return setColl;
            }
            else
            {
                var setColl = Caching.GetFromXML<SettingsCollection>(Caching.Folder.xml, "settings");
                
                return setColl;
            } 
        }

        public bool Save()
        {
            Caching.SaveToXML<SettingsCollection>(this, Caching.Folder.xml, "settings");
            return true;
        }

        public bool IsDailyDataExpanded(string customerId, string skuId)
        {
            var mySettings = App.appSetiings.Where(x => x.EMPLID.ToUpper() == Environment.UserName.ToUpper()).FirstOrDefault();
            var customers = mySettings.CUSTOMERS.Where(c => c.CUSTOMERID == customerId);
            if (customers != null && customers.Count() > 0)
            {
                var tabs = customers.First().TABS.Where(t => t.SkuId == skuId);
                if (tabs != null && tabs.Count() > 0)
                {
                    return tabs.First().IsDailyDataExpanded;
                }
            }
            return true;
        }
    }
}
