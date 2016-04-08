using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.Native;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    { 
        #region event handlers
        public event EventHandler ExitClick;
        public event EventHandler MaximizeScreenClick;
        public event EventHandler NormalizeScreenClick;
        public event EventHandler FullScreenClick;
        #endregion
        public MainMenu()
        {
            try
            {
                //MainWindow.myTopMenuViewModel = new Controlers.ViewModels.TopMenu()
                //{
                //    APPLICATION_VERSION = MainWindow.myMainWindowViewModel.APPLICATION_VERSION,
                //    Is_ALL_GRAPH_Enabled = false,
                //    Is_DAILYQA_WEEKLYQS_Enabled = false,
                //    IS_ADMIN = MainWindow.myIdentity.IsAdmin,
                //    TITLE = MainWindow.myMainWindowViewModel.TITLE
                //};
            }
            catch { }
            InitializeComponent();
            BAR_MANGER.LayoutUpdated += BAR_MANGER_LayoutUpdated;
            this.Loaded += (s, e) =>
            {
                Window.GetWindow(this).Closing += (s1, e1) =>
                {
                    var path = Caching.GetFilePath(Caching.Folder.xml, "mainmenubars");
                    BAR_MANGER.SaveLayoutToXml(path);
                };
            };
        }

        #region events
        //File
        private void EXIT_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OnExitClick();
        }



        //Edit
        private void CUSTOMER_EXCEPTION_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            var win = new Presentation.Views.Exceptions();
            win.CONTENT = new CustomerExceptions() { OWNER = win };
            win.ShowDialog();
        }

        private void SKU_EXCEPTION_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            var win = new Presentation.Views.Exceptions();
            win.CONTENT = new SkuExceptions() { OWNER = win };
            win.ShowDialog();
        }

        private void PERFORMANCE_EXCEPTION_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            var win = new Presentation.Views.Exceptions();
            win.CONTENT = new PerformanceExceptions(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB) { OWNER = win };
            win.ShowDialog();
        }

        private void HOLIDAYS_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.IsReadOnly)
            {
                MessageBox.Show("The app is in read only mode.");
                return;
            }
            var win = new Presentation.Views.Exceptions() { Title = "Holidays" };
            win.CONTENT = new Holidays() { OWNER = win };
            win.ShowDialog();
        }

        private void CUSTOMER_SETTINGS_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.IsReadOnly)
            {
                MessageBox.Show("The app is in read only mode.");
                return;
            }
            var win = new Presentation.Views.Exceptions() { Title = "Customer Settings" };
            win.CONTENT = new CustomerSettings() { OWNER = win };
            win.ShowDialog();
        }

        private void READ_ONLY_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel != null)
                MainWindow.myMainWindowViewModel.IsReadOnly = !MainWindow.myMainWindowViewModel.IsReadOnly;
            READ_ONLY_BTN.Content = MainWindow.myMainWindowViewModel.IsReadOnly ? "Change to edit mode" : "Change to read only mode";
        }
        //View
        private void HEIGHT_SHRINK_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {

            MainWindow.myMainWindowViewModel.HEIGHT -= 20;
        }


        private void WIDTH_SHRINK_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow.myMainWindowViewModel.WIDTH -= 20;
        }

        private void HEIGHT_ENLARGE_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow.myMainWindowViewModel.HEIGHT += 20;
        }

        private void WIDTH_ENLARGE_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow.myMainWindowViewModel.WIDTH += 20;
        }
        //statics
        //private void CUSTOMER_HAS_COMMENT_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    if (MainWindow.myTopMenuViewModel.FOCUSED_CUSTOMER != null && MainWindow.myTopMenuViewModel.FOCUSED_CUSTOMER.SETTING != null)
        //        MessageBox.Show( MainWindow.myTopMenuViewModel.FOCUSED_CUSTOMER.SETTING.COMMENT, "Comment", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
        //Far right
        private void staticClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OnExitClick();
        }


        private void MAXIMIZE_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (MaximizeScreenClick != null)
                MaximizeScreenClick(this, EventArgs.Empty);
        }

        private void FULLSCREEN_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (FullScreenClick != null)
                FullScreenClick(this, EventArgs.Empty);
        }

        private void staticMinimize_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Application.Current.MainWindow.Topmost = false;
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void NORMAL_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (NormalizeScreenClick != null)
                NormalizeScreenClick(this, EventArgs.Empty);
        }

        private void staticMaximize_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MaximizeScreenClick != null)
                MaximizeScreenClick(this, EventArgs.Empty);
        }

        private void ALL_GRAPH_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MainWindow.graphWindow = new Views.Graphs(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB);
            MainWindow.graphWindow.Show();
        }

        private void DAILY_QA_WEEKLY_QS_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MainWindow.dailyQaWeeklyQsWindow = new Views.DailyQAWeeklyQS(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB);
            MainWindow.dailyQaWeeklyQsWindow.Show();
        }

        private void WEEKLY_QS_ORDERQUANTITY_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MainWindow.weeklyQsOrderQuantityWindow = new Views.WeeklyQSOrderQuantity(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB);
            MainWindow.weeklyQsOrderQuantityWindow.Show();
        }
        #endregion

        #region OnClicks
        //File
        private void OK_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow.myTopMenuViewModel.FullViewBtnClicked();
        }

        private void QUICK_BTN_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow.myTopMenuViewModel.QuickViewBtnClicked();
        }

        protected virtual void OnExitClick()
        {
            if (ExitClick != null)
                ExitClick(this, EventArgs.Empty);
        }


        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.myTopMenuViewModel;
        }

        private void SEARCH_BY_CUSTOMER_EditValueChanged(object sender, RoutedEventArgs e)
        {
            var bar = sender as BarEditItem;
            var custNumber = bar.EditValue.ToString();
            var cust = MainWindow.myUsers.SelectMany(x => x.CUSTOMERS).First(x => x.ACCOUNTNUM == custNumber);
            var user = MainWindow.myUsers.First(x => x.CUSTOMERS.Contains(cust));
            if (MainWindow.myCustomerSearchViewModel != null)
            {
                cust.FocusedSku = cust.SKUS[0];
                MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER = cust;
                MainWindow.myCustomerSearchViewModel.SELECTED_SKU = (daisybrand.forecaster.Controlers.Objects.Sku)cust.FocusedSku;
                MainWindow.myCustomerSearchViewModel.SELECTED_USER = user;

            }
        }

        void BAR_MANGER_LayoutUpdated(object sender, EventArgs e)
        {

        }

        void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SEARCH_BY_CUSTOMER_EditValueChanged(SEARCH_BY_CUSTOMER, null);
            }
            if (e.Key == Key.Tab)
            {

            }
        }

        private void STORE_COUNT_EditValueChanged(object sender, RoutedEventArgs e)
        {
            //var bar = (e.Source) as BarEditItem;
            //if (MainWindow.myUsers != null && MainWindow.myUsers.FocusedEmplid != null && MainWindow.myUsers.FocusedEmplid.CUSTOMERS != null)
            //{
            //    var mainCustFocused = (ICustomer)MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer;
            //    if (mainCustFocused != null)
            //        MainWindow.myTopMenuViewModel.STORE_COUNT = bar.EditValue.ToString();
            //}
        }

        private void AVG_PRICEPOINT_EditValueChanged(object sender, RoutedEventArgs e)
        {

        }

        //private bool isInFocus;
        //void Editor_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        //{
        //    ////Debug.WriteLine("Editor has lost keyboard focus.");
        //    //var box = (TextEdit)sender;

        //    //if (e.NewFocus != null && e.NewFocus.IsKeyboardFocused && LayoutHelper.IsChildElementEx((DependencyObject)sender, (DependencyObject)e.NewFocus))
        //    //{

        //    //}
        //    //else if (e.OldFocus != null && !e.OldFocus.IsKeyboardFocused)
        //    //{
        //    //    //change the property
        //    //    MainWindow.myTopMenuViewModel.AVG_PRICE_POINT_DOLLAR = ((e.OldFocus) as TextBox).Text;
        //    //}
        //}

        //private void AVG_PRICEPOINT_BAR_EDIT_ITEM_LINK_LinkControlLoaded(object sender, BarItemLinkControlLoadedEventArgs e)
        //{
        //    ((BarEditItemLink)e.Link).Editor.LostKeyboardFocus -= new System.Windows.Input.KeyboardFocusChangedEventHandler(Editor_LostKeyboardFocus);
        //    ((BarEditItemLink)e.Link).Editor.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(Editor_LostKeyboardFocus);
        //}

        private void LOG_EVENTS_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myTopMenuViewModel != null)
            {
                MainWindow.myTopMenuViewModel.TRACK_EVENTS = !MainWindow.myTopMenuViewModel.TRACK_EVENTS;
                LOG_EVENTS.Content = (MainWindow.myTopMenuViewModel.TRACK_EVENTS ? "Disable" : "Enable") + " event logging";
            }
        }

        private void LOG_STEPS_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myTopMenuViewModel != null)
            {
                MainWindow.myTopMenuViewModel.TRACK_STEPS = !MainWindow.myTopMenuViewModel.TRACK_STEPS;
                LOG_STEPS.Content = (MainWindow.myTopMenuViewModel.TRACK_STEPS ? "Disable" : "Enable") + " step logging";
            }
        }


        private void PERCENTAGE_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB != null)
            {
                var win = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Owner = Application.Current.MainWindow,
                    Topmost = true
                };
                var perc = new UserControls.Percentage(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB) { WIN = win };
                win.Content = perc;
                win.Show();
            }
        }

        private void VOLATILITY_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB != null)
            {
                var win = new Window
                {                    
                    SizeToContent = SizeToContent.WidthAndHeight,                    
                    Topmost = true,
                    Owner = Application.Current.MainWindow,
                };
                var vol = new UserControls.Volatility(MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB) { WIN = win};
                win.Content = vol;
                win.Show();
            }
        }

        private void CALCULATOR_ItemClick(object sender, ItemClickEventArgs e)
        {
            var win = new Window
            {
                SizeToContent = SizeToContent.WidthAndHeight,
                Topmost = true,                
                Owner = Application.Current.MainWindow,                
            };
            var calc = new UserControls.Calculator { WIN = win };
            win.Content = calc;
            win.Show();
        }



        private void BAR_MANGER_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Caching.GetFilePath(Caching.Folder.xml, "mainmenubars");
            if (System.IO.File.Exists(path))
                if (!MainWindow.myMainWindowViewModel.IsNewVersion)
                    BAR_MANGER.RestoreLayoutFromXml(path);
                else
                    System.IO.File.Delete(path);
        }

        private void PROMOTIONS_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB != null)
            {
                var tab = MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB;
                var win = new Window
                {
                    //SizeToContent = SizeToContent.WidthAndHeight,
                    Topmost = true,
                    Owner = Application.Current.MainWindow,
                    Title = String.Format("Promotions for customer {0} and sku {1}", tab.CUSTOMER_NUMBER, tab.CAPTION)
                };
                var vol = new UserControls.Promotions(tab) {  };
                win.Height = MainWindow.myMainWindowViewModel.HEIGHT - 20;                 
                win.Top = MainWindow.myMainWindowViewModel.TOP;
                win.Content = vol;
                win.Show();
            }
        }

    }
}
