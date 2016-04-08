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
using DevExpress.Xpf.Charts;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Exceptions;

namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for Distribution.xaml
    /// </summary>
    public partial class Distribution : UserControl
    {
        ITab TAB { get; set; }
        public Distribution(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            DataContext = TAB.myGraphViewModel;
            //if (TAB.myGraphViewModel != null)
            //    TAB.myGraphViewModel.DISTRIBUTION_USERCONTROLS.Add(this);
            this.Loaded += (s, e) =>
            {
                Window win = Tools.GetParentWindow(this);
                win.Closed += (sender, ev) =>
                {
                    if (win.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.Graphs)))
                        MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled = true;
                    //else if (win.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.WeeklyQSOrderQuantity)))
                    //    MainWindow.myTopMenuViewModel.Is_WEEKLYQS_ORDERQUANTITY_Enabled = true;
                };
                Update();
            };

            //this.Unloaded += (s, e) =>
            //{
            //    if (TAB.myGraphViewModel != null)
            //        TAB.myGraphViewModel.DISTRIBUTION_USERCONTROLS.Remove(this);
            //};
            
        }
        public void Update()
        {
            LogManger.InsertStep();
            try
            {
                //GRAPH WEEKLY QS
                var QSCount = 0;
                var MaxAxisYRange = 0.0;
                if (TAB.myGraphViewModel != null && TAB.myGraphViewModel.QS_ZSCORES != null)
                {
                    var dataColl = TAB.myGraphViewModel.QS_ZSCORES;

                    series.Points.Clear();
                    foreach (var item in dataColl)
                    {
                        series.Points.Add(new SeriesPoint(item.Key, item.Value));
                        QSCount++;
                    }
                    MaxAxisYRange = dataColl.Max(x => x.Value);
                }


                //var axisX = DIAGRAM.AxisX;
                var axisY = DIAGRAM.AxisY;
                axisY.Range.MaxValue = MaxAxisYRange * 1.1;
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                TAB.myGraphViewModel.DISTRIBUTION_CONTENT = new TextBlock() { Text = "Error loading graph." };
            }
        }
    }
}
