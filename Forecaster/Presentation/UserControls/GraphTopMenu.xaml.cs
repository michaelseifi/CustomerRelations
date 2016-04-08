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
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for GraphTopMenu.xaml
    /// </summary>
    public partial class GraphTopMenu : UserControl
    {
        #region event handlers
        public event EventHandler ExitClick;
        public event EventHandler MaximizeScreenClick;
        public event EventHandler NormalizeScreenClick;
        public event EventHandler FullScreenClick;
        public event EventHandler OnMouseDoubleClick;
        #endregion

        public Window OWNER { get; set; }
        public ITab TAB { get; set; }
        public GraphTopMenu(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
        }

        private void staticMinimize_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            if (OWNER != null)
                OWNER.WindowState = WindowState.Minimized;
        }

        private void staticClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            if (OWNER != null)
                OWNER.Close();
        }

        private void NORMAL_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            if (NormalizeScreenClick != null)
                NormalizeScreenClick(this, EventArgs.Empty);
        }

        private void MAXIMIZE_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            if (MaximizeScreenClick != null)
                MaximizeScreenClick(this, EventArgs.Empty);
        }

        private void MINIMIZE_BTN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            if (OWNER != null)
                OWNER.WindowState = WindowState.Minimized;
        }

        private void DAILY_QA_WEEKLY_QS_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            var win = new Views.DailyQAWeeklyQS(TAB);
            win.Show();
        }

        private void WEEKLY_QS_ORDERQUANTITY_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LogManger.InsertEvent();
            var win = new Views.WeeklyQSOrderQuantity(TAB);
            win.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            MouseDoubleClick += GraphTopMenu_MouseDoubleClick;
        }

        void GraphTopMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LogManger.InsertEvent();
            if (OnMouseDoubleClick != null)
                OnMouseDoubleClick(this, EventArgs.Empty);
        }
    }
}
