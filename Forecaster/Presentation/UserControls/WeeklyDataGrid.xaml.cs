using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using daisybrand.forecaster.Exceptions;
using daisybrand.forecaster.Controls.SubControls;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Threading;

namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for WeeklyDataGrid.xaml
    /// </summary>
    public partial class WeeklyDataGrid : UserControl
    {
        ITab TAB { get; set; }
        List<IDailyData> _SelectedData;
        bool IsForecastDeleting;
        
        public WeeklyDataGrid(ITab tab)
        {
            this.DataContext = tab.myWeeklyData;
            InitializeComponent();
            this.TAB = tab;
            _GridControl_Quantity.AllowInfiniteGridSize = true;
            tableView.UseLightweightTemplates = UseLightweightTemplates.None;
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            grid.SelectItem(8);
            grid.Focus();
            //SET THE FOCUSE TO CURRENT WEEK ROW
            grid.View.FocusedRowHandle = 8;            
            //SET THE FOCUSE TO FORECAST COLUMN
            grid.CurrentColumn = grid.Columns["Data.FORECASTS"];
            var focusedItem = grid.CurrentItem as IDailyData;
            //BIND THE DAILY GRIDS TO FOCUSED ITEM IF NOT NULL
            if (focusedItem != null)
                TAB.myWeeklyData.FocusedDataCollection = new List<IDailyData>() { focusedItem };          
        }



        private void Number_Validate(object sender, GridCellValidationEventArgs e)
        {
            int i;
            try
            {
                if (e.Value == null) return;
                var value = e.Value.ToString().OperatorParse();
                if (e.Column.Header.ToString() == "FORECAST")
                {
                    var index = value.IndexOf(":");
                    if (index > -1)
                        value = value.Substring(index + 1);
                }
                var intValue = value.ToInt();
                if (e.Value != null && !int.TryParse(intValue.ToString(), out i))
                {
                    e.ErrorContent = "Please enter a valid formatted text.";
                    e.SetError(e.ErrorContent, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                }
            }
            catch (Exception ex)
            {
                e.ErrorContent = ex.Message;
                e.SetError(e.ErrorContent, DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                return;
            }
            
        }

        private void FORECAST_EDIT_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
        }    

        private void FORECAST_EDIT_TXB_GotFocus(object sender, RoutedEventArgs e)
        {
            IsForecastDeleting = true;
            var txb = sender as TextBox;
            txb.SelectionStart = 0;
            txb.SelectionLength = txb.Text.Length;
        }
        
        private void FORECAST_EDIT_TXB_LostFocus(object sender, RoutedEventArgs e)
        {
            IsForecastDeleting = false;
            var data = grid.SelectedItem as IDailyData;
            Forecast.Insert(
                data, 
                MainWindow.myTopMenuViewModel.AVERAGE_TURN, 
                MainWindow.myTopMenuViewModel.AVERAGE_TURN_L4WK);
        }

        private void tableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (IsForecastDeleting) { e.Handled = true; return; }
            try
            {
                IDailyData data = (IDailyData)e.Row;
                if (data == null) { e.Handled = true; return; }
                switch (e.Column.FieldName)
                {
                    case "Data.FORECASTS":
                    case "Data.BASE_LINE.VALUE":
                        Forecast.Insert(data, MainWindow.myTopMenuViewModel.AVERAGE_TURN, MainWindow.myTopMenuViewModel.AVERAGE_TURN_L4WK);
                        break;              
                    case "Data.COMMENT.VALUE":
                        Comment.UpdateOrInsert(data);
                        break;
                    case "Data.QC.VALUE":                        
                        QcQd.InsertOrUpdateValue(data, data.QC, "QC");
                        break;
                    case "Data.QD.VALUE":
                        QcQd.InsertOrUpdateValue(data, data.QD, "QD");
                        break;
                    case "Data.FSI.VALUE":
                        if (data.FSI.VALUE == null)
                        {
                            Fsi.Delete(data);
                            break;
                        }
                        Fsi.UpdateOrInsert(data);
                        break;
                    default:
                        break;
                }
            }
            catch (InsertException ix)
            {
                LogManger.RaiseErrorMessage(new Message { MESSAGE = ix.Message });
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "Unable to change the value." });
            }
        }

        /// <summary>
        /// <para>This code was updated on 12/17/2015 to allow summing multiple columns</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableviewDaily3_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            dailyGrid.SelectedItems.Clear();
            dailyGrid2.SelectedItems.Clear();
            TableView table = (TableView)sender;
            AddExceFunctionalities(table, dailyGrid3);
          
        }
        /// <summary>
        /// DAILY GRID ON SELECTION CHANGED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// <para>This code was updated on 12/17/2015 to allow summing multiple columns</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableviewDaily2_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            dailyGrid.SelectedItems.Clear();
            dailyGrid3.SelectedItems.Clear();
            TableView table = (TableView)sender;
            AddExceFunctionalities(table, dailyGrid2);
           
        }
        /// <summary>
        /// <para>This code was updated on 12/17/2015 to allow summing multiple columns</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableviewDaily_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            dailyGrid2.SelectedItems.Clear();
            dailyGrid3.SelectedItems.Clear();
            TableView table = (TableView)sender;
            AddExceFunctionalities(table, dailyGrid);          
        }

        void AddExceFunctionalities(TableView table, _GridControl_Quantity grid)
        {
            var items = grid.SelectedItems;
            if (items == null) return;
            //SelectedRowsCollection rows =  table.SelectedRows;
            //GridColumn column = table.FocusedColumn;
            IList<GridCell> cells = table.GetSelectedCells();
            IEnumerable<GridColumn> columns = cells.Select(x => x.Column).Distinct();

            if (items.Count > 0 && columns.Count() > 0)
            {
                var list = new List<int>();
                var colsNames = new List<string>();
                foreach (int rowIndex in grid.GetSelectedRowHandles())
                {
                    try
                    {

                        var cols = columns; //.Where(c => c.GridRow == rowIndex);

                        foreach (var col in cols)
                        {
                            var name = col.Name
                                .Replace("D_", "")
                                .Replace("2", "")
                                .Replace("3", "");

                            switch (name)
                            {
                                case "QS":
                                case "QA":
                                case "QW":
                                case "QO":
                                case "QP":
                                case "LYQS":
                                case "WTQS":
                                    int integer;
                                    if (int.TryParse(
                                        grid.GetCellValueByListIndex(
                                        rowIndex, col.FieldName).ToString(), out integer))
                                    {
                                        list.Add(integer);
                                        if (!colsNames.Any(c => c == name))
                                            colsNames.Add(name);
                                    }
                                    break;
                                default:
                                    //MainWindow.myStatusBarViewModel.ClearTextProperty();
                                    break;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        LogManger.Insert(ex);
                        MainWindow.myStatusBarViewModel.SetTextProperty(Properties.Settings.Default.ERROR);
                    }
                }

                if (list.Count() > 0)
                {
                    var names = colsNames.ConvertStringArrayToStringWithAPipe();
                    _SetStatusBarText(list, names);
                }
                else
                    MainWindow.myStatusBarViewModel.ClearTextProperty();
            }
            else
            {
                MainWindow.myStatusBarViewModel.ClearTextProperty();
            }
        }
        /// <summary>
        /// WEEKLY GRID ON SELECTION CHANGED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableView_SelectionChanged_2(object sender, GridSelectionChangedEventArgs e)
        {
            
            TableView table = (TableView)sender;
            var items = grid.SelectedItems;
            if (items == null) return;
            //SelectedRowsCollection rows = table.SelectedRows;
            
            //GridColumn column = table.FocusedColumn;
            IList<GridCell> cells =  table.GetSelectedCells();
            IEnumerable<GridColumn> columns = cells.Select(x => x.Column).Distinct();
            //var c = cells.Where(c => c.Column == grid.Columns[5]).First();
            
            if (items.Count > 0)
            {
                try
                {
                    TAB.myWeeklyData.FocusedDataCollection = items.Cast<IDailyData>();
                    if (TAB.myWeeklyData.FocusedDataCollection == null) return;
                    var content = TAB.myWeeklyData.FocusedDataCollection;
                    //MainWindow.myWeeklyData.FocusedDataCollection = items.Cast<IDailyData>();
                    //if (MainWindow.myWeeklyData.FocusedDataCollection == null) return;

                    var name = grid.CurrentColumn.Name;
                    string[] acceptableColumns = new string[] { "QS", "QA", "QW", "QO", "ORDER", "LY", "FORECAST", "QD", "QC", "BASE_LINE", "BASE_INDEX" };
                    var t = columns.Where(x => acceptableColumns.Contains(x.Name));
                    if (t.Count() > 1) name = ""; //IF THERE ARE MULTIPLE COLUMNS SELECTED, DO NOT DISPLAY STATUSBAR DETAIL 
                    else if (t.Count() == 0) name = "none";
                    switch (name)
                    {
                        case "QS":
                            _SetStatusBarText(content.Select(x => x.QS), name);
                            break;
                        case "QA":
                            _SetStatusBarText(content.Select(x => x.QA), name);
                            break;
                        case "QW":
                            _SetStatusBarText(content.Select(x => x.QW), name);
                            break;
                        case "QO":
                            _SetStatusBarText(content.Select(x => x.QO), name);
                            break;
                        case "LY":
                            _SetStatusBarText(content.Select(x => x.LYQS), name);
                            break;
                        case "FORECAST":
                            _SetStatusBarText(content.Select(x => x.FORECASTS.GetLastValue()), name);
                            break;
                        case "BASE_LINE":
                            _SetStatusBarText(content.Select(x => x.BASE_LINE.VALUE), name);
                            break;
                        case "BASE_INDEX":
                            _SetStatusBarText(content.Select(x => x.BASE_INDEX.ToDecimal()), name);
                            break;
                        case "QD":
                            _SetStatusBarText(content.Select(x => x.QD.GetValue()), name);
                            break;
                        case "QC":
                            _SetStatusBarText(content.Select(x => x.QC.GetValue()), name);
                            break;
                        case "ORDER":
                            _SetStatusBarText(content.Select(x => Convert.ToInt32(x.ORDER_DELIVERY)), name);
                            break;
                        case "":

                            var cols = t.ToArray();
                            List<object> result = new List<object>();

                            foreach (var cell in cells)
                                if (cols.Any(x => x == cell.Column))
                                {
                                    var value = grid.GetCellValue(cell.RowHandle, cell.Column).ToString().FilterComma();
                                    result.Add(Convert.ToInt32(value));
                                }
                            _SetStatusBarText(result.Cast<int>(), t.Select(x => x.Name).ConvertStringArrayToStringWithAPipe());
                            break;
                        default:
                            MainWindow.myStatusBarViewModel.ClearTextProperty();
                            break;
                    }
                }
                catch (FormatException fx)
                {
                    LogManger.Insert(fx);
                    TAB.myWeeklyData.FocusedDataCollection = null;
                    MainWindow.myStatusBarViewModel.ClearTextProperty();
                    MainWindow.myStatusBarViewModel.SetTextProperty(Properties.Settings.Default.ERROR);
                }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    TAB.myWeeklyData.FocusedDataCollection = null;                    
                    MainWindow.myStatusBarViewModel.ClearTextProperty();
                    MainWindow.myStatusBarViewModel.SetTextProperty(Properties.Settings.Default.ERROR);
                }
            }
            else
            {
                TAB.myWeeklyData.FocusedDataCollection = null;
                //MainWindow.myWeeklyData.FocusedDataCollection = null;
                MainWindow.myStatusBarViewModel.ClearTextProperty();
            }
        }

        private static void _SetStatusBarText(IEnumerable<int> value, string field)
        {
            MainWindow.myStatusBarViewModel.SetTextProperty(value.Max(), value.Min(), value.Sum(), value.Average(), value.Count(), field);
        }

        private static void _SetStatusBarText(IEnumerable<decimal> value, string field)
        {
            MainWindow.myStatusBarViewModel.SetTextProperty(value.Max(), value.Min(), value.Sum(), value.Average(), value.Count(), field);
        }

        private static void _HandleCellComment(string columnName)
        {
            try
            {
                var tab = MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB;
                var content = tab.myWeeklyData.FocusedDataCollection;
                if (content == null || content.Count() > 1) return;
                //OPEN WINDOW FOR EDITING THE CELL COMMENT
                var data = tab.myWeeklyData.FocusedData;
                //var data = MainWindow.myWeeklyData.FocusedData;
                var comment = string.Empty;
                switch (columnName)
                {
                    case "QS":
                        comment = data.QS_COMMENT.VALUE;
                        break;
                    case "QO":
                        comment = data.QO_COMMENT.VALUE;
                        break;
                    case "QA":
                        comment = data.QA_COMMENT.VALUE;
                        break;
                    case "QW":
                        comment = data.QW_COMMENT.VALUE;
                        break;
                    case "LY":
                        comment = data.LY_COMMENT.VALUE;
                        break;
                    case "QC":
                        comment = data.QC.COMMENT;
                        break;
                    case "QD":
                        comment = data.QD.COMMENT;
                        break;
                    case "FORECAST":
                        comment = data.FORECASTS.COMMENT.VALUE;
                        break;
                    case "BASE_LINE":
                        comment = data.BASE_LINE.COMMENT.VALUE;
                        break;
                    case "ORDER":
                        //if(data.ORDER_DELIVERY_COMMENT == null) data.ORDER_DELIVERY_COMMENT = new 
                        comment = data.ORDER_DELIVERY_COMMENT.VALUE;
                        break;
                    default:
                        break;
                }
                Presentation.Views.CellComment win = new Views.CellComment(tab)
                {
                    COMMENT = comment,
                    FIELD = columnName.ToField()
                };
                win.WindowStartupLocation = WindowStartupLocation.Manual;
                win.Left = System.Windows.Forms.Control.MousePosition.X;
                win.Top = System.Windows.Forms.Control.MousePosition.Y;
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "Unable to open comment box." });
            }
        }
        private void CELL_COMMENT_TXB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            
            //if (e.RightButton == MouseButtonState.Pressed)
            //{
            //    var columnName = ((TextBlock)sender).Name.Replace("_TXB", string.Empty);
            //    _HandleCellComment(columnName);
            //}
        }

        private void grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsForecastDeleting)
            {
                e.Handled = true;
                return;
            }
            if ((e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)) && e.KeyboardDevice.IsKeyDown(Key.V))
            {
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "To paste values, right click on the cell and click on Paste from Excel" });
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Delete)
            {
                try
                {
                    TableView view = (sender as GridControl).View as TableView;
                    IList<GridCell> cells = view.GetSelectedCells();
                    IEnumerable<GridColumn> columns = cells.Select(x => x.Column).Distinct();

                    if (columns.Count() == 1)
                    {
                        ColumnBase column = grid.CurrentColumn;
                        var items = grid.SelectedItems.Cast<IDailyData>();
                        switch (column.Name)
                        {
                            case "QC":
                                if (items == null || items.Count() == 0) break;
                                foreach (var item in items)
                                {
                                    QcQd.Delete(item, column.Name);
                                    (item).QC.SetValue(0.ToString());
                                }
                                break;
                            case "QD":
                                if (items == null || items.Count() == 0) break;
                                foreach (var item in items)
                                {
                                    QcQd.Delete(item, column.Name);
                                    (item).QD.SetValue(0.ToString());
                                }
                                break;
                            case "FSI":
                                if (items == null || items.Count() == 0) break;
                                foreach (var item in items)
                                {
                                    Fsi.Delete(item);
                                    (item).FSI.VALUE = string.Empty;
                                }
                                break;
                            case "COMMENT":
                                if (items == null || items.Count() == 0) break;
                                foreach (var item in items)
                                {
                                    Comment.Delete(item);
                                    (item).COMMENT.VALUE = string.Empty;
                                }
                                break;
                            case "FORECAST":
                                if (items == null || items.Count() != 1)
                                    throw new DeleteException("You can only delete one record at a time.");
                                if ((items.FirstOrDefault()).FORECASTS.LASTVALUE != 0.ToString())
                                {
                                    int rowIndex = grid.View.FocusedRowHandle;
                                    IsForecastDeleting = true;
                                    grid.SetCellValue(rowIndex, column.Name, 0);
                                    foreach (var item in items)
                                    {
                                        item.OnPropertyChanged("FORECASTS");
                                    }
                                    IsForecastDeleting = false;
                                    Forecast.Delete(this.TAB);
                                    grid.SelectItem(rowIndex + 1);
                                    grid.UnselectAll();
                                    grid.SelectItem(rowIndex);
                                    grid.CurrentColumn = column;
                                }
                                break;
                            case "BASE_LINE":
                                    if (items == null || items.Count() != 1)
                                    throw new DeleteException("You can only delete one record at a time.");
                                var row = items.FirstOrDefault();
                                if (row.BASE_LINE.VALUE != 0)
                                {
                                    int oldValue = row.BASE_LINE.VALUE;
                                    int rowIndex = grid.View.FocusedRowHandle;
                                    row.BASE_LINE.VALUE = 0;
                                    try
                                    {
                                        Forecast.Insert(row, MainWindow.myTopMenuViewModel.AVERAGE_TURN, MainWindow.myTopMenuViewModel.AVERAGE_TURN_L4WK);
                                        grid.SelectItem(rowIndex + 1);
                                        grid.UnselectAll();
                                        grid.SelectItem(rowIndex);
                                        grid.CurrentColumn = column;
                                    }
                                    catch (ItemNotInsertedException)
                                    {
                                        row.BASE_LINE.VALUE = oldValue;
                                    }
                                }
                                break;
                            default:
                                break;

                        }

                    }
                }
                catch (DeleteException) { }
                catch (ItemNotInsertedException) { }
                catch (Exception ex)
                {
                    LogManger.Insert(ex);
                    LogManger.RaiseErrorMessage(new Message { MESSAGE = ex.Message });
                }
            }
        }


        private void tableView_ShowGridMenu(object sender, GridMenuEventArgs e)
        {

            // Check whether this event was raised for a column's context menu.
            if (e.MenuType != GridMenuType.RowCell) return;
            TableView view = (sender as TableView);

            if (_SelectedData == null) _SelectedData = new List<IDailyData>();
            else _SelectedData.Clear();
            var items = grid.SelectedItems;
            if (items == null) return;
            foreach (var item in items)
                _SelectedData.Add(item as IDailyData);
            IList<GridCell> cells = view.GetSelectedCells();
            IEnumerable<GridColumn> columns = cells.Select(x => x.Column).Distinct();
            if (columns.Count() != 1) return;

            // Remove the Column Chooser menu item.
            e.Customizations.Add(new RemoveBarItemAndLinkAction()
            {
                ItemName = DefaultColumnMenuItemNames.ColumnChooser
            });

            
            var column = grid.CurrentColumn;
            if (items.Count == 1 && column.Header.ToString().ContainsAny(new string[] { "QA", "QS", "FORECAST", "QC", "QD", "OUTS", "LY QS", "QW", "ORDER", "BASE LINE" }))
            {
                switch (column.Header.ToString())
                {
                    case "QC":
                        var row = _SelectedData[0];
                        if (!string.IsNullOrEmpty(row.QC.FORMULA))
                        {
                            BarButtonItem formula = new BarButtonItem { Name = "FormulaItem", Content = row.QC.FORMULA };
                            e.Customizations.Add(formula);
                            e.Customizations.Add(new BarItemSeparator());
                        }
                        break;
                    case "QD":
                        var row1 = _SelectedData[0];
                        if (!string.IsNullOrEmpty(row1.QD.FORMULA))
                        {
                            BarButtonItem formula = new BarButtonItem { Name = "FormulaItem", Content = row1.QD.FORMULA };
                            e.Customizations.Add(formula);
                            e.Customizations.Add(new BarItemSeparator());
                        }
                        break;

                }               
                // Create a COMMENT menu item and add it to the context menu.
                BarButtonItem bi = new BarButtonItem() { Name = "CommentItem", Content = "Comment", Tag = new Duel<int, ColumnBase>(view.FocusedRowHandle, column) };
                //bi.Glyph = new im
                bi.ItemClick += CommentItem_ItemClick;
                e.Customizations.Add(bi);
                e.Customizations.Add(new BarItemSeparator());
            }
            if (column.Header.ToString().ContainsAny(new string[] { "FORECAST", "QC", "QD", "EVENTS", "COMMENT", "BASE LINE" }))
            {
                // Create a PASTE menu item and add it to the context menu.
                BarButtonItem bi = new BarButtonItem() { Name = "PasteItem", Content = "Paste from Excel", Tag = new Duel<int, ColumnBase>(view.FocusedRowHandle, column) };
                //bi.Glyph = new im
                bi.ItemClick += PasteItem_ItemClick;
                e.Customizations.Add(bi);                
            }

            if (column.Header.ToString().ContainsAny(new string[] { "PERFORMANCE", "FORECAST", "QC", "QD", "EVENTS", "COMMENT", "BASE LINE" }))
            {
                //create a COPY menu item and add it to the context menu.
                BarButtonItem bcopy = new BarButtonItem() { Name = "CopyItem", Content = "Copy", Tag = new Duel<int, ColumnBase>(view.FocusedRowHandle, column) };
                bcopy.ItemClick += bcopy_ItemClick;
                e.Customizations.Add(bcopy);
            }
        }

        void bcopy_ItemClick(object sender, ItemClickEventArgs e)
        {
            var bi = sender as BarButtonItem;
            var tag = bi.Tag as Duel<int, ColumnBase>;
            var currentItem = grid.GetRow(tag.KEY) as IDailyData ;
            if (_SelectedData == null)
            {
                _SelectedData = new List<IDailyData>();
                _SelectedData.AddRange(grid.SelectedItems.Cast<IDailyData>());
            }
            else if( !_SelectedData.Any(i => i.WEEK_ID == currentItem.WEEK_ID))
                _SelectedData = new List<IDailyData> { grid.GetRow(tag.KEY) as DailyData };
            var column = tag.VALUE.Header.ToString().ToUpper();
            Clipboard.Clear(); 
            StringBuilder sb = new StringBuilder();
            List<string> values = new List<string>();
            if (_SelectedData == null)
            {
                values.Add("Error");
                goto Finish;
            }
            switch (column)
            {
                case "PERFORMANCE":                    
                    values.AddRange(_SelectedData.Select(x=>x.PERFORMANCES).Select(x=>x.FIRST_DISPLAY));
                    break;
                case "FORECAST":
                    values.AddRange(_SelectedData.Select(x => x.FORECASTS).Select(x => x.LASTVALUE.ToString()));
                    break;
                case "BASE LINE":
                    values.AddRange(_SelectedData.Select(x => x.BASE_LINE).Select(x => x.VALUE.ToString()));
                    break;
                case "QC":
                    values.AddRange(_SelectedData.Select(x => x.QC).Select(x => x.GetValue().ToString()));
                    break;
                case "QD":
                    values.AddRange(_SelectedData.Select(x => x.QD).Select(x => x.GetValue().ToString()));
                    break;
                case "EVENTS":
                    values.AddRange(_SelectedData.Select(x => x.FSI).Select(x => x.VALUE.ToString()));
                    break;
                case "COMMENT":
                    values.AddRange(_SelectedData.Select(x => x.COMMENT).Select(x => x.VALUE.ToString()));
                    break;
                default:
                    break;
            }
Finish:
            for (int i = 0; i < values.Count(); i++)
                sb.Append(values[i] + "\r\n");
            Clipboard.SetText(sb.ToString(), TextDataFormat.CommaSeparatedValue);
        }

        private void CommentItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var btn = (BarButtonItem)sender;
            var tag = (Duel<int, ColumnBase>)btn.Tag;
            _HandleCellComment(tag.VALUE.Name);
        }
        private void PasteItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IList<GridCell> cells = tableView.GetSelectedCells();
                List<GridColumn> columns = cells.Select(x => x.Column).Distinct().ToList();
                if (columns.Count() > 1)
                {
                    LogManger.RaiseErrorMessage("Select only one column.");
                    e.Handled = true;
                    return;
                }
                var selectedRows = grid.GetSelectedRowHandles();
                var bi = sender as BarButtonItem;
                var tag = bi.Tag as Duel<int, ColumnBase>;
                if (tag.VALUE.Header.ToString().ToUpper() != columns[0].Header.ToString().ToUpper())
                {
                    LogManger.RaiseErrorMessage(new Message { MESSAGE = "Select only one column." });
                    e.Handled = true;
                    return;
                }
                IDataObject data = Clipboard.GetDataObject();
                if (data.GetDataPresent(DataFormats.CommaSeparatedValue))
                {
                    var s = data.GetData(DataFormats.CommaSeparatedValue).ToString();
                    string[] originalLines = Regex.Split(s, "\r\n");
                    var lines = originalLines.Take(originalLines.Count() - 1).ToArray();
                    if (_SelectedData != null)
                    {
                        MainWindow.SetBusyState(true);
                        var h = tag.VALUE.Header.ToString().ToUpper();
                        switch (h)
                        {
                            case "FORECAST":
                            case "BASE LINE":
                            case "QC":
                            case "QD":
                                for (int i = 0; i < lines.Count(); i++)
                                    if (lines[i] == "") lines[i] = "0";

                                if (lines.Any(x => !x.IsInt()))
                                {
                                    LogManger.RaiseErrorMessage(new Message { MESSAGE = "One or more of your values are not whole numbers." });
                                    break;
                                }

                                for (int j = 0; j < lines.Count(); j++)
                                {
                                    var weeklyData = (IDailyData)grid.GetRow(cells[0].RowHandle + j);
                                    if (weeklyData == null) break;
                                    //attach week id so that the converter knows which week this is for.
                                    var value = String.Format("{0}:{1}", weeklyData.WEEK_ID, lines[j]);
                                    if (h == "BASE LINE")
                                        grid.SetCellValue(cells[0].RowHandle + j, cells[0].Column, lines[j]);
                                    else
                                        grid.SetCellValue(cells[0].RowHandle + j, cells[0].Column, value);
                                }
                                break;
                            case "EVENTS":
                            case "COMMENT":
                                for (int j = 0; j < lines.Count(); j++)
                                {
                                    var weeklyData = (IDailyData)grid.GetRow(cells[0].RowHandle + j);
                                    if (weeklyData == null) break;
                                    grid.SetCellValue(cells[0].RowHandle + j, cells[0].Column, lines[j]);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (data.GetDataPresent(DataFormats.StringFormat))
                {
                    var s = data.GetData(DataFormats.StringFormat).ToString();
                    if (_SelectedData != null)
                    {
                        MainWindow.SetBusyState(true);
                        switch (tag.VALUE.Header.ToString().ToUpper())
                        {
                            case "FORECAST":
                            case "BASE_LINE":
                            case "QC":
                            case "QD":
                                int i;
                                if (!int.TryParse(s, out i))
                                {
                                    LogManger.RaiseErrorMessage(new Message { MESSAGE = "One or more of your values are not whole numbers." });
                                    break;
                                }
                                else
                                {
                                    var weeklyData = (IDailyData)grid.GetRow(cells[0].RowHandle);
                                    if (weeklyData == null) break;
                                    //attach week id so that the converter knows which week this is for.
                                    var value = String.Format("{0}:{1}", weeklyData.WEEK_ID, i);
                                    grid.SetCellValue(cells[0].RowHandle, cells[0].Column, value);
                                }
                                break;
                            case "EVENTS":
                            case "COMMENT":
                                var wData = (IDailyData)grid.GetRow(cells[0].RowHandle);
                                if (wData == null) break;
                                grid.SetCellValue(cells[0].RowHandle, cells[0].Column, s);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    throw new FormatException("System was not able to parse your data.\r\rPlease contact Michael Seifi");
                }
            }
            catch (FormatException fx)
            {
                LogManger.Insert1(fx, "Formating error in pasting onto the Grid");
                LogManger.RaiseErrorMessage(new Message { MESSAGE = fx.Message });
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "General error in pasting onto the Grid");
                LogManger.RaiseErrorMessage(new Message { MESSAGE = "System was unable to complete pasting your data.\r\rPlease contact Michael Seifi." });
            }
        }

        private void SKU_COMMENT_CNTRL_Loaded(object sender, RoutedEventArgs e)
        {
            SKU_COMMENT_CNTRL.Content = new UserControls.SkuComment { TAB = TAB };
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "RowNumber")
            {
                e.Value = grid.GetRowVisibleIndexByHandle(grid.GetRowHandleByListIndex(e.ListSourceRowIndex)) + 1;
            }
        }

        private void tableviewDaily_RowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            int row = e.HitInfo.RowHandle;
            dailyGrid.View.FocusedRowHandle =
                dailyGrid2.View.FocusedRowHandle =
                dailyGrid3.View.FocusedRowHandle = row;
            //var obj = dailyGrid.GetRow(row);
            //var obj2 = dailyGrid2.GetRow(row);
            //var obj3 = dailyGrid3.GetRow(row);

            //dailyGrid2.CurrentItem = obj2;
        }

        int dailyFocusedRow = 0;
        private void dailyGrid_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            dailyFocusedRow = 0;
            var grid = sender as GridControl;
            IEnumerable<IDailyData> data = grid.ItemsSource as IEnumerable<IDailyData>;
            if (data != null && data.Count() > 0)
            {
                var futureData = data.Where(d => d.IS_FUTURE);
                if (futureData.Count() > 0)
                {
                    var dataList = data.ToList();
                    for (int i = 0; i < data.Count(); i++)
                        dataList[i].INDEX = (uint)i;

                    var currentData = data.Where(d => d.IS_FUTURE == false);
                    if (currentData.Count() > 0)
                    {
                        var lastData = currentData.First(d => d.REPORT_AS_OF_DATE == currentData.Max(x => x.REPORT_AS_OF_DATE));
                        dailyFocusedRow = (int)lastData.INDEX;                        
                    }
                }
                
            }
            dailyGrid.View.FocusedRowHandle =
                    dailyGrid2.View.FocusedRowHandle =
                    dailyGrid3.View.FocusedRowHandle = dailyFocusedRow;
        }

        private void dailyGrid3_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            dailyGrid3.View.FocusedRowHandle = dailyFocusedRow;
        }

        private void dailyGrid2_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            dailyGrid2.View.FocusedRowHandle = dailyFocusedRow;
        }
    }
}
