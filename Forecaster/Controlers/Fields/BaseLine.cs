using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Controlers.Fields
{
    public class BaseLine : IField<int>, INotifyPropertyChanged
    {
        public BaseLine(IDailyData data)
        {
            DATA = data;            
            _WEEK_ID = data.WEEK_ID;
            COMMENT = new Comment(_WEEK_ID);

        }
        #region IField Members

        
        private string _WEEK_ID;
        private Comment _COMMENT;
        private int _VALUE;

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
                if (DATA.BASE_INDEX != null)
                    DATA.BASE_INDEX.NotifyPropertyChanged(null);
            }
        }

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
                value.NotifyPropertyChanged(null);
            }
        }

        public string TOOLTIP
        {
            get
            {
                return COMMENT.VALUE;
            }
            
        }

        #endregion
        #region properties
        public IDailyData DATA { get; set; }
        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

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
