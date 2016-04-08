using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Helpers = daisybrand.forecaster.Controlers.Helpers;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for QuantityGrid.xaml
    /// </summary>
    public partial class DailyDataGrid : UserControl
    {
        private IDailyData selectedQ = null;
        private List<IDailyData> selectedQs = null;

        public CustomerSearch customerSearch { get; set; }
        public DailyDataGrid()
        {
            
            this.DataContext = MainWindow.myDataPagingGrid;            
            InitializeComponent();
            GridControl.AllowInfiniteGridSize = true;
            
            
            //grdInfill.GroupBy(grdInfill.Columns["GlassType"], ColumnSortOrder.Ascending);
            //grdInfill.GroupBy(grdInfill.Columns["GlassDescription"], ColumnSortOrder.Ascending);
            //grdInfill.AutoExpandAllGroups = true;

        }

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            grid.GroupBy(grid.Columns["WEEK_NUMBER"], DevExpress.Data.ColumnSortOrder.Descending);
            grid.Columns["REPORT_AS_OF_DATE"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            grid.ExpandGroupRow(-9); //EXPAND THE LAST WEEK WITH LIVE DATA
            if(customerSearch != null)
                customerSearch.RefreshDataContext += customerSearch_RefreshDataContext;

            tableView1.SelectRow(0);
        }

        void customerSearch_RefreshDataContext(object sender, EventArgs e)
        {
            this.DataContext = MainWindow.myDataPagingGrid;
        }

        /// <summary>
        /// PART OF GRID CONTROL DATACONTEXT
        /// EXPORTS THE GRID TO A DOCUMENT VIEWER
        /// </summary>
        private void EXPORT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void grid_CustomGroupDisplayText(object sender, CustomGroupDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "WEEK_NUMBER") return;
            int groupLevel = grid.GetRowLevelByRowHandle(e.RowHandle);
            if (groupLevel != e.Column.GroupIndex) return;
            IDailyData q = (IDailyData)e.Row;
            DataCollection qs = (DataCollection)grid.ItemsSource;            
            var list = qs.Where(x => x.WEEK_ID == q.WEEK_ID);
            e.DisplayText = String.Format("{0} - Forecast: {7} - QS: {1} - QW: {2} - QO: {3} - QA: {4} - LYQS: {5} - LYQW: {6}",
                e.Value,
                list.Select(x => x.QS).Sum(),
                list.Select(x => x.QW).Sum(),
                list.Select(x => x.QO).Sum(),
                list.Select(x => x.QA).Sum(),
                list.Select(x => x.LYQS).Sum(),
                list.Select(x => x.LYQW).Sum(),
                list.Select(x => x.FORECASTS).Select(x => x.LASTVALUE).FirstOrDefault().ToString() ?? string.Empty);
        }

        private void tableView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

        }

        private void tableView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            IDailyData q = (IDailyData)e.Row;
            switch (e.Column.FieldName)
            {
                
            }            
        }

        private void FORECAST_Validate(object sender, GridCellValidationEventArgs e)
        {
            int value;
            if (!int.TryParse(e.Value.ToString(), out value))
            {
                e.ErrorContent = "Please enter a whole number";
                e.SetError(e.ErrorContent, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            }
        }



        private void grid_StartGrouping(object sender, RoutedEventArgs e)
        {            
            
        }

        private void grid_StartSorting(object sender, RoutedEventArgs e)
        {

        }       

        private void tableView1_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //IF COMMENT FOR THE PREVIOUS SELECTION HAS CHANGED, SAVE IT
  
            if (selectedQ != null)
            {
                bool hasCommentChanged = COMMENT_TXB.Text != selectedQ.COMMENT.VALUE;
                bool hasForecastChanged = FORECASE_TXB.Text != selectedQ.FORECASTS.LASTVALUE.ToString();
                if (hasCommentChanged || hasForecastChanged)
                {
                    IDailyData q = MainWindow.myDataPagingGrid.DATACOLLECTION.SelectMany(x => x).Where(x => x.DOCUMENTOBJECTID == selectedQ.DOCUMENTOBJECTID).FirstOrDefault();
                    if (hasCommentChanged)
                    {

                        q.COMMENT.VALUE = COMMENT_TXB.Text;
                        Comment.UpdateOrInsert(q);
                    }
                    if (hasForecastChanged)
                        try
                        {
                            Forecast.Insert(MainWindow.myDailyData.Where(x => x.WEEK_ID == q.WEEK_ID), q.FORECASTS, int.Parse(FORECASE_TXB.Text));
                        }
                        catch { }
                }
            }
            TableView table = (TableView)sender;
            SelectedRowsCollection rows = table.SelectedRows;
            if (rows.Count > 0)
            {
                selectedQs = new List<IDailyData>();
                foreach (var row in rows)
                    selectedQs.Add((IDailyData)row);                
            }
            else
                selectedQs = null;
            
            if (rows.Count == 1)
            {
                selectedQ = (IDailyData)rows[0];
                COMMENT_TXB.Text = selectedQ.COMMENT.VALUE;
                FORECASE_TXB.Text = selectedQ.FORECASTS.LASTVALUE.ToString();
                FORECAST_LIST_TXB.Text = selectedQ.FORECASTS.TOOLTIP;
                PERFOMANCES_USRCTRL.PERFORMANCE_GRID.ItemsSource = selectedQ.PERFORMANCES;
            }
            else
            {
                selectedQ = null;
                COMMENT_TXB.Text = string.Empty;
                FORECASE_TXB.DataContext = null;
                FORECAST_LIST_TXB.Text = string.Empty;
                PERFOMANCES_USRCTRL.PERFORMANCE_GRID.ItemsSource = new Performance();
            }
        }


        


    }

    public class DataPager : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;
        private List<DataCollection> _QUANTITIES;
        int pageIndex = 1;
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                if (value == pageIndex) return;
                pageIndex = value;
                NotifyPropertyChanged();
            }            
        }
        public List<DataCollection> DATACOLLECTION
        {
            get
            {
                return _QUANTITIES;
            }
            set
            {
                _QUANTITIES = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //public List<TestDataList> PagedCollection { get; private set; }
        //public DataPager()
        //{
        //    PagedCollection = new List<TestDataList>();
        //    PagedCollection.Add(TestDataList.Create(0));
        //    PagedCollection.Add(TestDataList.Create(3));
        //    PagedCollection.Add(TestDataList.Create(5));
        //    PagedCollection.Add(TestDataList.Create(7));
        //    PageIndex = 2;
        //}

    }
    //public class TestDataList : ObservableCollection<TestDataItem>
    //{
    //    public static TestDataList Create(int cc)
    //    {
    //        TestDataList res = new TestDataList();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            TestDataItem item = new TestDataItem();
    //            item.ID = i;
    //            item.Value = ((char)((int)'A' + cc)).ToString();
    //            res.Add(item);
    //        }
    //        for (int i = 0; i < 10; i++)
    //        {
    //            TestDataItem item = new TestDataItem();
    //            item.ID = i;
    //            item.Value = ((char)((int)'B' + cc)).ToString();
    //            res.Add(item);
    //        }
    //        return res;
    //    }
    //}
    //public class TestDataItem
    //{
    //    public int ID { get; set; }
    //    public string Value { get; set; }
    //}
}
