using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class MainWindow: INotifyPropertyChanged
    {
        public enum WindowsState
        {
            None,
            Maximized,
            Minimized,
            Normal
        }

        public enum Build
        {
            Debug,
            Release
        }
        #region general properties
        
        private WindowsState _WINDOWS_STATE;
        private bool _IsThereCustomerComment;
        private string _BUSYINDICATOR_TEXT;
        private Controlers.Collections.TabCollection _TABS;
        private string _TITLE;
        private bool _IsReadOnly;
        private int _LEFT;
        private int _TOP;
        private int _WIDTH;
        private int _HEIGHT;
        private string _NAME;
        private string _TOPTEXT;
        private Version _APPLICATION_VERSION;
        private bool _IsActualOrderRereshing;
        public WindowsState WINDOWS_STATE
        {
            get
            {
                return _WINDOWS_STATE;
            }
            set
            {
                _WINDOWS_STATE = value;
                NotifyPropertyChanged();
            }
        }

        public string TITLE
        {
            get
            {
                return _TITLE;
            }
            set
            {
                _TITLE = value;
                NotifyPropertyChanged();
                TopMenu.SetTitle(value);
            }
        }

        public Version APPLICATION_VERSION
        {
            get
            {
                return _APPLICATION_VERSION;
            }
            set
            {
                if (_APPLICATION_VERSION == value) return;
                _APPLICATION_VERSION = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsNewVersion
        {
            get;
            set;
        }
        public string NAME
        {
            get
            {
                return _NAME;
            }
            set
            {
                _NAME = value;
                TOPTEXT = "You are logged in as " + value;
                NotifyPropertyChanged();
            }
        }
        
        public string TOPTEXT
        {
            get
            {
                return _TOPTEXT;
            }
            private set
            {
                if (_TOPTEXT == value) return;
                _TOPTEXT = value;
                NotifyPropertyChanged();
            }
        }

        public int HEIGHT
        {
            get
            {
                return _HEIGHT;
            }
            set
            {
                _HEIGHT = value;
                NotifyPropertyChanged();
            }
        }

        public int ORIG_HEIGHT { get; set; }

        public int WIDTH
        {
            get
            {
                return _WIDTH;
            }
            set
            {
                _WIDTH = value;                
                NotifyPropertyChanged();
            }
        }

        public int ORIG_WIDTH { get; set; }

        public int TOP
        {
            get
            {
                return _TOP;
            }
            set
            {
                _TOP = value;
                NotifyPropertyChanged();
            }
        }

        public int ORIG_TOP { get; set; }

        public int LEFT
        {
            get
            {
                return _LEFT;
            }
            set
            {
                _LEFT = value;
                NotifyPropertyChanged();
            }
        }

        public int ORIG_LEFT { get; set; }

        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                if (_IsReadOnly == value)
                    return;
                _IsReadOnly = value;
                NotifyPropertyChanged();
                
                
                
            }
        }

        public bool IsActualOrderRereshing
        {
            get
            {
                return _IsActualOrderRereshing;
            }
            set
            {
                _IsActualOrderRereshing = value;                
                NotifyPropertyChanged();
                
            }
        }

        public bool IsThereCustomerComment
        {
            get
            {
                return _IsThereCustomerComment;
            }
            private set
            {
                _IsThereCustomerComment = value && TABS != null;
                NotifyPropertyChanged();
            }
        }


        //RESETS THE COMMENT ICON
        public void SetIsThereCustomerComment(Controlers.Objects.Customer.Setting setting)
        {
            IsThereCustomerComment = !string.IsNullOrEmpty(setting.COMMENT);
        }

        public System.Windows.Forms.Screen SCREEN_PRIMARY
        {
            get { return System.Windows.Forms.Screen.PrimaryScreen; }
        }

        public System.Windows.Forms.Screen SCREEN_SECONDARY
        {
            get
            {
                var all = System.Windows.Forms.Screen.AllScreens;
                foreach (var screen in all)
                {
                    if (!screen.Primary)
                        return screen;
                }
                return System.Windows.Forms.Screen.PrimaryScreen;
            }
        }


        public string BUSYINDICATOR_TEXT
        {
            get
            {
                if (string.IsNullOrEmpty(_BUSYINDICATOR_TEXT)) return "Please wait ...";
                return _BUSYINDICATOR_TEXT;
            }
            set
            {
                _BUSYINDICATOR_TEXT = value;
                NotifyPropertyChanged();
            }
        }

        bool RunTheClock { get; set; }

        public async void SetBUSYINDICATOR_TEXT_TimerAsync(bool run = true)
        {
            await SetBUSYINDICATOR_TEXT_TimerTask(run);
        }

        private Task<bool> SetBUSYINDICATOR_TEXT_TimerTask(bool run)
        {
            return Task.Run<bool>(() => SetBUSYINDICATOR_TEXT_Timer(run));
        }
        
        private bool SetBUSYINDICATOR_TEXT_Timer(bool run)
        {
            RunTheClock = run;
            if (!run) return true;
            var time = DateTime.Now;            
            while (RunTheClock)
            {
                var timer = DateTime.Now;
                var span = new TimeSpan(timer.Ticks - time.Ticks);               
                if (!RunTheClock) return true;
                BUSYINDICATOR_TEXT = String.Format("Please wait ... ({0})", span);                
            }
            return true;
        }
        #endregion

        #region object properties
        public Controlers.Collections.TabCollection TABS
        {
            get
            {
                return _TABS;
            }
            set
            {
                _TABS = value;
                NotifyPropertyChanged();
                value.NotifyPropertyChanged(null);
                RefreshStatusBarAddUri();
                RefreshTopMenuProperties();
            }
        }

        public void RefreshStatusBarAddUri()
        {
            if (forecaster.MainWindow.myStatusBarViewModel != null &&
                this.TABS != null &&
                this.TABS.FOCUSEDCUSTOMER != null)
                forecaster.MainWindow.myStatusBarViewModel.ADD_URL = this.TABS.FOCUSEDCUSTOMER.SETTING.ADD_URL_URI;
        }

        void RefreshTopMenuProperties()
        {
            if (forecaster.MainWindow.myTopMenuViewModel != null && this.TABS.FOCUSEDCUSTOMER != null)
                forecaster.MainWindow.myTopMenuViewModel.NotifyPropertyChanged("STORE_COUNT");
        }

        public static Controlers.Collections.TabCollection GetTABS()
        {
            if (daisybrand.forecaster.MainWindow.myMainWindowViewModel != null && daisybrand.forecaster.MainWindow.myMainWindowViewModel.TABS != null)
                return daisybrand.forecaster.MainWindow.myMainWindowViewModel.TABS;
            return null;
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
