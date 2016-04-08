using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using Parent = daisybrand.forecaster;
using daisybrand.forecaster.Controlers.Interfaces;
using System.Runtime.InteropServices;

namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class Percentage:INotifyPropertyChanged
    {

        ITab TAB { get; set; }
        public Percentage(int numberOfWeeks, ITab tab)
        {
            TAB = tab;
            NUMBER_OF_WEEKS = numberOfWeeks;           
        }
        #region properties
        private int _NUMBER_OF_WEEKS;
        private int _NUMBER_OF_DAYS;
        private double _SATURDAY;
        private double _FRIDAY;
        private double _THURSDAY;
        private double _WEDNESDAY;
        private double _TUESDAY;
        private double _MONDAY;
        private double _SUNDAY;

        public int NUMBER_OF_DAYS
        {
            get
            {
                return _NUMBER_OF_DAYS;
            }
            set
            {
                _NUMBER_OF_DAYS = value;                
                NotifyPropertyChanged();
            }
        }

        public int NUMBER_OF_WEEKS
        {
            get
            {
                return _NUMBER_OF_WEEKS;
            }
            set
            {
                _NUMBER_OF_WEEKS = value;
                NUMBER_OF_DAYS = value * 7;
                Update();
                NotifyPropertyChanged();
            }
        }
        
        public double SUNDAY
        {
            get
            {
                return _SUNDAY;
            }
            set
            {
                _SUNDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double MONDAY
        {
            get
            {
                return _MONDAY;
            }
            set
            {
                _MONDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double TUESDAY
        {
            get
            {
                return _TUESDAY;
            }
            set
            {
                _TUESDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double WEDNESDAY
        {
            get
            {
                return _WEDNESDAY;
            }
            set
            {
                _WEDNESDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double THURSDAY
        {
            get
            {
                return _THURSDAY;
            }
            set
            {
                _THURSDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double FRIDAY
        {
            get
            {
                return _FRIDAY;
            }
            set
            {
                _FRIDAY = value;
                NotifyPropertyChanged();
            }
        }
        public double SATURDAY
        {
            get
            {
                return _SATURDAY;
            }
            set
            {
                _SATURDAY = value;
                NotifyPropertyChanged();
            }
        }

        public void SetValue(System.DayOfWeek dayOfWeek, double value)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.CanWrite && string.Compare(prop.Name, dayOfWeek.ToString(), true) == 0)
                {
                    prop.SetValue(this, value, null);
                }
            }
        }

        #endregion



        public void Update()
        {
            Array days = Enum.GetValues(typeof(System.DayOfWeek));
            foreach (DayOfWeek day in days)
            {
                var value = TAB.myWeeklyData.GetQSPercentage(day, this.NUMBER_OF_DAYS);
                this.SetValue(day, value);
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
