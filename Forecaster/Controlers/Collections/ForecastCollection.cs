using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class ForecastCollection: ObservableCollection<IForecast>, INotifyPropertyChanged
    {
        public ForecastCollection()
        {
            
        }
        public ForecastCollection(List<IForecast> list)
            : base(list)
        {
            
        }
        public ForecastCollection(IEnumerable<IForecast> collection)
            : base(collection)
        {
            var activeItems = this.Where(x => x.ACTIVE);
            if (activeItems != null && activeItems.Count() > 0)
            {
                this.LASTENTERED = this.OrderByDescending(x => x.DATE_ENTERED).FirstOrDefault();
                this.LASTVALUE = this.LASTENTERED.VALUE.ToString();
            }
            else
                this.LASTVALUE = "0";
        }

        public static ForecastCollection GetEmpty(IForecast item)
        {
            List<IForecast> list = new List<IForecast>();
            return new ForecastCollection(list)
            {
                WEEKID = item.WEEKID
            };                        
        }

        public static List<ForecastCollection> Get(
            IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            return _GetForecasts(listOfSkusShipTo, shipTo);
        }

        public void AddItem(IForecast item)
        {
            this.Add(item);
            this.TOOLTIP = _GetForecastTooltip();
            var activeItems = 
                this.OrderByDescending(x => x.DATE_ENTERED).Where(x => x.ACTIVE);
            if (activeItems != null && activeItems.Count() > 0)
            {
                this.LASTENTERED = activeItems.First();
                this.LASTVALUE = LASTENTERED.VALUE.ToString();
            }
            else
                this.LASTVALUE = "0";
        }


        private static List<ForecastCollection> _GetForecasts(
            IEnumerable<string> listOfSkusShipTo, int shipTo)
        {
            List<ForecastCollection> list = new List<ForecastCollection>();
            try
            {
                List<datastores.FORECASTER_FORECAST_TABLE> query;
                using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_FORECAST_TABLETableAdapters.FORECASTER_FORECAST_TABLETableAdapter())
                {
                    query = context.FORECASTER_FORECAST_TABLE.Where(c => c.SHIP_TO_TP == shipTo).ToList();
                }
                if (query != null && query.Count() > 0)
                {
                    var query2 = query.Where(c => listOfSkusShipTo.Contains(c.ITEM_UPC + c.SHIP_TO_TP));
                    if (query2.Count() > 0)
                    {
                        var a = query2.Select(x => new Forecast(x.WEEKID)
                           {
                               DATE_ENTERED = x.DATE_ENTERED,
                               ENTERED_BY = x.ENTERED_BY,
                               QA = x.QA ?? 0,
                               QO = x.QO ?? 0,
                               QS = x.QS ?? 0,
                               QW = x.QW ?? 0,
                               QC = x.QC ?? 0,
                               QD = x.QD ?? 0,
                               VALUE = x.VALUE,
                               FORMULA = x.FORMULA ?? string.Empty,
                               ACTIVE = x.ACTIVE ?? false,
                               L13WKAVGTRN = x.L13WKAVGTRN ?? string.Empty,
                               L4WKAVGTRN = x.L4WKAVGTRN ?? string.Empty,
                               BASELINE = x.BASELINE ?? 0 ,
                               
                                                            
                           }).GroupBy(x => x.WEEKID);

                        foreach (IGrouping<string, IForecast> g in a)
                            list.Add(new ForecastCollection(g)
                            {
                                WEEKID = g.Key
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
            }
            return list;
        }

        private string _GetForecastTooltip()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HISTORY:\r");
            if (this.Count > 0)
            {
                IEnumerable<IForecast> nonZerolist = this.Where(f => f.VALUE != 0);
                IEnumerable<IForecast> orderedList = nonZerolist.OrderByDescending(x => x.DATE_ENTERED).Take(20);
                //List<IForecast> list = new List<IForecast>();
                //IForecast lastValue = null;
                //foreach (var l in orderedList)
                //{
                //    if (lastValue == null || l.VALUE != lastValue.VALUE)
                //    {
                //        list.Add(l);
                //        lastValue = l;
                //    }
                //}

                foreach (IForecast f in orderedList)
                {
                    sb.Append(string.Format("\t{0} - {1} [{11}] (QS: {2} - QW: {3} - QO: {4} - QA: {5} - QC: {6} - QD: {7} - L13WKAVGTRN: {8} - L4WKAVGTRN: {9} - BASELINE: {12} {10})\r",
                        f.DATE_ENTERED,
                        f.VALUE,
                        f.QS,
                        f.QW,
                        f.QO,
                        f.QA,
                        f.QC,
                        f.QD,
                        f.L13WKAVGTRN,
                        f.L4WKAVGTRN,
                        f.ENTERED_BY.ToUpper(),
                        f.FORMULA ?? @"n/a",
                        f.BASELINE));                   
                }
            }
            else
                sb.Append("\tN/A\r");
            if (!string.IsNullOrEmpty(this.COMMENT.VALUE))
                sb.Append("COMMENT:\r\t" + this.COMMENT.VALUE);
            return sb.ToString();
        }

        #region properties
        private IForecast _LASTENTERED;
        private ICellComment _COMMENT;
        private string _WEEKID;
        private int _LASTVALUE;
        private string _TOOLTIP;

        public string WEEKID
        {
            get
            {
                return _WEEKID;
            }
            set
            {
                _WEEKID = value;
            }
        }

        public string TOOLTIP
        {
            get
            {
                return _TOOLTIP;
            }
            set
            {
                if (_TOOLTIP == value) return;
                _TOOLTIP = value;
                NotifyPropertyChanged();
            }
        }



        public ICellComment COMMENT
        {
            get
            {
                return _COMMENT;
            }
            set
            {
                if (_COMMENT == value) return;
                _COMMENT = value;
                this.TOOLTIP = _GetForecastTooltip();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// SETS THE COMMENT'S VALUE
        /// RETURNS THE COMMENT'S VALUE
        /// </summary>
        public string SetCommentValue(string comment)
        {
            this.COMMENT.VALUE = comment;
            this.TOOLTIP = _GetForecastTooltip();
            return comment;
        }

        /// <summary>
        /// LAST ENTERED FORECAST VALUE
        /// </summary>
        public IForecast LASTENTERED
        {
            get
            {
                return _LASTENTERED;
            }
            set
            {
                _LASTENTERED = value;
                NotifyPropertyChanged();
            }
        }

        public string LASTFORMULA
        {
            get;
            set;
        }
        public string LASTVALUE
        {
            get
            {
                return _LASTVALUE.ToString();
            }
            set
            {
                LASTFORMULA = value;
                var d = value.FilterWeekId().OperatorParse();
                if (_LASTVALUE == d.ToInt()) return;
                _LASTVALUE = d.ToInt();
                NotifyPropertyChanged();
                //((IQuantity)this).FORECAST_TOOLTIP = _GetForecastTooltip();
            }
        }

        public int GetLastValue()
        {
            return _LASTVALUE;
        }

        public IForecast GetLASTENTERED()
        {
            var activeItems = this.OrderByDescending(x => x.DATE_ENTERED).Where(x => x.ACTIVE);
            if (activeItems != null && activeItems.Count() > 0)
            {
                return activeItems.First();
            }
            return null;
        }
        #endregion
        
        #region INotifyPropertyChanged Members

        private event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }

    
}
