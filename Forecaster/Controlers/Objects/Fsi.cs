using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Exceptions;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Fsi : IFsi, INotifyPropertyChanged
    {
        public Fsi(string weekId)
        {
            WEEKID = weekId;
            ENTERED_BY = Environment.UserName;
            DATE_ENTERED = DateTime.Now;
            VALUE = string.Empty;
            ITEM_UPC = string.Empty;
            SHIP_TO_TP = "0";
        }

        #region IFsi Members
        private DateTime _DATE_ENTERED;
        private string _ENTERED_BY;
        private int _FSIID;
        private string _WEEKID;
        private string _VALUE;
        private string _SHIP_TO_TP;
        private string _ITEM_UPC;
        public int FSIID
        {
            get
            {
                return _FSIID;
            }
            set
            {
                _FSIID = value;
                NotifyPropertyChanged();
            }
        }
        public string WEEKID
        {
            get
            {
                return _WEEKID;
            }
            set
            {
                _WEEKID = value;
                NotifyPropertyChanged();
            }
        }
        public string VALUE
        {
            get
            {
                return _VALUE;
            }
            set
            {
                _VALUE = value;
                NotifyPropertyChanged();
            }
        }
        public string ENTERED_BY
        {
            get
            {
                return _ENTERED_BY;
            }
            set
            {
                _ENTERED_BY = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime DATE_ENTERED
        {
            get
            {
                return _DATE_ENTERED;
            }
            set
            {
                _DATE_ENTERED = value;
                NotifyPropertyChanged();
            }
        }

        public string ITEM_UPC
        {
            get
            {
                return _ITEM_UPC;
            }
            set
            {
                _ITEM_UPC = value;
                NotifyPropertyChanged();
            }
        }

        public string SHIP_TO_TP
        {
            get
            {
                return _SHIP_TO_TP;
            }
            set
            {
                _SHIP_TO_TP = value;
                NotifyPropertyChanged();
            }
        }



        #endregion

        
        public static IFsi AddEmpty(List<IFsi> list, string weekId)
        {
            list.Add(new Fsi(weekId));
            return list.Where(x => x.WEEKID == weekId).FirstOrDefault();
        }

        public static List<IFsi> Get(string customerNumber, string skuId)
        {
            return _GetFsis(customerNumber, skuId);
        }
        public static void Delete(IDailyData data)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                            //new datastores.FORECASTER_FSI_TABLETableAdapters.FORECASTER_FSI_TABLETableAdapter())
                    {
                        var shipTo = int.Parse(data.SHIP_TO_TP);
                        var items = context.FORECASTER_FSI_TABLE.Where(i => i.WEEKID == data.WEEK_ID && i.ITEM_UPC == data.ITEM_UPC && i.SHIP_TO_TP == shipTo);
                        foreach (var i in items)
                            context.FORECASTER_FSI_TABLE.Remove(i);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new DeleteException("Unable to delete event field.");
                }
            }            
        }
        public static void UpdateOrInsert(IDailyData data)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var items = context.FORECASTER_FSI_TABLE.Where(i => i.WEEKID == data.WEEK_ID);
                        var shipTo = int.Parse(data.SHIP_TO_TP);
                        if (items != null && items.Count() > 0)
                        {
                            foreach (var i in items)
                            {
                                i.VALUE = data.FSI.VALUE;
                                i.DATE_ENTERED = DateTime.Now;
                                i.ENTERED_BY = Environment.UserName;
                            }
                        }
                        else
                        {
                            var item = new datastores.FORECASTER_FSI_TABLE
                            {
                                DATE_ENTERED = DateTime.Now,
                                ENTERED_BY = Environment.UserName,
                                ITEM_UPC = data.ITEM_UPC,
                                SHIP_TO_TP = shipTo,
                                VALUE = data.FSI.VALUE,
                                WEEKID = data.WEEK_ID
                            };
                            context.FORECASTER_FSI_TABLE.Add(item);
                        }
                        context.SaveChanges();
                    } 
                }
                catch (Exception ex)
                {
                    data.FSI.VALUE = "Error: contact administrator.";
                    LogManger.Insert(ex);
                    throw new InsertException("Unable to update or insert event value.");
                }
            }
        }

        private static List<IFsi> _GetFsis(string customerNumber, string skuId)
        {
            List<IFsi> c = new List<IFsi>();
            List<datastores.FORECASTER_FSI_TABLE> list;
            try
            {
                var shipTo = int.Parse(customerNumber);
                using (var context = new datastores.ForecasterEntities(null))
                    //new datastores.FORECASTER_FSI_TABLETableAdapters.FORECASTER_FSI_TABLETableAdapter())
                {
                    list = context.FORECASTER_FSI_TABLE.Where(i => i.SHIP_TO_TP == shipTo && i.ITEM_UPC == skuId).ToList();
                }
                if (list != null && list.Count() > 0)
                {
                    c.AddRange(list.Select(x => new Fsi(x.WEEKID)
                                                        {
                                                            DATE_ENTERED = x.DATE_ENTERED,
                                                            ENTERED_BY = x.ENTERED_BY,
                                                            VALUE = x.VALUE,
                                                            SHIP_TO_TP = x.SHIP_TO_TP.ToString(),
                                                            ITEM_UPC = x.ITEM_UPC,
                                                            WEEKID = x.WEEKID,
                                                            FSIID = x.FSIID
                                                        }));
                }
                    //var fsi = dt.GetData().Where(x => x.SHIP_TO_TP == int.Parse(customerNumber) && x.ITEM_UPC == skuId).Select(x => new Fsi(x.WEEKID)
                    //{
                    //    DATE_ENTERED = x.DATE_ENTERED,
                    //    ENTERED_BY = x.ENTERED_BY,
                    //    VALUE = x.VALUE,
                    //    SHIP_TO_TP = x.SHIP_TO_TP.ToString(),
                    //    ITEM_UPC = x.ITEM_UPC,
                    //    WEEKID = x.WEEKID,
                    //    FSIID = x.FSIID
                    //});
                    //if (fsi.Count() > 0)
                    //    c.AddRange(fsi);
                
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return c;
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
