using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Charts;
using daisybrand.forecaster.Controls;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for WeeklyPOSCases.xaml
    /// </summary>
    public partial class WeeklyPOSCases : UserControl
    {
        ITab TAB { get; set; }
        Window PARENT { get; set; }
        public WeeklyPOSCases(ITab tab, Window win = null)
        {
            InitializeComponent();
            TAB = tab;
            PARENT = win;
            //if (TAB.myGraphViewModel != null)
            //    TAB.myGraphViewModel.WEEKLY_POS_CASES_USERCONTROLS.Add(this);

    
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if (PARENT == null)
            //    PARENT = Tools.GetParentWindow(this);
            //PARENT.Closed += (s, ev) =>
            //{
            //    try
            //    {
            //        if (PARENT.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.Graphs)))
            //            MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled = true;
            //        else if (PARENT.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.WeeklyPOSCases)))
            //            MainWindow.myTopMenuViewModel.Is_WEEKLY_POS_CASES_Enabled = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Insert1(ex, "WeeklyPosUsercontrol parent closing event", true);
            //    }
            //};
            Update(TAB.myGraphViewModel.WEEKLY_POS_NUMBER_OF_DAYS); 
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (TAB.myGraphViewModel != null)
            //    TAB.myGraphViewModel.WEEKLY_POS_CASES_USERCONTROLS.Remove(this);
        }

        public void Update(int numberOfDays)
        {
            try
            {                
                var maxAxisYRange = 0.0;
                var minAxisYRange = 0.0;
                DateTime? minDate = null;
                DateTime? maxDate = null;
                if (TAB.myGraphViewModel != null && TAB.myGraphViewModel.POSCOLLECTION != null)
                {
                    var coll = TAB.myGraphViewModel.POSCOLLECTION.GroupBy(x => x.FIRST_DAY_OF_WEEK).Take(numberOfDays / 7);
                    minDate = coll.Min(x => x.Key);
                    maxDate = coll.Max(x => x.Key);
                    seriesWeeklyPos.Points.Clear();
                    foreach (var group in coll)
                    {
                        seriesWeeklyPos.Points.Add(new SeriesPoint(group.Key, Convert.ToDouble(group.Sum(x => x.CASES))));
                        maxAxisYRange = Math.Max(maxAxisYRange, Convert.ToDouble(group.Sum(x => x.CASES)));
                        minAxisYRange = Math.Min(minAxisYRange, Convert.ToDouble(group.Sum(x => x.CASES)));
                        DIAGRAM.AxisX.CustomLabels.Add(new CustomAxisLabel(group.Key, group.Key.ToShortDateString()));
                    }
                    for (int i = 0; i < maxAxisYRange; i += 5)
                    {
                        DIAGRAM.AxisY.CustomLabels.Add(new CustomAxisLabel(minAxisYRange + i, (minAxisYRange + i).ToString()));
                    }

                    //var perfs = MainWindow.myGraphViewModel.DAILY_DATA_COLLECTION.PERFORMANCES;
                    var perfs = TAB.myWeeklyData.SelectMany(x => x.PERFORMANCES.Where(p => p.IS_INCLUDED == true));
                    seriesPromotionsTPR.Points.Clear();
                    seriesPromotionsAD.Points.Clear();
                    seriesPromotionsTPR.DisplayName = "TPR";
                    seriesPromotionsAD.DisplayName = "AD";
                    List<DateTime> adDates = new List<DateTime>();
                    List<DateTime> tprDates = new List<DateTime>();
                    foreach (var perf in perfs)
                    {
                        TimeSpan t = new TimeSpan(perf.END_DATE.Ticks - perf.START_DATE.Ticks);
                        var totalDays = t.TotalDays;
                        if (totalDays == 0) totalDays = 1; //if end date and start date are the same, display at least one day
                        for (int i = 0; i < totalDays; i++)
                        {
                            var date = perf.START_DATE.AddDays(i);
                            if (adDates.Any(x => x == date) || tprDates.Any(x => x == date)) continue;
                            if (date >= minDate && date <= maxDate.Value.AddDays(7)) //IF THE DAY IS AFTER THE START DAY AND UP 7 DAYS AFTER END DATE, ADD IT TO THE GRAPH
                            {
                                var sPoint = new SeriesPoint(date, maxAxisYRange * 1.1);
                                //XElement xmlTxt = new XElement("tip",
                                //    new XElement("promotion", perf.PROMOTION_NUMBER),
                                //    new XElement("type", perf.PERFORMANCE_TYPE),
                                //    new XElement("start", perf.START_DATE),
                                //    new XElement("end", perf.END_DATE),
                                //    new XElement("name", perf.AD_NAME));
                                //sPoint.ToolTipHint = xmlTxt;
                                sPoint.SetCurrentValue(RangeBarSeries2D.Value2Property, minAxisYRange);
                                if (perf.PERFORMANCE_TYPEID == 1)
                                {
                                    seriesPromotionsAD.Points.Add(sPoint);
                                    adDates.Add(date);
                                    seriesPromotionsTPR.Points.Remove(seriesPromotionsTPR.Points.Where(x => Convert.ToDateTime(x.ActualArgument) == date).FirstOrDefault());
                                }
                                else if (perf.PERFORMANCE_TYPEID == 2)
                                {
                                    seriesPromotionsTPR.Points.Add(sPoint);
                                    tprDates.Add(date);
                                }
                            }
                        }

                    }

                }
                //var axisX = DIAGRAM.AxisX;
                var axisY = DIAGRAM.AxisY;
                axisY.Range.MaxValueInternal = maxAxisYRange * 1.1;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to update POS weekly", true);
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "System was unable to update the Weekly POS data.\rPlease report this to Michael Seifi." });
            }
        }
        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindow.SetBusyState(true);
        }

        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            ChartHitInfo hitInfo = chartControl1.CalcHitInfo(e.GetPosition(chartControl1));

            if (hitInfo != null && hitInfo.SeriesPoint != null)
            {
                SeriesPoint point = hitInfo.SeriesPoint;

                XElement xmlTxt = point.ToolTipHint as XElement;

                tooltip_text.Text = string.Format("{0}\r{1} - {2} {3}\r{4}",
                    xmlTxt.Element("promotion").Value,
                    xmlTxt.Element("type").Value,
                    Convert.ToDateTime(xmlTxt.Element("start").Value).ToShortDateString(),
                    Convert.ToDateTime(xmlTxt.Element("end").Value).ToShortDateString(),
                    xmlTxt.Element("name").Value);
                tooltip1.Placement = PlacementMode.Center;

                tooltip1.IsOpen = true;
                Cursor = Cursors.Hand;
            }
            else
            {
                tooltip1.IsOpen = false;
                Cursor = Cursors.Arrow;
            }
        }

        private void chartControl1_MouseLeave(object sender, MouseEventArgs e)
        {
            tooltip1.IsOpen = false;
        }

        private void POS_MARKERS_CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            seriesWeeklyPos.MarkerVisible = !seriesWeeklyPos.MarkerVisible;
            seriesWeeklyPos.LabelsVisibility = !seriesWeeklyPos.LabelsVisibility;
        }

        private void POS_MARKERS_CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {
            seriesWeeklyPos.MarkerVisible = !seriesWeeklyPos.MarkerVisible;
            seriesWeeklyPos.LabelsVisibility = !seriesWeeklyPos.LabelsVisibility;
        }

        private string NUMBER_OF_WEEKS;

        private void NUMBER_OF_WEEKS_BTN_Click(object sender, RoutedEventArgs e)
        {
            var value = NUMBER_OF_WEEKS;
            int i;
            if (value.Length == 0 || !int.TryParse(value, out i) || i > 78 || i < 1)
            {
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
                TAB.myGraphViewModel.NotifyPropertyChanged("WEEKLY_POS_NUMBER_OF_WEEKS");
                return;
            }

            MainWindow.SetBusyState(true);
            TAB.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS = i;
            Update(TAB.myGraphViewModel.WEEKLY_POS_NUMBER_OF_DAYS);
        }

        private void NUMBER_OF_WEEKS_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            NUMBER_OF_WEEKS = (sender as TextBox).Text;
            //var value = (sender as TextBox).Text;
            //int i;
            //if (int.TryParse(value, out i) && i < 78 && i > 0)
            //    NUMBER_OF_WEEKS = i;
            //else if (value.Length == 0) { }
            //else
            //{
            //    NUMBER_OF_WEEKS = MainWindow.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS;
            //    Log.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
            //    (sender as TextBox).Text = MainWindow.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS.ToString();
            //}
        }



        private void NUMBER_OF_WEEKS_TXB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //var focused = Keyboard.FocusedElement;
            //if (focused != null && focused.GetType().Equals(typeof(Button)))
            //{
            //    var btn = (Button)focused;
            //    if (btn.Content.ToString() == "GO")
            //    {
            //        return;
            //    }
            //}
            //(sender as TextBox).Text = MainWindow.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS.ToString();
        }



 
    }
}
