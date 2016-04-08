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
using System.Windows.Shapes;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.Views
{
    /// <summary>
    /// Interaction logic for DailyQAWeeklyQs.xaml
    /// </summary>
    public partial class DailyQAWeeklyQS : Window
    {
        ITab TAB { get; set; }
        public DailyQAWeeklyQS(ITab tab)
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
            if (TAB.myGraphViewModel.Is_DAILY_QA_AND_WEEKLY_QS_CONTENT_Null())
                TAB.myGraphViewModel.DAILY_QA_AND_WEEKLY_QS_CONTENT = new UserControls.DailyQAWeeklyQS(tab);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.myTopMenuViewModel.Is_DAILYQA_WEEKLYQS_Enabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.myTopMenuViewModel.Is_DAILYQA_WEEKLYQS_Enabled = true;
        }
    }
}
