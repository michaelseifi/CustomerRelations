using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Extensions;
using datastores = CRDataStore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace daisybrand.forecaster.Controlers.Objects
{
    public class QcQd : IQcQd, INotifyPropertyChanged
    {
        public QcQd(string weekId)
        {
            this.ID = -1;
            DATE_ENTERED = DateTime.Now;
            WEEKID = weekId;
        }

        public QcQd(string weekId, string comment)
        {
            this.ID = -1;
            DATE_ENTERED = DateTime.Now;
            WEEKID = weekId;
            COMMENT = comment;
        }
        public QcQd()
        {
            
        }

        public static List<IQcQd> GetCollection(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            return _GetQCQD(listOfSkusShipTo, shipTo);
        }

        public static void Delete(IDailyData data, string field)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var shipTo = int.Parse(data.SHIP_TO_TP);
                        var items = context.FORECASTER_QCQD_TABLE.Where(i => i.WEEKID == data.WEEK_ID && i.FIELD == field && i.ITEM_UPC == data.ITEM_UPC && i.SHIP_TO_TP == shipTo);
                        foreach (var i in items)
                            context.FORECASTER_QCQD_TABLE.Remove(i);
                        if (field == "QC")
                            data.QC.COMMENT = string.Empty;
                        else if (field == "QD")
                            data.QD.COMMENT = string.Empty;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new DeleteException("Unable to delete QC/QD field.");
                }
            }
        }

        public static int InsertOrUpdateComment(IDailyData data, string comment, string field)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var items = context.FORECASTER_QCQD_TABLE.Where(i => i.WEEKID == data.WEEK_ID && i.FIELD == field);
                        if (items.Count() > 0)
                            foreach (var i in items)
                            {
                                i.COMMENT = comment;
                                i.ENTERED_BY = Environment.UserName;
                            }
                        else
                        {
                            var item = new datastores.FORECASTER_QCQD_TABLE
                            {
                                COMMENT = comment,
                                WEEKID = data.WEEK_ID,
                                FIELD = field,
                                SHIP_TO_TP = int.Parse(data.SHIP_TO_TP),
                                ITEM_UPC = data.ITEM_UPC,
                                VALUE = field == "QC" ? data.QC.GetValue() : data.QD.GetValue(),
                                FORMULA = field == "QC" ? data.QC.FORMULA : data.QD.FORMULA,
                                ENTERED_BY = Environment.UserName,
                                DATE_ENTERED = DateTime.Now
                            };
                            context.FORECASTER_QCQD_TABLE.Add(item);
                        }
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                }
                return 1;
            }
            else
                return 1;
        }

        public static int InsertOrUpdateValue(IDailyData data, IQcQd iQcQd, string field)
        {
            if (Tools.IsInEditMode())
            {
                
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var items = context.FORECASTER_QCQD_TABLE.Where(i => i.WEEKID == data.WEEK_ID && i.FIELD == field);
                       if(items.Count()>0)
                           foreach(var i in items)
                           {
                               i.ENTERED_BY = Environment.UserName;
                               i.VALUE = iQcQd.GetValue();
                               i.DATE_ENTERED = DateTime.Now;
                               i.FORMULA = iQcQd.FORMULA;
                           }
                       else
                       {
                           var item = new datastores.FORECASTER_QCQD_TABLE
                           {
                               WEEKID = data.WEEK_ID,
                               FIELD = field,
                               ITEM_UPC = data.ITEM_UPC,
                               SHIP_TO_TP = int.Parse(data.SHIP_TO_TP),
                               VALUE = iQcQd.GetValue(),
                               FORMULA = iQcQd.FORMULA,
                               COMMENT = field == "QC" ? data.QC.COMMENT : data.QD.COMMENT,
                               ENTERED_BY = Environment.UserName,
                               DATE_ENTERED = DateTime.Now
                           };
                           context.FORECASTER_QCQD_TABLE.Add(item);
                       }
                       context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new InsertException("Unable to update or insert QC/QD value.");
                }
            }
            else
                return 1;
           
        }       
        
        public static IQcQd AddEmpty(List<IQcQd> list, string weekId)
        {
            list.Add(new QcQd(weekId));
            return list.Where(x => x.WEEKID == weekId).FirstOrDefault();
        }

        private static List<IQcQd> _GetQCQD(IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            List<IQcQd> list = new List<IQcQd>();
            List<datastores.FORECASTER_QCQD_TABLE> query;
            try
            {
                using (var context = new datastores.ForecasterEntities(null))                   
                {
                    query = context.FORECASTER_QCQD_TABLE.Where(c => c.SHIP_TO_TP == shipTo).ToList();                        
                }
                if (query != null)
                {
                    var query2 = query.Where(c => listOfSkusShipTo.Contains(string.Format("{0}{1}", c.ITEM_UPC, c.SHIP_TO_TP)));
                    if (query2.Count() > 0)
                    {
                        list.AddRange(query2.Select(x => new QcQd(x.WEEKID)
                        {
                            ID = x.QCID,
                            DATE_ENTERED = x.DATE_ENTERED,
                            VALUE = x.VALUE.ToString(),
                            FIELD = x.FIELD,
                            FORMULA = x.FORMULA ?? string.Empty,
                            COMMENT = x.COMMENT ?? string.Empty,
                            ITEM_UPC = x.ITEM_UPC,
                            SHIP_TO_TP = x.SHIP_TO_TP.ToString(),
                            ENTERED_BY = x.ENTERED_BY
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return list;
        }  

        #region properties
        public int ID { get; set; }
        public string WEEKID { get; set; }
        private string _FORMULA;
        private string _ITEM_UPC;
        private string _ENTERED_BY;
        private string _SHIP_TO_TP;
        private string _COMMENT;
        private int _VALUE;

        public string VALUE
        {
            get
            {
                return _VALUE.ToString();
            }
            set
            {
                SetValue(value);
            }
        }
        public int GetValue()
        {
            return _VALUE;
        }

        public string FORMULA
        {
            get
            {
                return _FORMULA;
            }
            set
            {
                _FORMULA = value;
                NotifyPropertyChanged();
            }
        }

        public void SetValue(string value)
        {
            try
            {
                this.FORMULA = value;
                var dcml = Math.Round(Convert.ToDecimal(value.FilterWeekId().OperatorParse()));
                var i = int.Parse(dcml.ToString());
                _VALUE = i;
                NotifyPropertyChanged("VALUE");
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Value is " + value ?? "Null");
                LogManger.RaiseErrorMessage(new Message { MESSAGE = ex.Message });
            }
        }

        public DateTime DATE_ENTERED { get; set; }
        public string FIELD { get; set; }
        public string COMMENT
        {
            get
            {
                return _COMMENT;
            }
            set
            {
                _COMMENT = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
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




        #region IQcQd Members


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

        #endregion

        #region IQcQd Members


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

        #endregion
    }
}
