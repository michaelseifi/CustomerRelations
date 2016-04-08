using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics ;
using System.Windows.Threading;
using daisybrand.forecaster.Controlers.Interfaces;
using ViewModels = daisybrand.forecaster.Controlers.ViewModels;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Presentation.Views;
using daisybrand.forecaster.Extensions;
using System.IO;

namespace daisybrand.forecaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region variables
        static bool IsBusy;
        
        public static Settings mySettings;
        public static Identity myIdentity;

        public static HolidayCollection myHolidays;
        public static Customer.Settings myCustomerSettings;
        public static Sku.Settings mySkuSettings;
        public static SkuCollection mySkus;
        
        public static ViewModels.MainWindow myMainWindowViewModel;
        public static ViewModels.TopMenu myTopMenuViewModel;
        public static ViewModels.StatusBar_Main myStatusBarViewModel;
        public static ViewModels.CustomerSearch myCustomerSearchViewModel;
        

        public static UserCollection myUsers;

        //WINDOWS
        public static Presentation.Views.Graphs graphWindow;
        public static Presentation.Views.DailyQAWeeklyQS dailyQaWeeklyQsWindow;
        public static Presentation.Views.WeeklyQSOrderQuantity weeklyQsOrderQuantityWindow;
        //public static Graphs myGraphWindow;
        #endregion
        public MainWindow()
        {
            App.appSetiings =  SettingsCollection.Get();
            mySettings = App.appSetiings.Where(x => x.EMPLID.ToUpper() == Environment.UserName.ToUpper()).FirstOrDefault();
            

            myHolidays = HolidayCollection.Get();
            myMainWindowViewModel = new ViewModels.MainWindow()
            {
                APPLICATION_VERSION = 
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
            };

            //CHECK FOR NEW VERSION            
            myMainWindowViewModel.IsNewVersion = mySettings.VERSION == null || mySettings.VERSION != myMainWindowViewModel.APPLICATION_VERSION.ToString();
            mySettings.VERSION = myMainWindowViewModel.APPLICATION_VERSION.ToString();
            _SetTestTitle();
            _SetReleaseTitle();
            //myIdentity = Identity.GetIdnetity("jbumpass");
            myIdentity = Identity.GetIdnetity(Environment.UserName);
            myMainWindowViewModel.NAME = myIdentity.Name;

            System.Windows.Forms.Screen screen = 
                System.Windows.Forms.Screen.FromHandle(
                    new System.Windows.Interop.WindowInteropHelper(this).Handle);

            int height = mySettings.HEIGHT > 0 
                ? Convert.ToInt32(mySettings.HEIGHT) : screen.Bounds.Height - 90;
            myMainWindowViewModel.HEIGHT = height;
            myMainWindowViewModel.ORIG_HEIGHT = height;

            int width = mySettings.WIDTH > 0 
                ? Convert.ToInt32(mySettings.WIDTH) : screen.Bounds.Width - 90;
            myMainWindowViewModel.WIDTH = width;
            myMainWindowViewModel.ORIG_WIDTH = width;

            int top = Convert.ToInt32(mySettings.TOP);
            myMainWindowViewModel.TOP = top;
            myMainWindowViewModel.ORIG_TOP = top;

            int left = Convert.ToInt32(mySettings.LEFT);
            myMainWindowViewModel.LEFT = left;
            myMainWindowViewModel.ORIG_LEFT = left;

            //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();           
           
            //((ICustomer)myCustomer).NETWORKALIAS = "";
            //Window myWindow = Tools.GetParentWindow(this.QUANTITY_GRID);
            //System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(myWindow).Handle);
            //this.LAYOUTCONTROL.MaxHeight = screen.Bounds.Height - 50; 

            
        }

        [Conditional("Debug")]
        void _SetTestTitle()
        {
            myMainWindowViewModel.TITLE = "Forecaster Test";
        }

        [Conditional("Release")]
        void _SetReleaseTitle()
        {
            myMainWindowViewModel.TITLE = "Forecaster";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            if (myIdentity.IsAuthenticated)
            {
                //if (MessageBox.Show(
                //        String.Format("Welcome {0}\r\rOnly authorized users are allowed to use this software.\r\rIf you are not authorized to use this software or/and if your name is not {0}, cancel and contact your administrator.", myIdentity.Name), 
                //        "Welcome", 
                //        MessageBoxButton.OKCancel, 
                //        MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                //    Process.GetCurrentProcess().Kill();
                
                if (_GetMySkus() && _GetMyUsers(mySkus) )
                {
                    if (myTopMenuViewModel != null)
                        myTopMenuViewModel.IS_CUSTOMER_COLLECTION_LOADED = true;
                    SkuCollection.GetUsersSkusAsync();
                    Customer.Settings.GetCustomersSettingsAsync();
                    Sku.Settings.GetSkuSettingsAsync();
                    ViewModels.TopMenu.GetListOfCustomersAsync();

                    //PerformanceExceptionsCollection.GetAsync();
                   
                    

                    myCustomerSearchViewModel = new ViewModels.CustomerSearch();
               
                    var uc = new Presentation.UserControls.CustomerSearch()
                    {
                        _QUANTITY_GRID = this.QUANTITY_GRID,                        
                    };
                    uc.Loaded += (s, args) =>
                    {
                        uc.Owner = this;
                    };
                    
                    CUSTOMER_SEARCH.Content = uc;
                    this.DataContext = myMainWindowViewModel;
                    //mainWindow = this;
                    
                   

                }
                else
                {
                    MessageBox.Show("System is unable to authenticate your credentials");
                    Process.GetCurrentProcess().Kill();
                }
            }
            else
            {
                MessageBox.Show("System is unable to authenticate your credentials");
                Process.GetCurrentProcess().Kill();
            }
            InitiateEventHandlers();
            if (App.screen != null) App.screen.Close(new TimeSpan(0));
        }

        public static void CloseAllOtherWindowsIfOpen()
        {
            if (graphWindow != null) graphWindow.Close();
            if (dailyQaWeeklyQsWindow != null) dailyQaWeeklyQsWindow.Close();
            if (weeklyQsOrderQuantityWindow != null) weeklyQsOrderQuantityWindow.Close();  
        }

        #region busyindicator
        public void SetBusyIndicatorState(bool busy)
        {
            this.BUSYINDICATOR.IsBusy = busy;
        }

        public static void SetBusyState(bool busy)
        {
            try
            {
                if (busy != IsBusy)
                {
                    IsBusy = busy;
                    String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                    Mouse.OverrideCursor = busy ? new Cursor(appStartPath + "\\tornado.cur") : null;
                    if (IsBusy)
                    {                        //MainWindow.myTimer =
                        new System.Windows.Threading.DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, dispatcherTimer_Tick, Application.Current.Dispatcher);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        static void dispatcherTimer_Tick(object sender, EventArgs e)
        {            
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null)            {
                
                SetBusyState(false);
                dispatcherTimer.Stop();
            }
        }

        //public void SetBusyIndicator(bool isBusy, string message)
        //{
        //    this.BUSYINDICATOR.IsBusy = isBusy;
        //    this.BUSYINDICATOR.BusyContent = message;
        //}
        #endregion

        #region event handlers
        void TOP_MENU_ExitClick(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region events
        void TOP_MENU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var barManger = (sender as Presentation.UserControls.TopMenu).Content as Controls._BarManger;
            if (barManger.IsClickOnEmptySpot)
            {
                barManger.IsClickOnEmptySpot = false;
                e.Handled = true;

            }
            else
            {
                var w = this.Width;
                var h = this.Height;
                if (w > myMainWindowViewModel.ORIG_WIDTH || h > myMainWindowViewModel.ORIG_HEIGHT)
                    _NormalizeScreen();
                else
                    _MaximizeScreen();
            }
        }

        void TOP_MENU_NormalizeScreenClick(object sender, EventArgs e)
        {
            _NormalizeScreen();
        }

        void TOP_MENU_MaximizeScreenClick(object sender, EventArgs e)
        {
            _MaximizeScreen();
        }

        void TOP_MENU_FullScreenClick(object sender, EventArgs e)
        {
            _FullScreen();
        }
        #endregion

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (WindowState == System.Windows.WindowState.Normal)
                    _FullScreen();
                else
                    this.WindowState = System.Windows.WindowState.Normal;
            }
        }
        private void CUSTOMER_HAS_COMMENT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left) { }
            if (MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.SETTING != null)
                MessageBox.Show(MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.SETTING.COMMENT, MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.ACCOUNT_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            else if(MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null)
                MessageBox.Show(MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.COMMENT, MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.ACCOUNT_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
                mySettings.HEIGHT = (e.NewSize.Height);
            if (e.WidthChanged)
                mySettings.WIDTH = (e.NewSize.Width);            
        }

        private void window_LocationChanged(object sender, EventArgs e)
        {
            var win = sender as Window;
            mySettings.LEFT = win.Left;
            mySettings.TOP = win.Top;            
        }


    }
}
