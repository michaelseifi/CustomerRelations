using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class StatusBar_Main: INotifyPropertyChanged
    {
        #region properties
        private Uri _ADD_URL;
        private double _MIN;
        private double _MAX;
        private string _TEXT;
        private int _COUNT;
        private double _AVERAGE;
        private double _SUM;

        public string TEXT
        {
            get
            {
                return _TEXT;
            }
            private set
            {
                if (_TEXT == value) return;
                _TEXT = value;
                NotifyPropertyChanged();
            }
        }

        public double MAX
        {
            get
            {
                return _MAX;
            }
            set
            {
                if (_MAX == value) return;
                _MAX = value;
                NotifyPropertyChanged();
            }
        }
        public double MIN
        {
            get
            {
                return _MIN;
            }
            set
            {
                if (_MIN == value) return;
                _MIN = value;
                NotifyPropertyChanged();
            }
        }

        public double SUM
        {
            get
            {
                return _SUM;
            }
            set
            {
                if (_SUM == value) return;
                _SUM = value;
                NotifyPropertyChanged();
            }
        }
        public double AVERAGE
        {
            get
            {
                return _AVERAGE;
            }
            set
            {
                if (_AVERAGE == value) return;
                _AVERAGE = Math.Round(value, 2);
                NotifyPropertyChanged();
            }
        }
        public int COUNT
        {
            get
            {
                return _COUNT;
            }
            set
            {
                if (_COUNT == value) return;
                _COUNT = value;
                NotifyPropertyChanged();
            }
        }

        public Uri ADD_URL
        {
            get
            {
                return _ADD_URL;
                //if (MainWindow.GetTABS() != null)
                //    return MainWindow.GetTABS().FOCUSEDCUSTOMER.SETTING.ADD_URL_URI;
                //else if (daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null
                //    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING != null)
                //    return daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.ADD_URL_URI;
                //return null;                  
            }
            set
            {
                _ADD_URL = value;
                NotifyPropertyChanged();
            }
        }

        //public void RefreshADD_URL()
        //{
        //    NotifyPropertyChanged("ADD_URL");
        //}
        #endregion


        public void SetTextProperty(int max, int min, int sum, double average, int count, string subject)
        {
            this.SUM = sum;
            this.AVERAGE = average;
            this.COUNT = count;
            this.MIN = min;
            this.MAX = max;
            this.TEXT = String.Format("{3} (Sum: {0}  |  Average: {1}  |  Count: {2}  |  Min: {4}  |  Max: {5})", SUM.ToString("#,###"), Math.Round(AVERAGE, 0).ToString("#,###"), COUNT.ToString("#,###"), subject, MIN.ToString("#,###"), MAX.ToString("#,###"));
        }

        public void SetTextProperty(decimal max, decimal min, decimal sum, decimal average, int count, string subject)
        {
            this.SUM = Convert.ToDouble(sum);
            this.AVERAGE = Convert.ToDouble(average);
            this.COUNT = count;
            this.MIN = Convert.ToDouble(min);
            this.MAX = Convert.ToDouble(max);
            this.TEXT = String.Format("{3} (Sum: {0}  |  Average: {1}  |  Count: {2}  |  Min: {4}  |  Max: {5})", SUM.ToString("#,###.##"), AVERAGE.ToString("#,###.##"), COUNT.ToString("#,###"), subject, MIN.ToString("#,###.##"), MAX.ToString("#,###.##"));
        }

        public void SetTextProperty(string text)
        {
            this.TEXT = text;
        }
        public void ClearTextProperty()
        {
            this.TEXT = string.Empty;
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
