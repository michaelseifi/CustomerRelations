using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.Views
{
    public partial class Graphs
    {
        
        private void _LoadParts()
        {
            MainWindow.SetBusyState(true);
            var top = new UserControls.GraphTopMenu(TAB) { OWNER = this };
            top.MaximizeScreenClick += top_MaximizeScreenClick;
            top.NormalizeScreenClick += top_NormalizeScreenClick;
            top.OnMouseDoubleClick += top_OnMouseDoubleClick;
            TOPMENU.Content = top;
            var graph = TAB.myGraphViewModel;
            //if (graph.Is_DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT_Null())
            graph.DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT = new UserControls.DailyQAWeeklyQS(TAB);
            //if (graph.Is_WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT_Null())
            graph.WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT = new UserControls.WeeklyQSOrderQuantity(TAB);
            graph.VOLATILITY_CONTENT = new UserControls.Volatility(TAB);
            graph.PERCENTAGE_CONTENT = new UserControls.Percentage(TAB);
            graph.PROMOTION_CONTENT = new UserControls.Promotions(TAB);
            
        }

        void top_OnMouseDoubleClick(object sender, EventArgs e)
        {
            var w = this.Width;
            var h = this.Height;
            var graph = TAB.myGraphViewModel;
            if (w > graph.ORIG_WIDTH || h > graph.ORIG_HEIGHT)
                _NormalizeScreen();
            else
                _MaximizeScreen();
        }

        void top_NormalizeScreenClick(object sender, EventArgs e)
        {
            this._NormalizeScreen();
        }

        void top_MaximizeScreenClick(object sender, EventArgs e)
        {
            this._MaximizeScreen();
        }

        private void _MaximizeScreen()
        {
            if (WindowState == System.Windows.WindowState.Maximized)
                WindowState = System.Windows.WindowState.Normal;
            //this.Topmost = false;  
            var workingArea = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;
            this.Left = workingArea.Left;
            this.Top = workingArea.Top;
            this.Width = workingArea.Width;
            this.Height = workingArea.Height;

        }

        private void _SetHeightAndWidth()
        {
            var workingArea = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;

            int height = MainWindow.mySettings.GRAPH.HEIGHT > 0 ? Convert.ToInt32(MainWindow.mySettings.GRAPH.HEIGHT) : workingArea.Height - 90;
            TAB.myGraphViewModel.MAX_HEIGHT = workingArea.Height;
            TAB.myGraphViewModel.HEIGHT = height;
            TAB.myGraphViewModel.ORIG_HEIGHT = height;

            int width = MainWindow.mySettings.GRAPH.WIDTH > 0 ? Convert.ToInt32(MainWindow.mySettings.GRAPH.WIDTH) : workingArea.Width - 90;
            TAB.myGraphViewModel.MAX_WIDTH = workingArea.Width;
            TAB.myGraphViewModel.WIDTH = width;
            TAB.myGraphViewModel.ORIG_WIDTH = width;

            int top =  Convert.ToInt32(MainWindow.mySettings.GRAPH.TOP);
            TAB.myGraphViewModel.TOP = top;
            

            int left = Convert.ToInt32(MainWindow.mySettings.GRAPH.LEFT);
            TAB.myGraphViewModel.LEFT = left;
        }

        private void _NormalizeScreen()
        {
            if (WindowState == System.Windows.WindowState.Maximized)
                WindowState = System.Windows.WindowState.Normal;
            var workingArea = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).WorkingArea;


            this.Width = MainWindow.myMainWindowViewModel.ORIG_WIDTH -
                (System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).Bounds.Width - workingArea.Width);
            this.Left = ((workingArea.Width - this.Width) / 2) + workingArea.Left;
            this.Height = MainWindow.myMainWindowViewModel.ORIG_HEIGHT -
                 (System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle).Bounds.Height - workingArea.Height);
            this.Top = ((workingArea.Height - this.Height) / 2);

        }

        private void _FullScreen()
        {
            //this.Topmost = true;
            WindowState = System.Windows.WindowState.Maximized;
        }

        private void _MinimizeScreen()
        {
            //this.Topmost = false;
            WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
