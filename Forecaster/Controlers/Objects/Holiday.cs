using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Holiday: Controlers.Interfaces.IHoldiay
    {

        #region IHoldiay Members

        private string _DESCRIPTION;
        private DateTime _END;
        private DateTime _START;
        private Guid _HOLIDAYID;
        public Guid HOLIDAYID
        {
            get
            {
                return _HOLIDAYID;
            }
            set
            {
                _HOLIDAYID = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime START
        {
            get
            {
                return _START;
            }
            set
            {
                _START = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime END
        {
            get
            {
                return _END;
            }
            set
            {
                _END = value;
                NotifyPropertyChanged();
            }
        }

        public string DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }
            set
            {
                _DESCRIPTION = value.ToUpper();
                NotifyPropertyChanged();
            }
        }

        #endregion

        public void Insert()
        {
            if (!Tools.IsInEditMode()) return;
            using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_HOLIDAYSTableAdapters.FORECASTER_HOLIDAYSTableAdapter())
            {
                var item = new datastores.FORECASTER_HOLIDAYS
                {
                    DESCRIPTION = this.DESCRIPTION,
                    END = this.END,
                    START = this.START,
                    HOLIDAYID = this.HOLIDAYID,
                };
                context.FORECASTER_HOLIDAYS.Add(item);
                context.SaveChanges();
                //dt.Insert(this.HOLIDAYID, this.START, this.END, this.DESCRIPTION);                
            }
        }

        public void Delete()
        {
            if (!Tools.IsInEditMode()) return;
            using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_HOLIDAYSTableAdapters.FORECASTER_HOLIDAYSTableAdapter())
            {
                var items = context.FORECASTER_HOLIDAYS.Where(i => i.HOLIDAYID == this.HOLIDAYID);
                foreach (var i in items)
                    context.FORECASTER_HOLIDAYS.Remove(i);
                context.SaveChanges();
                //dt.Delete(this.HOLIDAYID, this.START, this.END, this.DESCRIPTION);
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
