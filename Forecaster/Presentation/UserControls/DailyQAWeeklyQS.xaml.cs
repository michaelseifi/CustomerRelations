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
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Helpers;
using DevExpress.Xpf.Charts;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Presentation.UserControls
{
     
    /// <summary>
    /// Interaction logic for DailyQAWeeklyQS.xaml
    /// </summary>
    public partial class DailyQAWeeklyQS : UserControl
    {
        ITab TAB { get; set; }
        public DailyQAWeeklyQS(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
            //if (MainWindow.myGraphViewModel != null)
            //    MainWindow.myGraphViewModel.DAILY_QA_WEEKLY_QS_USERCONTROLS.Add(this);
            this.Loaded += (s, e) =>
            {
                Update(TAB.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS);
            };

        }

        public void Update(int numberOfDays)
        {
            LogManger.InsertStep();
            //MainWindow.SetBusyState(true);
            try
            {
                //GRAPH WEEKLY QS
                var QSCount = 0;
                DateTime? QSDate = null;
                var MaxAxisYRange = 0;
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
                    seriesQS.Points.Clear();
                   
                    foreach (IDailyData item in data)
                    {
                        foreach (var dailyColl in item.DAILY_DATA)
                        {
                            seriesQS.Points.Add(new SeriesPoint(dailyColl.REPORT_AS_OF_DATE, item.QS));
                            
                        }
                        DIAGRAM.AxisX.CustomLabels.Add(new CustomAxisLabel(item.REPORT_AS_OF_DATE, item.REPORT_AS_OF_DATE.ToShortDateString()));
                        QSCount++;
                    }
                    QSDate = data.SelectMany(x => x.DAILY_DATA).Min(x => x.REPORT_AS_OF_DATE);
                    MaxAxisYRange = data.Max(x => x.QS);
                    minAxisYRange = data.Min(x => x.QS);
                    minDate = data.Min(x => x.REPORT_AS_OF_DATE);
                    maxDate = data.Max(x => x.REPORT_AS_OF_DATE);
                    
                }

                if (TAB.myDailyData != null)
                {
                    seriesQA.Points.Clear();
                    var dataDailyColl = TAB.myDailyData;
                    if (QSDate != null) //IF WE HAVE THE MIN DATE FROM ABOVE, JUST FETCH DATA FROM THAT DATE ON
                    {
                        var dataDaily = dataDailyColl.Where(x => x.REPORT_AS_OF_DATE <= dataDailyColl.LASTDAYWITHREALDATA && x.REPORT_AS_OF_DATE >= QSDate);
                        foreach (var item in dataDaily)
                            seriesQA.Points.Add(new SeriesPoint(item.REPORT_AS_OF_DATE, item.QA));
                        MaxAxisYRange = Math.Max(MaxAxisYRange, dataDaily.Max(x => x.QA));
                        minAxisYRange = Math.Min(minAxisYRange, dataDaily.Min(x => x.QA));
                        minDate = new DateTime(Math.Min(((DateTime)minDate).Ticks, dataDaily.Min(x => x.REPORT_AS_OF_DATE).Ticks));
                        maxDate = new DateTime(Math.Max(((DateTime)maxDate).Ticks, dataDaily.Max(x => x.REPORT_AS_OF_DATE).Ticks));
                    }
                    else // ELSE TAKE SO MANY WEEKS BACK
                    {
                        var num = (numberOfDays / 7) * dataDailyColl.NUMBEROFDAYSINWEEK + dataDailyColl.NUMBEROFDAYINLASTWEEK;
                        var dataDaily = dataDailyColl.Where(x => x.REPORT_AS_OF_DATE <= dataDailyColl.LASTDAYWITHREALDATA).Take(num);
                        foreach (var item in dataDaily)
                            seriesQA.Points.Add(new SeriesPoint(item.REPORT_AS_OF_DATE, item.QA));
                        MaxAxisYRange = Math.Max(MaxAxisYRange, dataDaily.Max(x => x.QA));
                        minAxisYRange = Math.Min(minAxisYRange, dataDaily.Min(x => x.QA));
                        minDate = new DateTime(Math.Min(((DateTime)minDate).Ticks, dataDaily.Min(x => x.REPORT_AS_OF_DATE).Ticks));
                        maxDate = new DateTime(Math.Max(((DateTime)maxDate).Ticks, dataDaily.Max(x => x.REPORT_AS_OF_DATE).Ticks));
                    }
                }
                for (int i = 0; i < MaxAxisYRange; i += 5)
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
                            var sPoint = new SeriesPoint(date, MaxAxisYRange * 1.1);
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

                //MAKE SURE WE HAVE A LITTLE SPACE ON TOP - BY 10%
                //var axisX = DIAGRAM.AxisX;
                var axisY = DIAGRAM.AxisY;
                axisY.Range.MaxValue = MaxAxisYRange * 1.1;
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                //throw new GraphException("Unable to load Daily QA Weekly QS graph.");
                TAB.myGraphViewModel.Set_DAILY_QA_AND_WEEKLY_QS_CONTENT(ex.Message + "\rContact system administrator.");
            }
            
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //MainWindow.SetBusyState(true);
        }

        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            LogManger.InsertEvent();
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
            LogManger.InsertEvent();
            seriesQS.MarkerVisible = !seriesQS.MarkerVisible;
            seriesQS.LabelsVisibility = !seriesQS.LabelsVisibility;
        }

        private void QS_MARKERS_CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            seriesQS.MarkerVisible = !seriesQS.MarkerVisible;
            seriesQS.LabelsVisibility = !seriesQS.LabelsVisibility;
        }

        private void QA_MARKER_CHKBX_Unchecked(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            seriesQA.MarkerVisible = !seriesQA.MarkerVisible;
            seriesQA.LabelsVisibility = !seriesQA.LabelsVisibility;
        }

        private void QA_MARKER_CHKBX_Checked(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            seriesQA.MarkerVisible = !seriesQA.MarkerVisible;
            seriesQA.LabelsVisibility = !seriesQA.LabelsVisibility;
        }

        private string NUMBER_OF_WEEKS;
        private void NUMBER_OF_WEEK_BTN_Click(object sender, RoutedEventArgs e)
        {
            LogManger.InsertEvent();
            int i;
            if (NUMBER_OF_WEEKS.Length == 0 || !int.TryParse(NUMBER_OF_WEEKS, out i) || i > 78 || i < 1)                
            {
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
                TAB.myGraphViewModel.NotifyPropertyChanged("DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS");
                return;
            }

            MainWindow.SetBusyState(true);
            TAB.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS = i;
        }

        private void NUMBER_OF_WEEK_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogManger.InsertEvent();
            NUMBER_OF_WEEKS = (sender as TextBox).Text;
            //var value = (sender as TextBox).Text;
            
            //int i;
            //if (int.TryParse(value, out i) && i < 78 && i > 0)
            //    NUMBER_OF_WEEKS = i;
            //else if (value.Length == 0) { }
            //else
            //{
            //    NUMBER_OF_WEEKS = MainWindow.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS;
            //    Log.RaiseErrorMessage(new Message { MESSAGE = "Enter a valid number between 1 and 78." });
            //    (sender as TextBox).Text = MainWindow.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS.ToString();
            //}
        }



        private void NUMBER_OF_WEEK_TXB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            LogManger.InsertEvent();
            //var focused = Keyboard.FocusedElement;
            //if (focused != null && focused.GetType().Equals(typeof(Button)))
            //{
            //    var btn = (Button)focused;
            //    if (btn.Content.ToString() == "GO")
            //    {
            //        return;
            //    }
            //}
            //(sender as TextBox).Text = MainWindow.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS.ToString();
        }
    }
}
