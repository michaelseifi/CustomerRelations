using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Tab : Controlers.Interfaces.ITab, INotifyPropertyChanged, IDisposable
    {
        private string _ERROR_COMMENT;
        private bool _HAS_ERROR;
        private ISku _SKU;
       private bool _IsLoaded;
        private PerformanceExceptionsCollection _MyPerformanceExceptions;
        private DataCollection _MyFirst120Data;
        private DataCollection _MyWeeklyData;
        private DataCollection _MyDailyData;
        private object _CONTENT;
        private object _CAPTION;

        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
            set
            {
                _IsLoaded = value;
                NotifyPropertyChanged();
                MainWindow.myMainWindowViewModel.TABS.CheckLoading();
            }
        }
        public object CAPTION
        {
            get
            {
                return _CAPTION;
            }
            set
            {
                _CAPTION = value;
                NotifyPropertyChanged();
            }
        }

        public bool HAS_ERROR
        {
            get
            {
                return _HAS_ERROR;
            }
            set
            {
                _HAS_ERROR = value;
                NotifyPropertyChanged();
            }
        }

        public string ERROR_COMMENT
        {
            get
            {
                return _ERROR_COMMENT;
            }
            set
            {
                _ERROR_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        public ISku SKU
        {
            get
            {
                return _SKU;
            }
            set
            {
                _SKU = value;
                NotifyPropertyChanged();
            }
        }

        public string CUSTOMER_NUMBER { get; set; }
        public object CONTENT
        {
            get
            {
                return _CONTENT;
            }
            set
            {
                _CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        public string L4WKAVGTRN
        {
            get
            {
                if (MainWindow.myTopMenuViewModel != null)
                    return MainWindow.myTopMenuViewModel.AVERAGE_TURN_L4WK;
                return string.Empty;
            }
            
        }

        public string L13WKAVGTRN
        {
            get
            {
                if (MainWindow.myTopMenuViewModel != null)
                    return MainWindow.myTopMenuViewModel.AVERAGE_TURN;
                return string.Empty;
            }
        }

        public DataCollection myDailyData
        {
            get
            {
                return _MyDailyData;
            }
            set
            {
                _MyDailyData = value;
                NotifyPropertyChanged();
            }
        }
        public DataCollection myWeeklyData
        {
            get
            {
                return _MyWeeklyData;
            }
            set
            {
                _MyWeeklyData = value;
                NotifyPropertyChanged();
            }
        }

        public void GetForecasts ()
        {
            var forecasts =
                myWeeklyData
                    .OrderByDescending(data => data.REPORT_AS_OF_DATE)
                    .Select(data => data.FORECASTS);
            
        }

        public DataCollection myFirst120Data
        {
            get
            {
                return _MyFirst120Data;
            }
            set
            {
                _MyFirst120Data = value;
                NotifyPropertyChanged();
            }
        }
        public PerformanceExceptionsCollection myPerformanceExceptions
        {
            get
            {
                return _MyPerformanceExceptions;
            }
            set
            {
                _MyPerformanceExceptions = value;
                NotifyPropertyChanged();
            }
        }
        public ViewModels.Graph myGraphViewModel { get; set; }
        public ViewModels.Percentage myPercentageViewModel { get; set; }

        public Objects.Settings.Customer.Tab Setting { get; set; }
        public void SetError(string tooltip)
        {
            this.HAS_ERROR = true;
            this.ERROR_COMMENT = tooltip;
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

        #region IDisposable Members
        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                if (this.myDailyData != null)
                {
                    this.myDailyData.Dispose();
                    this.myDailyData = null;
                }
                if (this.myWeeklyData != null)
                {
                    this.myWeeklyData.Dispose();
                    this.myWeeklyData = null;
                }

                if(myFirst120Data != null)
                {
                    myFirst120Data.Dispose();
                    myFirst120Data = null;
                }

                if (myPerformanceExceptions != null)
                {
                    myPerformanceExceptions.Dispose();
                    myPerformanceExceptions = null;
                }

                if(this.myGraphViewModel != null)
                {
                    this.myGraphViewModel = null;
                }

                if (this.myPerformanceExceptions != null)
                {
                    this.myPerformanceExceptions.Dispose();
                    this.myPerformanceExceptions = null;
                }
            }

            // Free any unmanaged objects here. 
            //
            //get rid of each dailydata holiday collection 


            disposed = true;
        }
        #endregion
    }
}
