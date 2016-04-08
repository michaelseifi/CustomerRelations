using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Helpers;
using System.Collections;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Extensions;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Forecast : IForecast, INotifyPropertyChanged
    {

        private int _BASELINE;
        private string _FORMULA;
        private int _VALUE;
        private string _ENTERED_BY;
        private DateTime _DATE_ENTERED;
        private int _FORECASTID;

        #region IForecast Members

        public int FORECASTID
        {
            get
            {
                return _FORECASTID;
            }
            set
            {
                _FORECASTID = value;
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
            }
        }
        public int VALUE
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

        public int QS { get; set; }
        public int QW { get; set; }
        public int QO { get; set; }
        public int QA { get; set; }

        public int QC { get; set; }

        public int QD { get; set; }
        public string WEEKID { get; set; }

        public bool ACTIVE { get; set; }

        public int BASELINE
        {
            get
            {
                return _BASELINE;
            }
            set
            {
                _BASELINE = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        public string L4WKAVGTRN { get; set; }

        public string L13WKAVGTRN { get; set; }
        public Forecast(string weekId)
        {
            WEEKID = weekId;
            ENTERED_BY = Environment.UserName;
            DATE_ENTERED = DateTime.Now;
        }

        public Forecast(string weekId, int value)
        {
            WEEKID = weekId;
            ENTERED_BY = Environment.UserName;
            DATE_ENTERED = DateTime.Now;
            VALUE = value;
        }

        public static void Delete(ITab tab)
        {
            if (Tools.IsInEditMode())
            {
                try
                {
                    var data = tab.myWeeklyData.FocusedData;
                    var item = new datastores.FORECASTER_FORECAST_TABLE
                    {
                        ACTIVE = false,
                        WEEKID = data.WEEK_ID,
                        ITEM_UPC = data.ITEM_UPC,
                        SHIP_TO_TP = int.Parse(data.SHIP_TO_TP),
                        VALUE = 0,
                        FORMULA = string.Empty,
                        L13WKAVGTRN = tab.L13WKAVGTRN,
                        L4WKAVGTRN = tab.L4WKAVGTRN,
                        QA = data.QA,
                        QS = data.QS,
                        QO = data.QO,
                        QW = data.QW,
                        ENTERED_BY = Environment.UserName,
                        DATE_ENTERED = DateTime.Now,
                        QC = data.QC.GetValue(),
                        QD = data.QD.GetValue(),
                        BASELINE = data.BASE_LINE.VALUE
                    };

                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var shipTo = int.Parse(data.SHIP_TO_TP);
                        context.FORECASTER_FORECAST_TABLE.Where(f => f.WEEKID == data.WEEK_ID && f.ITEM_UPC == data.ITEM_UPC && f.SHIP_TO_TP == shipTo)
                            .Update(f => f.ACTIVE = false);
                        context.FORECASTER_FORECAST_TABLE.Add(item);

                        data.FORECASTS.Update(x => x.ACTIVE = false);
                        data.FORECASTS.AddItem(new Forecast(data.WEEK_ID, 0)
                        {
                            QA = data.QA,
                            QO = data.QO,
                            QS = data.QS,
                            QW = data.QW,
                            QC = data.QC.GetValue(),
                            QD = data.QD.GetValue(),
                            L13WKAVGTRN = tab.L13WKAVGTRN,
                            L4WKAVGTRN = tab.L4WKAVGTRN,
                            ENTERED_BY = Environment.UserName,
                            DATE_ENTERED = DateTime.Now,
                            FORMULA = string.Empty,
                            ACTIVE = true,
                            BASELINE = data.BASE_LINE.VALUE
                        });

                        context.SaveChanges();
                    }

                    
                    //data.FORECASTS.NotifyPropertyChanged(null);
                    //data.OnPropertyChanged("FORECASTS");
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    throw new DeleteException("Unable to delete forecast field.");
                }
            }
            else
            {
                var data = tab.myWeeklyData.FocusedData;
                data.FORECASTS.AddItem(new Forecast(data.WEEK_ID, 0)
                {
                    FORMULA = string.Empty,
                    QA = data.QA,
                    QO = data.QO,
                    QS = data.QS,
                    QW = data.QW,
                    QC = data.QC.GetValue(),
                    QD = data.QD.GetValue(),
                    L13WKAVGTRN = tab.L13WKAVGTRN,
                    L4WKAVGTRN = tab.L4WKAVGTRN,
                    ENTERED_BY = Environment.UserName,
                    DATE_ENTERED = DateTime.Now,
                    ACTIVE = true,
                    BASELINE = data.BASE_LINE.VALUE
                });
            }
        }


        public static void Insert(IDailyData data, string L13WKAVGTRN, string L4WKAVGTRN)
        {            
            if (Tools.IsInEditMode())
            {
                try
                {
                    var item = new datastores.FORECASTER_FORECAST_TABLE
                    {
                        ACTIVE = true,
                        WEEKID = data.WEEK_ID,
                        ITEM_UPC = data.ITEM_UPC,
                        SHIP_TO_TP = int.Parse(data.SHIP_TO_TP),
                        VALUE = data.FORECASTS.GetLastValue(),
                        FORMULA = data.FORECASTS.LASTFORMULA,
                        QA = data.QA,
                        QS = data.QS,
                        QO = data.QO,
                        QW = data.QW,
                        L13WKAVGTRN = L13WKAVGTRN,
                        L4WKAVGTRN = L4WKAVGTRN,
                        ENTERED_BY = Environment.UserName,
                        DATE_ENTERED = DateTime.Now,
                        QC = data.QC.GetValue(),
                        QD = data.QD.GetValue(),
                        BASELINE = data.BASE_LINE.VALUE
                        
                    };
                    using (var context = new datastores.ForecasterEntities(null))                       
                    {
                        context.FORECASTER_FORECAST_TABLE.Add(item);
                       
                        data.FORECASTS.AddItem(new Forecast(data.WEEK_ID, data.FORECASTS.GetLastValue())
                        {
                            FORMULA = data.FORECASTS.LASTFORMULA,
                            QA = data.QA,
                            QO = data.QO,
                            QS = data.QS,
                            QW = data.QW,
                            QC = data.QC.GetValue(),
                            QD = data.QD.GetValue(),
                            L13WKAVGTRN = L13WKAVGTRN,
                            L4WKAVGTRN = L4WKAVGTRN,
                            ENTERED_BY = Environment.UserName,
                            DATE_ENTERED = DateTime.Now,
                            ACTIVE = true,
                            BASELINE = data.BASE_LINE.VALUE
                        });

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new ItemNotInsertedException(ex);
                }
            }
            else
            {
                data.FORECASTS.AddItem(new Forecast(data.WEEK_ID, data.FORECASTS.GetLastValue())
                {
                    FORMULA = data.FORECASTS.LASTFORMULA,
                    QA = data.QA,
                    QO = data.QO,
                    QS = data.QS,
                    QW = data.QW,
                    QC = data.QC.GetValue(),
                    QD = data.QD.GetValue(),
                    L13WKAVGTRN = L13WKAVGTRN,
                    L4WKAVGTRN = L4WKAVGTRN,
                    ENTERED_BY = Environment.UserName,
                    DATE_ENTERED = DateTime.Now,
                    ACTIVE = true,
                    BASELINE = data.BASE_LINE.VALUE
                });
            }
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
