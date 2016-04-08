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
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : UserControl
    {
        public Tabs()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.TABS != null)
                TABS.ItemsSource = MainWindow.myMainWindowViewModel.TABS;
        }

        private void TABS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!MainWindow.myMainWindowViewModel.TABS.IsLoading) MainWindow.SetBusyState(true);
            MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB = (Tab)TABS.SelectedItem;
            
        }

        private void TABS_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void headerText_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void RESET_FORECAST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SKU_ITEM_NUM_Loaded(object sender, RoutedEventArgs e)
        {
            SKU_ITEM_NUM.Content = new UserControls.SkuItemNumber();
        }
    }
}
