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
using System.Windows.Controls.Primitives;

using System.Xml.Linq;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Exceptions;
using DevExpress.Xpf.Charts;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for WeeklyQSOrderQuantity.xaml
    /// </summary>
    public partial class WeeklyQSOrderQuantity : UserControl
    {
        ITab TAB { get; set; }
        public WeeklyQSOrderQuantity(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            //if (MainWindow.myGraphViewModel != null)
            //    MainWindow.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_USERCONTROLS.Add(this);                            

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Window win = Tools.GetParentWindow(this);
            //win.Closed += (sender, ev) =>
            //{
            //    if (win.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.Graphs)))
            //        MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled = true;
            //    else if (win.GetType().Equals(typeof(daisybrand.forecaster.Presentation.Views.WeeklyQSOrderQuantity)))
            //        MainWindow.myTopMenuViewModel.Is_WEEKLYQS_ORDERQUANTITY_Enabled = true;
            //};
            Update(TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void Update(int numberOfDays)
        {
            try
            {
                //MainWindow.SetBusyState(true);
                //GRAPH WEEKLY QS AND WEEKLY QA(OUTS)
                var QSCount = 0;
                var QSDate = new DateTime();
                var maxAxisYRange = 0;
                var minAxisYRange = 0.0;
                DateTime? minDate = null;
                DateTime? maxDate = null;
                if (TAB.myWeeklyData != null)
                {
                    var dataColl = TAB.myWeeklyData;
                    var data = dataColl.Where(x => x.REPORT_AS_OF_DATE <= dataColl.LASTDAYWITHREALDATA).Take(numberOfDays / 7);
                    //var average = MainWindow.myFirst120Data.Average(x => x.QS);
                    //var sum = MainWindow.myFirst120Data.Sum(x => Math.Pow(x.QS - average, 2));
                    //MainWindow.myGraphViewModel.QS_STANDARD_DEVIATION = Math.Sqrt((sum) / (MainWindow.myFirst120Data.Count() - 1));
                    seriesWeeklyQS.Points.Clear();
                    seriesWeeklyQO.Points.Clear();
                    foreach (IDailyData item in data)
                    {
                        seriesWeeklyQS.Points.Add(new SeriesPoint(item.REPORT_AS_OF_DATE, item.QS));
                        seriesWeeklyQO.Points.Add(new SeriesPoint(item.REPORT_AS_OF_DATE, item.QO));
                        DIAGRAM.AxisX.CustomLabels.Add(new CustomAxisLabel(item.REPORT_AS_OF_DATE, item.REPORT_AS_OF_DATE.ToShortDateString()));
                        QSCount++;
                    }
                    QSDate = data.Min(x => x.REPORT_AS_OF_DATE);
                    maxAxisYRange = Math.Max(data.Max(x => x.QS), data.Max(x => x.QO));
                    minAxisYRange = Math.Min(data.Min(x => x.QS), data.Min(x => x.QO));

                    minDate = data.Min(x => x.REPORT_AS_OF_DATE);
                    maxDate = data.Max(x => x.REPORT_AS_OF_DATE);

                }
                //GRAPH WEEKLY ORDER QUANTITY
                if (TAB.myGraphViewModel != null && TAB.myGraphViewModel.ORDERCOLLECTION != null)
                {
                    var orders = TAB.myGraphViewModel.ORDERCOLLECTION.Where(x => x.REQUESTED_SHIP_DATE_KEY >= QSDate);  //.Take(QSCount > 0 ? QSCount : 120);
                    if (orders.Count() == 0)
                        goto Finish;
                    IEnumerable<IGrouping<DateTime, IOrderCase>> OrderGroups = orders.GroupBy(x => x.FIRST_DAY_OF_WEEK);

                    seriesOrderQuantity.Points.Clear();
                    var maxOrderCase = 0.0;
                    var minOrderCase = 0.0;
                    DateTime? lastDateInserted = null;
                    foreach (IGrouping<DateTime, IOrderCase> group in OrderGroups)
                    {
                        if (lastDateInserted != null)
                        {
                            var missingDates = _GetMissingOrderDate((DateTime)lastDateInserted, group.Key);
                            if(missingDates != null)
                                foreach(var d in missingDates)
                                    seriesOrderQuantity.Points.Add(new SeriesPoint(d, 0));                            
                        }
                        var sum = group.Sum(x => x.ORDER_CASE);
                        seriesOrderQuantity.Points.Add(new SeriesPoint(group.Key, sum));
                        maxOrderCase = Math.Max(maxOrderCase, sum);                
                        lastDateInserted = group.Key;
                    }
                    maxAxisYRange = Convert.ToInt32(Math.Max(maxAxisYRange, maxOrderCase));
                    minOrderCase = Convert.ToInt32(Math.Min(minAxisYRange, minOrderCase));
                    minDate = new DateTime(Math.Min(((DateTime)minDate).Ticks, orders.Min(x => x.REQUESTED_DELIVERY_DATE_KEY).Ticks));
                    maxDate = new DateTime(Math.Max(((DateTime)maxDate).Ticks, orders.Max(x => x.REQUESTED_DELIVERY_DATE_KEY).Ticks));
                }


                for (int i = 0; i < maxAxisYRange; i += 5)
                {
                    DIAGRAM.AxisY.CustomLabels.Add(new CustomAxisLabel(minAxisYRange + i, (minAxisYRange + i).ToString()));
                }
                //GRAPH PROMOTIONS

                //minDate = new DateTime(Math.Min())
                //maxDate = coll.Max(x => x.Key);
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
                            else if (perf.PERFORMANCE_TYPEID == 2 && !adDates.Any(x => x == date))
                            {
                                seriesPromotionsTPR.Points.Add(sPoint);
                                tprDates.Add(date);
                            }
                        }
                    }

                }
            Finish:
                //var axisX = DIAGRAM.AxisX;
                var axisY = DIAGRAM.AxisY;
                axisY.Range.MaxValueInternal = maxAxisYRange * 1.1;
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                TAB.myGraphViewModel.Set_WEEKLY_QS_ORDER_QUANTITY_CONTENT(ex.Message + "\rContact system administrator.");
                //throw new GraphException("Unable to load Weekly QS, Order, and QA graph.");
            }
            
        }

        private List<DateTime> _GetMissingOrderDate(DateTime prevDate, DateTime key)
        {
            List<DateTime> list = null;
            var dif = (prevDate - key).TotalDays;
            for (int i = 7; i <dif ; i+=7)
            {
                if (list == null) list = new List<DateTime>();
                list.Add(prevDate.AddDays(-i));
            }
            return list;
        }

        #region events
        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            ChartHitInfo hitInfo = chartControl1.CalcHitInfo(e.GetPosition(chartControl1));

            if (hitInfo != null && hitInfo.SeriesPoint != null)
            {
                SeriesPoint point = hitInfo.SeriesPoint;

                XElement xmlTxt = point.ToolTipHint as XElement;
                if (xmlTxt == null) return;
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

        private void QS_MARKERS_CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            seriesWeeklyQS.MarkerVisible = !seriesWeeklyQS.MarkerVisible;
            seriesWeeklyQS.LabelsVisibility = !seriesWeeklyQS.LabelsVisibility;
        }

        private void QS_MARKERS_CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {

            seriesWeeklyQS.MarkerVisible = !seriesWeeklyQS.MarkerVisible;
            seriesWeeklyQS.LabelsVisibility = !seriesWeeklyQS.LabelsVisibility;

        }

        private void ORDER_MARKER_CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {
            seriesOrderQuantity.MarkerVisible = !seriesOrderQuantity.MarkerVisible;
            seriesOrderQuantity.LabelsVisibility = !seriesOrderQuantity.LabelsVisibility;
        }

        private void ORDER_MARKER_CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            seriesOrderQuantity.MarkerVisible = !seriesOrderQuantity.MarkerVisible;
            seriesOrderQuantity.LabelsVisibility = !seriesOrderQuantity.LabelsVisibility;
        }

        private void QO_MARKER_CHKBOX_Checked(object sender, RoutedEventArgs e)
        {
            seriesWeeklyQO.MarkerVisible = !seriesWeeklyQO.MarkerVisible;
            seriesWeeklyQO.LabelsVisibility = !seriesWeeklyQO.LabelsVisibility;
        }

        private void QO_MARKER_CHKBOX_Unchecked(object sender, RoutedEventArgs e)
        {
            seriesWeeklyQO.MarkerVisible = !seriesWeeklyQO.MarkerVisible;
            seriesWeeklyQO.LabelsVisibility = !seriesWeeklyQO.LabelsVisibility;
        }
        #endregion


        private string NUMBER_OF_WEEKS;

        private void NUMBER_OF_WEEKS_BTN_Click(object sender, RoutedEventArgs e)
        {
            var value = NUMBER_OF_WEEKS;
            int i;
            if (value.Length == 0 || !int.TryParse(value, out i) || i > 78 || i < 1)
            {
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
                TAB.myGraphViewModel.NotifyPropertyChanged("WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS");
                return;
            }
            MainWindow.SetBusyState(true);
            TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS = i;
        }

        private void NUMBER_OF_WEEKS_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {

            NUMBER_OF_WEEKS = (sender as TextBox).Text;
            //int i;
            //if (int.TryParse(value, out i) && i < 78 && i > 0)
            //    NUMBER_OF_WEEKS = i;
            //else if (value.Length == 0) { }
            //else
            //{
            //    NUMBER_OF_WEEKS = MainWindow.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS;
            //    Log.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
            //    (sender as TextBox).Text = MainWindow.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS.ToString();
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
            //(sender as TextBox).Text = MainWindow.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS.ToString();
        }


        
        
    }
}
