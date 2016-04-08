using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Pos : IPos, INotifyPropertyChanged
    {

        #region IPos Members

        private DateTime _FIRST_DAY_OF_WEEK;
        private DateTime _DATE;
        private decimal _CASES;
        private string _DIVISION_NUMBER;
        private string _DIVISION_NAME;
        public string DIVISION_NAME
        {
            get
            {
                return _DIVISION_NAME;
            }
            set
            {
                _DIVISION_NAME = value;
                NotifyPropertyChanged();
            }
        }

        //public string DIVISION_NUMBER
        //{
        //    get
        //    {
        //        return _DIVISION_NUMBER;
        //    }
        //    set
        //    {
        //        _DIVISION_NUMBER = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        public decimal CASES
        {
            get
            {
                return Math.Round(_CASES, 2);
            }
            set
            {
                _CASES = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime DATE
        {
            get
            {
                return _DATE;
            }
            set
            {
                _DATE = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime FIRST_DAY_OF_WEEK
        {
            get
            {
                return _FIRST_DAY_OF_WEEK;
            }
            set
            {
                _FIRST_DAY_OF_WEEK = value;
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

    
    
    }
}
