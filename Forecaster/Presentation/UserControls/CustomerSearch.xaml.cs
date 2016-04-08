using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using DevExpress.Xpf.Editors;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using Models = daisybrand.forecaster.Controlers.ViewModels;

namespace daisybrand.forecaster.Presentation.UserControls
{
    
    /// <summary>
    /// Interaction logic for CustomerSearch.xaml
    /// </summary>
    public partial class CustomerSearch : UserControl
    {
        
        //public event EventHandler RefreshDataContext;
        public ContentControl _QUANTITY_GRID { get; set; }        
        public MainWindow Owner { get; set; }
        public CustomerSearch()
        {
            this.DataContext = MainWindow.myCustomerSearchViewModel;
            MainWindow.myTopMenuViewModel.OPEN_FULL_VIEW_BTN_CLICKED 
                += myTopMenuViewModel_OPEN_FULL_VIEW_BTN_CLICKED;
            MainWindow.myTopMenuViewModel.QUICK_VIEW_BTN_CLICKED 
                += myTopMenuViewModel_QUICK_VIEW_BTN_CLICKED;
            InitializeComponent();            
        }

        void myTopMenuViewModel_QUICK_VIEW_BTN_CLICKED(object sender, EventArgs e)
        {
            QUICK_VIEW_BTN_Clicked();
        }

        void myTopMenuViewModel_OPEN_FULL_VIEW_BTN_CLICKED(object sender, EventArgs e)
        {
            OK_BTN_Clicked();
        }

        public void Update(Controlers.Interfaces.IUser user, Controlers.Interfaces.ICustomer customer)
        {
            LogManger.InsertStep();
            USERS_COMBO.SelectedItem = user;
            CUSTOMERS_COMBO.SelectedItem = customer;
        }

        private void USERS_COMBO_Initialized(object sender, EventArgs e)
        {
            LogManger.InsertEvent();
            USERS_COMBO.ItemsSource = MainWindow.myUsers;
        }

        private void USERS_COMBO_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            if (MainWindow.myUsers.FocusedEmplid == null) { USERS_COMBO.Focus(); return; }
            USERS_COMBO.SelectedItem = MainWindow.myUsers.FocusedEmplid;
            CUSTOMERS_COMBO.Focus();             
        }



        /// <summary>
        /// GET DATA FOR THE CUSTOMER AND POPULATE THE GRID CONTROL
        /// </summary>
        private void OK_BTN_Click(object sender, RoutedEventArgs e)
        {            
            OK_BTN_Clicked();
        }

        private void OK_BTN_Clicked()
        {
            if (PosCollections.PosCancellationTokenSource != null)
                PosCollections.PosCancellationTokenSource.Cancel();
            App.AbortDispatcherOperations();
            LogManger.InsertEvent();
            MainWindow.myTopMenuViewModel.IS_DATA_LOEADED = false;
            _LoadGrid(
                VIEW.SelectedIndex, 
                int.Parse(NUMBER_OF_ITERATIONS.SelectedItemValue.ToString()));
        }

        private void QUICK_VIEW_BTN_Click(object sender, RoutedEventArgs e)
        {            
            QUICK_VIEW_BTN_Clicked();
        }

        private void QUICK_VIEW_BTN_Clicked()
        {
            if (PosCollections.PosCancellationTokenSource != null)
                PosCollections.PosCancellationTokenSource.Cancel();
            App.AbortDispatcherOperations();
            VIEW.SelectedIndex = 1;
            NUMBER_OF_ITERATIONS.SelectedIndex = 1;
            MainWindow.myTopMenuViewModel.IS_DATA_LOEADED = false;
            _LoadGrid(1, 2);
        }
        private async void _LoadGrid (int view, int iteration)
        {
            //MainWindow.SetBusyIndicator(true);
            if (USERS_COMBO.SelectedIndex < 0 || CUSTOMERS_COMBO.SelectedIndex < 0 || SKUIDS_COMBO.SelectedIndex < 0)
            {
                MessageBox.Show("Please select all criteria", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Dispose of the previous data, if it exists
            if (MainWindow.myMainWindowViewModel.TABS != null)
                MainWindow.myMainWindowViewModel.TABS.Dispose();
            //Owner.SetBusyIndicatorState(true);
            //start the tabcollection here. but add contents to it as each content gets built
            //if this is the full view, set the minimze flag so the window would minimze
            var cust = ((ICustomer)CUSTOMERS_COMBO.SelectedItem);
            var skus
                = ((ListCollectionView)SKUIDS_COMBO.ItemsSource).Cast<Controlers.Interfaces.ISku>();
            MainWindow.myMainWindowViewModel.TABS = new TabCollection(skus, cust)
            {
                OWNER = Owner,
                IsLoading = true,
                //MinimizeWin = view != 1
            };

            MainWindow.myMainWindowViewModel.SetIsThereCustomerComment(cust.SETTING);


            //IEnumerable<Task> perfExcepTasks = MainWindow.myMainWindowViewModel.TABS.Select(t => PerformanceExceptionsCollection.GetAsync(t));
            //await Task.WhenAll(perfExcepTasks);
            //Owner.SetBusyIndicatorState(false);
            Controlers.Objects.OrderStatus.SalesOrderStatusList
                = Controlers.Objects.OrderStatus.GetNameFromMainDispatcher();
            await PosCollections.GetDivisionsTask(int.Parse(cust.ACCOUNTNUM));
            foreach (var tab in MainWindow.myMainWindowViewModel.TABS)
            {
                tab.Setting
                    = MainWindow.mySettings.GetCustomer(cust.ACCOUNTNUM).GetTab(tab.SKU.SKUID);
                _CallLoadWeeklyGridAsync(
                    ((ICustomer)(CUSTOMERS_COMBO.SelectedItem)).ACCOUNTNUM, tab, iteration, view);
            }

            MainWindow.myCustomerSearchViewModel.SELECTED_USER_PREVIOUS
                = (USERS_COMBO.SelectedItem as IUser).DISPLAY_VALUE;
            MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER_PREVIOUS
                = (CUSTOMERS_COMBO.SelectedItem as ICustomer).ACCOUNT_NAME;
            MainWindow.myCustomerSearchViewModel.SELECTED_SKU_PREVIOUS
                = (SKUIDS_COMBO.SelectedItem as ISku).DISPLAY_VALUE;
            MainWindow.myCustomerSearchViewModel.SELECTED_VIEW_PREVIOUS
                = (VIEW.SelectedItem as ComboBoxEditItem).Content.ToString();

            MainWindow.myCustomerSearchViewModel.ITERATION
                = (string)((ComboBoxEditItem)NUMBER_OF_ITERATIONS.SelectedItem).Content;
            MainWindow.myCustomerSearchViewModel.NotifyPropertyChanged(null);
            _QUANTITY_GRID.Content = new UserControls.Tabs();

            MainWindow.myTopMenuViewModel.SEARCH_VALUE
                = ((ICustomer)(CUSTOMERS_COMBO.SelectedItem)).ACCOUNTNUM;

            Owner.SetBusyIndicatorState(true);
            MainWindow.myMainWindowViewModel.SetBUSYINDICATOR_TEXT_TimerAsync(true);
            if (MainWindow.myMainWindowViewModel.TABS.MinimizeWin)
            {
                await Tools.WaitTask(1);
                //Owner.MinimizeScreen();
            }
        }

        //private void myQuantities_RefreshDataContext(object sender, EventArgs e)
        //{
        //    if (RefreshDataContext != null)
        //        RefreshDataContext(this, EventArgs.Empty);
        //}

        private void USERS_COMBO_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            //IUser user = (IUser)sender;
            //MainWindow.myUsers.FocusedEmplid = (IUser)((_ComboBoxEdit_Users)sender).SelectedItem;
            //MainWindow.myCustomerSearchViewModel.IS_USER_BOX_SELECTED = USERS_COMBO.SelectedIndex > -1;
            //MainWindow.myTopMenuViewModel.IS_USER_SELECTED = USERS_COMBO.SelectedIndex > -1;
        }

        private void CUSTOMERS_COMBO_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            SKUIDS_COMBO.SelectedIndex = 0;
            SKUIDS_COMBO.IsEnabled = false;
            //MainWindow.myCustomerSearchViewModel.IS_CUSTOMER_BOX_SELECTED = CUSTOMERS_COMBO.SelectedIndex > -1;
            //if (CUSTOMERS_COMBO.SelectedIndex > -1)
            //    MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer = CUSTOMERS_COMBO.SelectedItem as Controlers.Objects.Customer;
        }

        private void SKUIDS_COMBO_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku = SKUIDS_COMBO.SelectedItem as Controlers.Objects.Sku;
            //MainWindow.myCustomerSearchViewModel.se
            //MainWindow.myCustomerSearchViewModel.IS_SKU_BOX_SELECTED = SKUIDS_COMBO.SelectedIndex > -1;
            //MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer.FocusedSku = SKUIDS_COMBO.SelectedItem as Controlers.Interfaces.ISku;
        }

        //private void CUSTOMERS_COMBO_Initialized(object sender, EventArgs e)
        //{
        //    CUSTOMERS_COMBO.ItemsSource = MainWindow.myCustomers;
        //}

        private void SKUIDS_COMBO_Initialized(object sender, EventArgs e)
        {
            //SKUIDS_COMBO.ItemsSource = MainWindow.mySkus;
        }

        private void VIEW_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            _SetNumberOfIterations();           
        }

        private void VIEW_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            _SetNumberOfIterations();
        }



  


        
    }
}
