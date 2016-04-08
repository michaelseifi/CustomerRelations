using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;

namespace daisybrand.forecaster.Controlers.Fields
{
    public class DaysOfSupplies : IField<decimal>, INotifyPropertyChanged
    {
        public DaysOfSupplies (IDailyData data)
        {
            DATA = data;
            _WEEK_ID = data.WEEK_ID;
            COMMENT = new Comment(_WEEK_ID);

        }
        #region IField implementation
        private string _WEEK_ID;
        private decimal _VALUE;
        private Comment _COMMENT;
        public Comment COMMENT
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

        public string TOOLTIP
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public decimal VALUE
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

        public string WEEK_ID
        {
            get
            {
                return _WEEK_ID;
            }
            set
            {
                _WEEK_ID = value;
            }
        }
        #endregion

        #region properties
        public IDailyData DATA
        {
            get; set;
        }
        #endregion


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged ([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        #endregion

    }
}
