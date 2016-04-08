using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.Views
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : Window
    {
        private Point start;
        ITab TAB { get; set; }
        public Graphs(ITab tab)
        {

            if (tab.myGraphViewModel.WINDOW != null)
            {
                tab.myGraphViewModel.WINDOW.Close();
                tab.myGraphViewModel.WINDOW = null;
            }
            tab.myGraphViewModel.WINDOW = this;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //SizeToContent = SizeToContent.WidthAndHeight;
            TAB = tab;
            DataContext = TAB.myGraphViewModel;
            _SetHeightAndWidth();
            InitializeComponent();
            
            //System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            
            _LoadParts();            
        }

        #region events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled = false;
            TOPMENU.MouseDoubleClick += TOPMENU_MouseDoubleClick;
            TOPMENU.MouseDown += delegate { DragMove(); };
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
   
        }

        void TOPMENU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled = true;
            TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT = null;
            TAB.myGraphViewModel.DisposeWEEKLY_POS_CASES_CONTENT();
            TAB.myGraphViewModel.DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT = null;
            TAB.myGraphViewModel.VOLATILITY_CONTENT = null;
            TAB.myGraphViewModel.PERCENTAGE_CONTENT = null;
            TAB.myGraphViewModel.PROMOTION_CONTENT = null;
        }



        private void OPEN_DAILYQAANDWEEKLYQS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OPEN_WEEKLYQSANDORDERQUANTITY_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
                MainWindow.mySettings.GRAPH.HEIGHT = (e.NewSize.Height);
            if (e.WidthChanged)
                MainWindow.mySettings.GRAPH.WIDTH = (e.NewSize.Width);   
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            var win = sender as Window;
            MainWindow.mySettings.GRAPH.LEFT = win.Left;
            MainWindow.mySettings.GRAPH.TOP = win.Top; 
        }



        
        //private void OPEN_DAILYQAANDWEEKLYQS_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MainWindow.dailyQaWeeklyQsWindow == null)
        //    {
        //        MainWindow.dailyQaWeeklyQsWindow = new Presentation.Views.DailyQAWeeklyQS();
        //        MainWindow.dailyQaWeeklyQsWindow.ShowInTaskbar = true;
        //        MainWindow.dailyQaWeeklyQsWindow.Show();
        //    }
        //    else
        //    {
        //        MainWindow.dailyQaWeeklyQsWindow.Activate();
        //    }
        //}

        //private void OPEN_WEEKLYQSANDORDERQUANTITY_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MainWindow.weeklyQsOrderQuantityWindow == null)
        //    {
        //        MainWindow.weeklyQsOrderQuantityWindow = new Presentation.Views.WeeklyQSOrderQuantity();
        //        MainWindow.weeklyQsOrderQuantityWindow.ShowInTaskbar = true;
        //        MainWindow.weeklyQsOrderQuantityWindow.Show();
        //    }
        //    else
        //    {
        //        MainWindow.weeklyQsOrderQuantityWindow.Activate();
        //    }
        //}

        //private void EXPAND_POS_BTN_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void EXPAND_POS_TOGGLEBTN_Click(object sender, RoutedEventArgs e)
        //{
        //    POS_XPNDR.IsExpanded = !POS_XPNDR.IsExpanded;
        //    e.Handled = true;
        //}


        #endregion


    }
}
