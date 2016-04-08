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
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for CustomerSettings.xaml
    /// </summary>
    public partial class CustomerSettings : UserControl
    {
        public Window OWNER { get; set; }
        private Customer.Setting ORIGINAL { get; set; }
        private ICustomer FOCUSED_CUSTOMER { get; set; }
        public CustomerSettings()
        {
            InitializeComponent();
            this.DataContext = MainWindow.myUsers;            
        }

        private void CLOSE_BTN_Click(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            var c = (ICustomer)(CUSTOMERS_COMBO.SelectedItem);
            _SaveIfChanged(c);
            MainWindow.myMainWindowViewModel.SetIsThereCustomerComment(c.SETTING);            
            CUSTOMERS_COMBO = null;
            if (OWNER != null) OWNER.Close();
        }

        private void CUSTOMERS_COMBO_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            //HANDLE NEW ITEM
            var c = (ICustomer)CUSTOMERS_COMBO.SelectedItem;
            FOCUSED_CUSTOMER = c;
            if (c.SETTING == null) c.SETTING = new Customer.Setting() { CUSTOMER_ID = int.Parse(c.ACCOUNTNUM) };
            ORIGINAL = new Customer.Setting
            {
                ADD_URL = c.SETTING.ADD_URL,
                CUSTOMER_ID = c.SETTING.CUSTOMER_ID,
                STORE_COUNT = c.SETTING.STORE_COUNT,
                COMMENT = c.SETTING.COMMENT,
                REPORT_DAYS = c.SETTING.REPORT_DAYS,
                REPORT_END_DAY = c.SETTING.REPORT_END_DAY,
                REPORT_START_DAY = c.SETTING.REPORT_START_DAY,
                WILL_COVER_ORDER_DATE = c.SETTING.WILL_COVER_ORDER_DATE
            };
            //REPORT_START_DAY.ItemsSource = c.SETTING;
        }

        private void CUSTOMERS_COMBO_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            if (MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null)
            {
                var c = MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER;
                if (c.SETTING == null) c.SetSETTING(new Customer.Setting
                {
                    REPORT_START_DAY = null,
                    REPORT_END_DAY = null,
                    CUSTOMER_ID = int.Parse(((ICustomer)c).ACCOUNTNUM)
                });
                FOCUSED_CUSTOMER = c;
                
                CUSTOMERS_COMBO.SelectedItem = c;
                
                ORIGINAL = new Customer.Setting
                {
                    ADD_URL = c.SETTING.ADD_URL,
                    CUSTOMER_ID =c.SETTING.CUSTOMER_ID,
                    STORE_COUNT = c.SETTING.STORE_COUNT,
                    COMMENT = c.SETTING.COMMENT,
                    REPORT_DAYS = c.SETTING.REPORT_DAYS,
                    REPORT_END_DAY = c.SETTING.REPORT_END_DAY,
                    REPORT_START_DAY = c.SETTING.REPORT_START_DAY,
                    WILL_COVER_ORDER_DATE = c.SETTING.WILL_COVER_ORDER_DATE
                };
            }            
        }



        private void ADD_URL_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogManger.InsertEvent();
            if (ORIGINAL != null && (sender as TextBox).Text != ORIGINAL.ADD_URL)
                BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }

        private void STORE_COUNT_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogManger.InsertEvent();
            if (ORIGINAL != null && (sender as TextBox).Text != ORIGINAL.STORE_COUNT)
                BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }

        private void COMMENT_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtBx = sender as TextBox;
            if (txtBx.Text.Length == 240)
                MessageBox.Show("You have reached the max number of character count allowed for this section. Please limit your input to 240 character count including spaces.");

            LogManger.InsertEvent();
            if (ORIGINAL != null && txtBx.Text != ORIGINAL.COMMENT)
                BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }

        private void _SaveIfChanged(ICustomer cust)
        {
            LogManger.InsertStep();
            if (cust != null && cust.SETTING.HAS_CHANGED)
            {
                
                cust.SETTING.Save();
                MainWindow.myMainWindowViewModel.RefreshStatusBarAddUri();
                MainWindow.myTopMenuViewModel.RefreshSTORE_COUNT();
                
            }
        }

        private void CUSTOMERS_COMBO_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            //HANDLE PREVIOUS ITEM
            var cust = FOCUSED_CUSTOMER;
            if (cust != null && cust.SETTING != null)
                _SaveIfChanged(cust);
            ORIGINAL = null;
        }

        private void REPORT_START_DAY_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            REPORT_START_DAY.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
        }

        private void REPORT_START_DAY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogManger.InsertEvent();
            if (REPORT_END_DAY.SelectedItem != null && REPORT_START_DAY.SelectedItem != null)
                if ((DayOfWeek)(REPORT_START_DAY.SelectedItem) >= ((DayOfWeek)(REPORT_END_DAY.SelectedItem)))
                {
                    MessageBox.Show("Start day has to be before end day.");
                    REPORT_START_DAY.SelectedItem = e.RemovedItems[0];
                    return;
                }
                else
                {
                    REPORTING_NUMBER_OF_DAYS.Text = 
                        ((DayOfWeek)(REPORT_END_DAY.SelectedItem) - (DayOfWeek)(REPORT_START_DAY.SelectedItem) + 1).ToString();
                }
            if (ORIGINAL != null)
                if ((sender as ComboBox).SelectedValue.ToString().ToUpper() != ORIGINAL.REPORT_START_DayOfWeek.ToString().ToUpper())
                    BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }
        private void REPORT_END_DAY_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogManger.InsertEvent();
            if (REPORT_END_DAY.SelectedItem != null && REPORT_START_DAY.SelectedItem != null)
                if ((DayOfWeek)(REPORT_START_DAY.SelectedItem) >= ((DayOfWeek)(REPORT_END_DAY.SelectedItem)))
                {
                    MessageBox.Show("Start day has to be before end day.");
                    REPORT_END_DAY.SelectedItem = e.RemovedItems[0];
                    return;
                }
                else
                {
                    REPORTING_NUMBER_OF_DAYS.Text = ((DayOfWeek)(REPORT_END_DAY.SelectedItem) - (DayOfWeek)(REPORT_START_DAY.SelectedItem) + 1).ToString();
                }

            if (ORIGINAL != null && 
                (sender as ComboBox).SelectedValue.ToString().ToUpper() != ORIGINAL.REPORT_END_DayOfWeek.ToString().ToUpper())
                    BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }

        private void REPORT_END_DAY_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            REPORT_END_DAY.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
        }

        private void REPORTING_NUMBER_OF_DAYS_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogManger.InsertEvent();
            if (!REPORTING_NUMBER_OF_DAYS.Text.IsInt())
            {
                MessageBox.Show("Enter a number for the reporting days.");
                e.Handled = true;
                return;
            }
            if (ORIGINAL != null && (sender as TextBox).Text != ORIGINAL.COMMENT)
                    BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);            
        }

        private void WILL_COVER_ORDER_DATE_CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            if (ORIGINAL != null && (sender as CheckBox).IsChecked != null)
                BeginEdit((ICustomer)CUSTOMERS_COMBO.SelectedItem);
        }

        private void BeginEdit(ICustomer c)
        {
            if (c != null && c.SETTING != null)
                c.SETTING.BeginEdit();
        }


       
    }

    public class Add_URLConverter:IValueConverter
    {
        #region IValueConverter Members


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var cust = (ICustomer)value;
                if (cust != null && cust.SETTING != null)
                    return cust.SETTING.ADD_URL;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
