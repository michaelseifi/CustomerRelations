using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.Views
{
    /// <summary>
    /// Interaction logic for WeeklyQSOrderQuantity.xaml
    /// </summary>
    public partial class WeeklyQSOrderQuantity : Window
    {
        ITab TAB { get; set; }
        public WeeklyQSOrderQuantity(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Height = screen.Bounds.Height - 90;
            Width = screen.Bounds.Width - 90;
            Top = 45;
            Left = 45;
            DataContext = TAB.myGraphViewModel;
            //if (TAB.myGraphViewModel.Is_WEEKLY_QS_ORDER_QUANTITY_CONTENT_Null())
            TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_CONTENT = new daisybrand.forecaster.Presentation.UserControls.WeeklyQSOrderQuantity(tab);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.myTopMenuViewModel.Is_WEEKLYQS_ORDERQUANTITY_Enabled = false;
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.myTopMenuViewModel.Is_WEEKLYQS_ORDERQUANTITY_Enabled = true;
        }
    }
}
