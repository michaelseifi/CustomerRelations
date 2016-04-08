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

namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Icons.xaml
    /// </summary>
    public partial class Icons : UserControl
    {
        public Icons()
        {
            InitializeComponent();
            this.DataContext = forecaster.MainWindow.myMainWindowViewModel;
        }

        private void CUSTOMER_HAS_COMMENT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left) { }
            if (MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.SETTING != null)
                MessageBox.Show(MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.SETTING.COMMENT, MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.ACCOUNT_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            else if (MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null)
                MessageBox.Show(MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.COMMENT, MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.ACCOUNT_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
