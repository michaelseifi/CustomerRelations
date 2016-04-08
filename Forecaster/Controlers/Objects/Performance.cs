using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Enums;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Performance : IPerformance, INotifyPropertyChanged
    {

        #region properties
        
        private string _MODIFIED_BY;
        private DateTime _MODIFIED_DATE;
        private string _CREATED_BY;
        private DateTime _CREATED_DATE;
        private MState _M_STATE;
         private DState _D_STATE;
        private bool _IS_DEFAULT;
        private bool _IS_INCLUDED;
        private string _CONFIRMED_BY;
        private string _CONFIRMED_DATE_STR;
        private DateTime _CONFIRMED_DATE;
        private bool _IS_CONFIRMED;
        private double _PRICE;
        private double _QUANTITY;
        private string _PERFORMANCE_TYPE;
        private int _PERFORMANCE_TYPEID;
        private string _CUSTOMER_ID;
        private string _SKU_ID;
        private string _PROMOTION_NUMBER;
        private Guid _DOCUMENTOBJECTID;
        private string _BANNERS_TOOLTIP;
        private string _FIRST_BANNER;
        private IEnumerable<string> _BANNERS;
        private string _REGIONAL_MNGR;
        private string _CR_ASSOCIATE;
        private string _TP_NAME;
        private string _AD_NAME;
        private DateTime _REPORT_AS_OF_DATE;
        private string _WEEKID;
        private DateTime _END_DATE;
        private DateTime _START_DATE;



        public bool IS_INCLUDED
        {
            get
            {
                return _IS_INCLUDED;
            }
            set
            {
                _IS_INCLUDED = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_DEFAULT
        {
            get
            {
                return _IS_DEFAULT;
            }
            set
            {
                _IS_DEFAULT = value;
                NotifyPropertyChanged();
            }
        }

        public Guid DOCUMENTOBJECTID
        {
            get
            {
                return _DOCUMENTOBJECTID;
            }
            set
            {
                _DOCUMENTOBJECTID = value;
                NotifyPropertyChanged();
            }
        }
        public string PROMOTION_NUMBER
        {
            get
            {
                return _PROMOTION_NUMBER;
            }
            set
            {
                _PROMOTION_NUMBER = value;
                NotifyPropertyChanged();
            }
        }
        public string SKU_ID
        {
            get
            {
                return _SKU_ID;
            }
            set
            {
                _SKU_ID = value;
                NotifyPropertyChanged();
            }
        }
        public string CUSTOMER_ID
        {
            get
            {
                return _CUSTOMER_ID;
            }
            set
            {
                _CUSTOMER_ID = value;
                NotifyPropertyChanged();
            }
        }
        [Obsolete("Use TYPE_ID instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int PERFORMANCE_TYPEID
        {
            get
            {
                return TYPE_ID;
            }
            set
            {
                TYPE_ID = value;
            }
        }
        public int TYPE_ID
        {
            get
            {
                return _PERFORMANCE_TYPEID;
            }
            set
            {
                _PERFORMANCE_TYPEID = value;
                NotifyPropertyChanged();
            }
        }
        [Obsolete("Use TYPE instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string PERFORMANCE_TYPE
        {
            get
            {
                return TYPE;
            }
            set
            {
                TYPE = value;
            }
        }
        public string TYPE
        {
            get
            {
                return _PERFORMANCE_TYPE;
            }
            set
            {
                _PERFORMANCE_TYPE = value;
                NotifyPropertyChanged();
            }
        }
        public double QUANTITY
        {
            get
            {
                return _QUANTITY;
            }
            set
            {
                _QUANTITY = value;
                NotifyPropertyChanged();
            }
        }
        public double PRICE
        {
            get
            {
                return _PRICE;
            }
            set
            {
                _PRICE = value;
                NotifyPropertyChanged();
            }
        }
        public bool IS_CONFIRMED
        {
            get
            {
                return _IS_CONFIRMED;
            }
            set
            {
                _IS_CONFIRMED = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime CONFIRMED_DATE
        {
            get
            {
                return _CONFIRMED_DATE;
            }
            set
            {
                _CONFIRMED_DATE = value;
                NotifyPropertyChanged();
            }
        }
        public string CONFIRMED_DATE_STR
        {
            get
            {
                return _CONFIRMED_DATE_STR;
            }
            set
            {
                _CONFIRMED_DATE_STR = value;
                NotifyPropertyChanged();
            }
        }
        public string CONFIRMED_BY
        {
            get
            {
                return _CONFIRMED_BY;
            }
            set
            {
                _CONFIRMED_BY = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime CREATED_DATE
        {
            get
            {
                return _CREATED_DATE;
            }
            set
            {
                _CREATED_DATE = value;
                NotifyPropertyChanged();
            }
        }
        public string CREATED_BY
        {
            get
            {
                return _CREATED_BY;
            }
            set
            {
                _CREATED_BY = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime MODIFIED_DATE
        {
            get
            {
                return _MODIFIED_DATE;
            }
            set
            {
                _MODIFIED_DATE = value;
                NotifyPropertyChanged();
            }
        }
        public string MODIFIED_BY
        {
            get
            {
                return _MODIFIED_BY;
            }
            set
            {
                _MODIFIED_BY = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime START_DATE
        {
            get
            {
                return _START_DATE;
            }
            set
            {
                _START_DATE = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime END_DATE
        {
            get
            {
                return _END_DATE;
            }
            set
            {
                _END_DATE = value;
                NotifyPropertyChanged();
            }
        }

        public string WEEKID
        {
            get
            {
                return _WEEKID;
            }
            set
            {
                _WEEKID = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime REPORT_AS_OF_DATE
        {
            get
            {
                return _REPORT_AS_OF_DATE;
            }
            set
            {
                _REPORT_AS_OF_DATE = value;
                NotifyPropertyChanged();
            }
        }

        public string AD_NAME
        {
            get
            {
                return _AD_NAME;
            }
            set
            {
                _AD_NAME = value;
                NotifyPropertyChanged();
            }
        }

        public string TP_NAME
        {
            get
            {
                return _TP_NAME;
            }
            set
            {
                _TP_NAME = value;
                NotifyPropertyChanged();
            }
        }

        public string CR_ASSOCIATE
        {
            get
            {
                return _CR_ASSOCIATE;
            }
            set
            {
                _CR_ASSOCIATE = value;
                NotifyPropertyChanged();
            }
        }

        public string REGIONAL_MANAGER
        {
            get
            {
                return _REGIONAL_MNGR;
            }
            set
            {
                _REGIONAL_MNGR = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<string> BANNERS
        {
            get
            {
                return _BANNERS;
            }
            set
            {
                _BANNERS = value;
                FIRST_BANNER = value.FirstOrDefault();
                //SetBannersToolTip();
                NotifyPropertyChanged();
            }
        }

        public string FIRST_BANNER
        {
            get
            {
                return _FIRST_BANNER;
            }
            set
            {
                _FIRST_BANNER = value;
                NotifyPropertyChanged();
            }
        }

        public string BANNERS_TOOLTIP
        {
            get
            {
                return _BANNERS_TOOLTIP;
            }
            set
            {
                _BANNERS_TOOLTIP = value;
                NotifyPropertyChanged();
            }
        }

        public string STATUS
        {
            get;
            set;
        }

        public DState D_STATE
        {
            get
            {
                return _D_STATE;
            }
            set
            {
                _D_STATE = value;
                NotifyPropertyChanged();
            }
        }
        public MState M_STATE
        {
            get
            {
                return _M_STATE;
            }
            set
            {
                _M_STATE = value;
                NotifyPropertyChanged();
            }
        }

        public string STATE_USER { get; set; }
        public DateTime STATE_LAST_UPDATED { get; set; }

        #endregion

        public static int GetTypeId(string type)
        {
            if (type.ToUpper() == "TPR") return 2;
            else if (type.ToUpper() == "AD") return 1;
            else return 0;
        }

        public static string GetState(IPerformance perf)
        {
            if(perf.IS_DEFAULT)
            {
                switch (perf.D_STATE)
                {
                    case(DState.Check):
                        return "C";
                    case(DState.Confirmed):
                        return "CNF";
                    case(DState.None):
                        return "N";                        
                }
            }
            else
            {
                switch (perf.M_STATE)
                {
                    case MState.DidNotKnow:
                        return "D";
                    case MState.Confirmed:
                        return "CNF";
                    //case MState.None:
                    //    return "N";
                }
            }
            return "";
        }
        //void SetBannersToolTip()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (string s in BANNERS)
        //        sb.Append(s + "\r");           
        //    BANNERS_TOOLTIP = sb.ToString();
        //}


        
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
