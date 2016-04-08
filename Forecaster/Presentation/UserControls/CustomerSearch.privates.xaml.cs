using System;
using System.Windows;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Helpers;
using Models = daisybrand.forecaster.Controlers.ViewModels;

using DevExpress.Xpf.Editors;
namespace daisybrand.forecaster.Presentation.UserControls
{
    public partial class CustomerSearch
    {        
        private async void _CallLoadWeeklyGridAsync(
            string accountNum, Tab tab, int numberOfYearsOrMonths, int view)
        {

            await PerformanceExceptionsCollection.GetAsync(tab);
            LogManger.InsertStep();
            int iterations = 0;
            switch (view)
            {
                case 0:
                    iterations = DateTime.Now.Year - numberOfYearsOrMonths + 1;
                    break;
                case 1:
                    iterations = numberOfYearsOrMonths;
                    break;
                default:
                    break;
            }

            MainWindow.CloseAllOtherWindowsIfOpen();
            var c = ((ICustomer)CUSTOMERS_COMBO.SelectedItem);
            
            var b = await _GetWeeklyDataTask(c, tab, iterations, view);
            
            _SetData(b, tab);
            
            _LoadWeeklyGrid(tab);

            //if (DataCollection.HasChanged(accountNum, skuId, iterations, view))
            //{
            //    MainWindow.CloseAllOtherWindowsIfOpen();
            //    var b = await _GetWeeklyDataTask(accountNum, skuId, iterations, view);
            //    b.CUSTOMER = ((ICustomer)CUSTOMERS_COMBO.SelectedItem);
            //    b.SKUID = ((ISku)SKUIDS_COMBO.SelectedItem);
            //    _SetData(b);
            //    _LoadWeeklyGrid(b);
            //}
            //else
            //{
            //    var b = await _WaitTask(2);
            //    _LoadWeeklyGrid(MainWindow.myDailyData);
            //}

            Models.TopMenu.EnableGraphs();
        }

        private void _SetData(DataCollection data, ITab tab)
        {
            LogManger.InsertStep();

            tab.myDailyData = data;
            tab.myDailyData.UpdatePerformances(tab.myPerformanceExceptions);
            //UPDATE PERFOMANCES WITH OTHER DATA
            if (tab.myDailyData == null || tab.myDailyData.Count() < 1) return;
            {
                tab.myDailyData.PERFORMANCES.Update(x => x.CR_ASSOCIATE = data.CUSTOMER.CR_ASSOCIATE);
                tab.myDailyData.PERFORMANCES.Update(x => x.REGIONAL_MANAGER = data.CUSTOMER.REGIONAL_MANGER);
                tab.myDailyData.PERFORMANCES.Update(x => x.TP_NAME = data.CUSTOMER.NAME);
            }

            tab.myDailyData.SetLASTDAYOFLASTWEEKWITHREALDATA();

            tab.myFirst120Data = new DataCollection(tab.myDailyData.Where(x => x.REPORT_AS_OF_DATE <= tab.myDailyData.LASTDAYWITHREALDATA).OrderByDescending(x => x.REPORT_AS_OF_DATE).Take(120))
            {
                CUSTOMER = tab.myDailyData.CUSTOMER,
                SKUID = tab.myDailyData.SKUID,
            };

            tab.myWeeklyData = DataCollection.GetWeekly(tab);
            //tab.myWeeklyData.UpdatePerformances(tab.myPerformanceExceptions);
            //PerformanceCollection.UpdateWithBannersAsync(tab.myWeeklyData.CUSTOMER.ACCOUNTNUM, tab.myWeeklyData);

        }


        private Task<DataCollection> _GetWeeklyDataTask(ICustomer customer, ITab tab, int iterations, int view)
        {
            return Task.Run<DataCollection>(() => _GetWeeklyDataCollection(customer, tab, iterations, view));
        }

        private DataCollection _GetWeeklyDataCollection(ICustomer customer, ITab tab, int iterations, int view)
        {
            return new DataCollection(customer, tab, iterations, view);
        }

        private bool _Wait(int seconds)
        {
            DateTime later = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < later)
            {

            }
            return true;
        }

        private Task<bool> _WaitTask(int seconds)
        {
            return Task.Run<bool>(() => _Wait(seconds));
        }

        private void _LoadWeeklyGrid(Tab tab)
        {
            LogManger.InsertStep();
            WeeklyDataGrid grid = 
                new WeeklyDataGrid(tab);         
            
            
            //_QUANTITY_GRID.Content = grid;

            _LoadViews(tab);
            //Owner.BUSYINDICATOR.IsBusy = false;
            tab.CONTENT = grid;

            tab.myGraphViewModel.IS_ORDERCOLLECTION_LOADED = false;
            OrderCasesCollection.GetAsync(tab);

            tab.myGraphViewModel.POS_COLLECTION_WAITING_STACKPANEL = null;
            tab.myGraphViewModel.IS_POSCOLLECTION_LOADED = false;

            //if this is a kroger and we are able to pull Kroger divisions
            if (PosCollections.KrogerDivisions != null && PosCollections.KrogerDivisions.Count() > 0)
                PosCollections.GetAsync(tab);

            ActualOrderCollection.GetAync(tab.myWeeklyData.CUSTOMER, tab);
            tab.IsLoaded = true;
        }

        private void _LoadViews(Tab tab)
        {
            LogManger.InsertStep();            
            tab.myGraphViewModel = 
                new Controlers.ViewModels.Graph(tab.myWeeklyData.CUSTOMER.ACCOUNT_NAME, tab.myWeeklyData.CUSTOMER.ACCOUNTNUM, tab);
            if (MainWindow.myTopMenuViewModel != null)
                MainWindow.myTopMenuViewModel.IS_DATA_LOEADED = true; 
                               
            if (tab.myPercentageViewModel == null)
                tab.myPercentageViewModel = new Models.Percentage(6, tab);
            else
                tab.myPercentageViewModel.Update();
            
        }
        private void _SetNumberOfIterations()
        {
            switch (VIEW.SelectedIndex)
            {
                case 0:
                    NUMBER_OF_DAYS_LBL.Content = "Number of years";
                    NUMBER_OF_ITERATIONS.Items.Clear();
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "1" });
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "2" });
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "3", IsSelected = true });
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "4" });
                    break;
                case 1:
                    NUMBER_OF_DAYS_LBL.Content = "Number of months";
                    NUMBER_OF_ITERATIONS.Items.Clear();
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "1" });
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "2", IsSelected = true });
                    NUMBER_OF_ITERATIONS.Items.Add(new ComboBoxEditItem() { Content = "3" });
                    break;
                default:
                    break;
            }
        }
    }
}
