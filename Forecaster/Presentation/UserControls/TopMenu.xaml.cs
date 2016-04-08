using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for TopMenu.xaml
    /// </summary>
    public partial class TopMenu : UserControl
    {
        #region event handlers
        public event EventHandler ExitClick;
        public event EventHandler MaximizeScreenClick;
        public event EventHandler NormalizeScreenClick;
        public event EventHandler FullScreenClick;
        public event MouseButtonEventHandler DragMoveWindow;
        #endregion
        public TopMenu()
        {
            try
            {
                MainWindow.myTopMenuViewModel = new Controlers.ViewModels.TopMenu()
                {
                    APPLICATION_VERSION = MainWindow.myMainWindowViewModel.APPLICATION_VERSION,
                    TOPTEXT = MainWindow.myMainWindowViewModel.TOPTEXT,
                    Is_ALL_GRAPH_Enabled = false,
                    Is_DAILYQA_WEEKLYQS_Enabled = false,
                    IS_ADMIN = MainWindow.myIdentity.IsAdmin,
                    TITLE = MainWindow.myMainWindowViewModel.TITLE
                };
            }
            catch { }
            InitializeComponent();
            BAR_MANGER.LayoutUpdated += BAR_MANGER_LayoutUpdated;
            this.Loaded += (s, e) =>
            {
                Window.GetWindow(this).Closing += (s1, e1) =>
                {
                    var path = Caching.GetFilePath(Caching.Folder.xml, "topmenubars");
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



        //Far right
        private void staticClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OnExitClick();
        }



        private void staticMinimize_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Application.Current.MainWindow.Topmost = false;
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }



        private void staticMaximize_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MaximizeScreenClick != null)
                MaximizeScreenClick(this, EventArgs.Empty);
        }

        #endregion

        #region OnClicks
        //File
   

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
            if (SEARCH_BY_CUSTOMER_BAR_EDIT_ITEM_LINK.Editor != null)
            {
                SEARCH_BY_CUSTOMER_BAR_EDIT_ITEM_LINK.Editor.KeyDown += Editor_KeyDown;
                BAR_MANGER.LayoutUpdated -= BAR_MANGER_LayoutUpdated;
            }
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



        private bool isInFocus;
    


        private void BAR_MANGER_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Caching.GetFilePath(Caching.Folder.xml, "topmenubars");
            if (System.IO.File.Exists(path))
                BAR_MANGER.RestoreLayoutFromXml(path);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DragMoveWindow != null)
                DragMoveWindow(sender, e);
        }

        private void SEARCH_BY_CUSTOMER_BAR_EDIT_ITEM_LINK_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var comboBoxEditItem = (sender as BarEditItemLinkControl).Edit as ComboBoxEdit;
            comboBoxEditItem.SelectAll();
            BAR_MANGER.IsClickOnEmptySpot = true;
        }
    }
}
