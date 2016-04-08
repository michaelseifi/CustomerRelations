using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Extensions;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Presentation.UserControls
{
    /// <summary>
    /// Interaction logic for PerformanceExceptions.xaml
    /// </summary>
    public partial class PerformanceExceptions : UserControl
    {
        ITab TAB { get; set; }
        public Presentation.Views.Exceptions OWNER { get; set; }
        private int _CUSTOMER_ID;
        private string _SKU;
        private IPerformance DGridItem { get; set; }
        private datastores.FORECASTER_PERFORMANCES_EXCEPTIONS CGridItem { get; set; }
        private int? CGridItemCurrentState { get; set; }
        public PerformanceExceptions(ITab tab)
        {
            InitializeComponent();
            TAB = tab;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _CUSTOMER_ID = int.Parse(TAB.myWeeklyData.CUSTOMER.ACCOUNTNUM);
            _SKU = TAB.myWeeklyData.SKUID.SKUID;
            var list = TAB.myWeeklyData.PERFORMANCES.Where(p => p.IS_DEFAULT); //.Where(i => TAB.myWeeklyData.SelectMany(s => s.PERFORMANCES).Select(s => s.DOCUMENTOBJECTID).Contains(i.DOCUMENTOBJECTID));
            this.DEFAULT_GRID.ItemsSource = list;
            //this.Where(x => x.PERFORMANCES.Any(i => i.DOCUMENTOBJECTID == guid));
            //update the performances if they haven't already

            if (TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS == null) TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS =
                new Controlers.Collections.PerformanceExceptionsCollection(TAB.myPerformanceExceptions.Where(x => x.CUSTOMER_ID == _CUSTOMER_ID && x.IS_DEFAULT == false && x.SKU == _SKU).ToList(), TAB);
            this.CUSTOM_GRID.ItemsSource = TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS;
            System.ComponentModel.SortDescription sort = new System.ComponentModel.SortDescription("FROM", System.ComponentModel.ListSortDirection.Ascending);
            this.CUSTOM_GRID.Items.SortDescriptions.Add(sort);
            if (this.CUSTOM_GRID.Items.Count > 0)
                this.CUSTOM_GRID.ScrollIntoView(this.CUSTOM_GRID.Items[this.CUSTOM_GRID.Items.Count - 1]);
        }

        
        private void CLOSE_Click(object sender, RoutedEventArgs e)
        {
            //HANDLE RECIRDS THAT WERE STARTED BY CLICKING ON A NEW ROW BUT NOT COMPLETED
            var itemSource = (Controlers.Collections.PerformanceExceptionsCollection)(this.CUSTOM_GRID.ItemsSource);
            itemSource.RemoveRange(itemSource.Where(x => x.GUID == Guid.Empty));

            //REMOVE THE DEFAULT DATAGRID SCROLLBAR EVENT
            DEFAULT_GRID.RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(dg_ScrollChanged));
            CUSTOM_GRID.RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(cg_ScrollChanged));
            if (OWNER != null) OWNER.Close();
        }

        private void MStateCbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CGridItem == null) { e.Handled = true; return; }
            var row = (datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)this.CUSTOM_GRID.SelectedItem;
            if (row == null || row.GUID != CGridItem.GUID || row.GUID == Guid.Empty) { e.Handled = true; return; }
            var cbbx = sender as ComboBox;
            if (CGridItemCurrentState == null || CGridItemCurrentState == cbbx.SelectedIndex) { e.Handled = true; return; }
            row.STATE = cbbx.SelectedIndex;
            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.OnStateChanged(row);
            TAB.myWeeklyData.UpdatePerformance(row);
            //TAB.myWeeklyData.Update(x => x.UPDATE_PERFORMANCES());
            //TAB.myWeeklyData.Update(x => x.OnPropertyChanged(null));
            CUSTOM_GRID.UnselectAll();
        }

        private void CUSTOM_GRID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            CGridItem = null; CGridItemCurrentState = null;
            if (dg.SelectedItems.Count == 1 && dg.SelectedItem.GetType().Equals(typeof(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)))
            {
                CGridItem = (datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)dg.SelectedItem;
                CGridItemCurrentState = CGridItem.STATE;
            }            
        }

        private void CUSTOM_GRID_Loaded(object sender, RoutedEventArgs e)
        {
            //ADD THIS TO CAPTURE SCROLL BAR MOVE EVENT
            var dg = sender as DataGrid;
            dg.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(cg_ScrollChanged));
        }

        private void cg_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //SETTING HANDLED TO TRUE PRENVENTS THE MSTATECBBX_SELECTIONCHANGED EVENT TO BE FIRED WHEN THE GRID IS BEING SCROLLED BY MOVING THE SCROLL BAR
            e.Handled = true;
        }
        private void CUSTOM_GRID_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //SETTING HANDLED TO TRUE PRENVENTS THE MSTATECBBX_SELECTIONCHANGED EVENT TO BE FIRED WHEN THE GRID IS BEING SCROLLED BY A MOUSE WHEEL
            e.Handled = true;
        }

        private void DStateCbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGridItem == null) { e.Handled = true; return; }
            var row = (IPerformance)this.DEFAULT_GRID.SelectedItem;
            if (row == null || row.DOCUMENTOBJECTID != DGridItem.DOCUMENTOBJECTID) { e.Handled = true; return; }
            var cbbx = sender as ComboBox;
            row.D_STATE = (Controlers.Enums.DState)cbbx.SelectedIndex;
            var item = new datastores.FORECASTER_PERFORMANCES_EXCEPTIONS
            {
                GUID = row.DOCUMENTOBJECTID,
                CUSTOMER_ID = int.Parse(row.CUSTOMER_ID),
                DESCRIPTION = "",
                FROM = row.START_DATE,
                TO = row.END_DATE,
                TYPE = row.PERFORMANCE_TYPE,
                PRICE = row.PRICE,
                QUANTITY = Convert.ToInt32(row.QUANTITY),
                IS_DEFAULT = true,
                INCLUDE = false,
                SKU = row.SKU_ID,
                STATE = (int)row.D_STATE
            };

            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.OnStateChanged(item);
            if (row.D_STATE == Controlers.Enums.DState.None && !row.IS_INCLUDED)
            {
                TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.DeleteItem(item);
                row.IS_INCLUDED = true;
                //TAB.myWeeklyData.Update(x => x.UPDATE_PERFORMANCES());
                TAB.myWeeklyData.GetAverageTurn();
                TAB.myWeeklyData.Update(x => x.OnPropertyChanged(null));
                if (TAB.myPercentageViewModel != null)
                    TAB.myPercentageViewModel.Update();
                if (TAB.myGraphViewModel != null)
                {
                    TAB.myGraphViewModel.SetQSStatisticalData();
                    TAB.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS += 0;
                    TAB.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS += 0;
                    TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS += 0;
                }
            }
            else
            {
                //TAB.myWeeklyData.Update(x => x.UPDATE_PERFORMANCES());
                TAB.myWeeklyData.Update(x => x.OnPropertyChanged(null));
            }
            DEFAULT_GRID.UnselectAll();
        }

        private void DEFAULT_GRID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg.SelectedItems.Count != 1)
                DGridItem = null;
            else
                DGridItem = (IPerformance)dg.SelectedItem;
        }

        private void DEFAULT_GRID_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //SETTING HANDLED TO TRUE PRENVENTS THE DSTATECBBX_SELECTIONCHANGED EVENT TO BE FIRED WHEN THE GRID IS BEING SCROLLED BY A MOUSE WHEEL
            e.Handled = true;
        }

        private void DEFAULT_GRID_Loaded(object sender, RoutedEventArgs e)
        {
            //ADD THIS TO CAPTURE SCROLL BAR MOVE EVENT
            var dg = sender as DataGrid;
            dg.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(dg_ScrollChanged));
        }

        private void dg_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //SETTING HANDLED TO TRUE PRENVENTS THE DSTATECBBX_SELECTIONCHANGED EVENT TO BE FIRED WHEN THE GRID IS BEING SCROLLED BY MOVING THE SCROLL BAR
            e.Handled = true;
        }

        /// <summary>
        /// ACTIVATE A DEFAULT PERFORMANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivateBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBusyState(true);
            var row = (IPerformance)this.DEFAULT_GRID.SelectedItem;
            if (row == null) return;
            var item = new datastores.FORECASTER_PERFORMANCES_EXCEPTIONS
            {
                GUID = row.DOCUMENTOBJECTID,
                CUSTOMER_ID = int.Parse(row.CUSTOMER_ID),
                DESCRIPTION = "",
                FROM = row.START_DATE,
                TO = row.END_DATE,
                TYPE = row.PERFORMANCE_TYPE,
                PRICE = row.PRICE,
                QUANTITY = Convert.ToInt32(row.QUANTITY),
                IS_DEFAULT = true,
                INCLUDE = false,
                SKU = row.SKU_ID,
                STATE = (int)row.D_STATE
            };
            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.DeleteItem((datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)item);
            row.IS_INCLUDED = true;

            //TAB.myWeeklyData.Update(x => x.UPDATE_PERFORMANCES());
            TAB.myWeeklyData.GetAverageTurn();
            TAB.myWeeklyData.Update(x => x.OnPropertyChanged(null));
            if (TAB.myPercentageViewModel != null)
                TAB.myPercentageViewModel.Update();
            if (TAB.myGraphViewModel != null)
            {
                TAB.myGraphViewModel.SetQSStatisticalData();
                TAB.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS += 0;
                TAB.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS += 0;
                TAB.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS += 0;
            }
            
        }

        /// <summary>
        /// DEACTIVATE A DEFAULT PERFORMANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeactivateBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SetBusyState(true);
            var row = (IPerformance)this.DEFAULT_GRID.SelectedItem;
            if (row == null) return;
            var item = new datastores.FORECASTER_PERFORMANCES_EXCEPTIONS
            {
                GUID = row.DOCUMENTOBJECTID,
                CUSTOMER_ID = int.Parse(row.CUSTOMER_ID),
                DESCRIPTION = "",
                FROM = row.START_DATE,
                TO = row.END_DATE,
                TYPE = row.PERFORMANCE_TYPE,
                PRICE = row.PRICE,
                QUANTITY = Convert.ToInt32(row.QUANTITY),
                IS_DEFAULT = true,
                INCLUDE = false,
                SKU = row.SKU_ID,
                STATE = (int)row.D_STATE
            };
            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.InsertOrUpdateItem(item); //add it to the database            
            TAB.myWeeklyData.RemovePerformance(row.DOCUMENTOBJECTID); //remove it from performance collection            
        }

        /// <summary>
        /// DELETE A CUSTOM PERFORMANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CUSTOM_GRID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainWindow.SetBusyState(true);
            var grid = sender as DataGrid;
            if (grid.SelectedIndex < 0) return;
            var gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(grid.SelectedIndex);
            var items = grid.SelectedItems;
            if (e.Key == Key.Delete && !gridRow.IsEditing)
            {
                // User is attempting to delete the row
                var result = MessageBox.Show(
                    "About to delete the current row(s).\n\nProceed?",
                    "Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);
                e.Handled = (result == MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                    foreach (var item in items)
                        TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.DeleteItem((datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)item);                
            }
        }

        /// <summary>
        /// ADD A CUSTOM PERFORMANCE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CUSTOM_GRID_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.IsEditing)
            {
                MainWindow.SetBusyState(true);
                var item = (datastores.FORECASTER_PERFORMANCES_EXCEPTIONS)e.Row.Item;
                
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.DESCRIPTION))
                    {
                        //MessageBox.Show("You need to write a note.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //e.Cancel = true;
                        //return;
                        item.DESCRIPTION = string.Empty;
                    }
                    if (item.TYPE == null)
                    {
                        MessageBox.Show("Select a type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        return;
                    }
                    else if (item.FROM > item.TO)
                    {
                        MessageBox.Show("Your starting date must be less than or equal to the ending date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        return;
                    }                    
                    else
                    {
                        if (e.Row.IsNewItem)
                        {
                            item.CUSTOMER_ID = _CUSTOMER_ID;
                            item.IS_DEFAULT = false;
                            item.INCLUDE = true;
                            item.SKU = _SKU;
                            item.GUID = Guid.NewGuid();
                            item.STATE = item.STATE ?? 0;
                            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.InsertOrUpdateItem(item); //insert it to the database
                            TAB.myPerformanceExceptions.Add(item); //add it to the performance exception collection
                            TAB.myWeeklyData.AddPerformance(new Performance
                            {
                                CUSTOMER_ID = item.CUSTOMER_ID.AddLeadingZeros(),
                                DOCUMENTOBJECTID = item.GUID,
                                END_DATE = item.TO,
                                IS_DEFAULT = false,
                                IS_INCLUDED = true,
                                TYPE = item.TYPE,
                                TYPE_ID = Performance.GetTypeId(item.TYPE),
                                PRICE = item.PRICE,
                                QUANTITY = item.QUANTITY,
                                START_DATE = item.FROM,
                                M_STATE = (Controlers.Enums.MState)item.STATE
                            });
                            (sender as DataGrid).RowEditEnding -= CUSTOM_GRID_RowEditEnding;
                            CUSTOM_GRID.CommitEdit(DataGridEditingUnit.Row, true);
                            CUSTOM_GRID.Items.Refresh();
                            (sender as DataGrid).RowEditEnding += CUSTOM_GRID_RowEditEnding;
                        }
                        else
                        {
                            TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.InsertOrUpdateItem(item); //insert it to the database

                            //var p = TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.First(i => i.GUID == item.GUID);
                            //p.DESCRIPTION = item.DESCRIPTION;
                            //p.FROM = item.FROM;
                            //p.PRICE = item.PRICE;
                            //p.QUANTITY = item.QUANTITY;
                            //p.TO = item.TO;
                            //p.TYPE = item.TYPE;

                            //var myP = TAB.myPerformanceExceptions.First(i => i.GUID == item.GUID);
                            //myP.DESCRIPTION = item.DESCRIPTION;
                            //myP.FROM = item.FROM;
                            //myP.PRICE = item.PRICE;
                            //myP.QUANTITY = item.QUANTITY;
                            //myP.TO = item.TO;
                            //myP.TYPE = item.TYPE;

                            TAB.myWeeklyData.UpdatePerformance(item);
                            
                        }
                        
                    }
                }
            }
        }

        private void CUSTOM_GRID_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.IsNewItem)
            {

            }
            else
            {
                //e.Cancel = true;                
            }
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp.SelectedDate.Value.Year < 2010)
                dp.SelectedDate = DateTime.Now;
        }

        private void CUSTOM_GRID_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var row = e.Item as datastores.FORECASTER_PERFORMANCES_EXCEPTIONS;
            var item = new datastores.FORECASTER_PERFORMANCES_EXCEPTIONS
            {
                CUSTOMER_ID = row.CUSTOMER_ID,
                DESCRIPTION = row.DESCRIPTION,
                FROM = row.FROM,
                GUID = row.GUID,
                INCLUDE = row.INCLUDE,
                IS_DEFAULT = row.IS_DEFAULT,
                PRICE = row.PRICE,
                QUANTITY = row.QUANTITY,
                SKU = row.SKU,
                TO = row.TO,
                TYPE = row.TYPE
            };
            App.myClipboard = item;
            //DataFormat format = datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2.GerFormat();
            
            //DataObject dataObj = new DataObject();
            //dataObj.SetData("Performance", (datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2)e.Item, true);
            //Clipboard.SetDataObject(dataObj, true);
            //if (Clipboard.ContainsData("Performance"))
            //{
            //    var cc = Clipboard.GetData("Performance");
            //    var c = cc;
            //}

        }

        private void CUSTOM_GRID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.V &&
                (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                try
                {
                    var item = App.myClipboard as datastores.FORECASTER_PERFORMANCES_EXCEPTIONS;
                    item.CUSTOMER_ID = int.Parse(TAB.CUSTOMER_NUMBER);
                    item.SKU = TAB.SKU.SKUID;
                    item.GUID = Guid.NewGuid();
                    TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.InsertOrUpdateItem(item); //insert it to the database
                    TAB.myPerformanceExceptions.Add(item); //add it to the performance exception collection
                    TAB.myWeeklyData.AddPerformance(new Performance
                    {
                        CUSTOMER_ID = item.CUSTOMER_ID.AddLeadingZeros(),
                        DOCUMENTOBJECTID = item.GUID,
                        END_DATE = item.TO,
                        IS_DEFAULT = false,
                        IS_INCLUDED = true,
                        TYPE = item.TYPE,
                        TYPE_ID = Performance.GetTypeId(item.TYPE),
                        PRICE = item.PRICE,
                        QUANTITY = item.QUANTITY,
                        START_DATE = item.FROM
                    });
                    TAB.myWeeklyData.PERFORMANCE_EXCEPTIONS.Add(item);
                }
                catch (Exception ex)
                {
                    LogManger.Insert1(ex, "Unable to cast the copied object to a performance");
                }
                //if (Clipboard.ContainsData("Performance"))
                //{
                //    var cc = Clipboard.GetData("Performance");
                //    var c = cc;
                //}
                    //as datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2;
                //System.Security.Permissions.UIPermission clip = new System.Security.Permissions.UIPermission(System.Security.Permissions.PermissionState.None) { Clipboard = System.Security.Permissions.UIPermissionClipboard.AllClipboard };
                //DataObject data =(DataObject)Clipboard.GetDataObject();
                //var formats = data.GetFormats();
                //var s = formats;
                ////var item = data.GetData(typeof(datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2));
                ////var i = item;
                //if (data.GetDataPresent("Performance", true))
                //{
                //    var item = data.GetData("Performance") as datastores.FORECASTER_PERFORMANCES_EXCEPTIONS2;

                //}
            }
        }

 

 



  

   








 






   
    }
}
