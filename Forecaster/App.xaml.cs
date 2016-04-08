using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Markup;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using System.Diagnostics;
namespace daisybrand.forecaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SettingsCollection appSetiings;
        public static Controlers.ViewModels.MainWindow.Build BUILD;
        private bool IsAppSettingsSaved;
        public static SplashScreen screen;
        public static LocalDataStoreSlot myOrderStatusList;
        public static List<DispatcherOperation> myDispatchOperations;
        public static Object myClipboard;
        readonly bool DoHandle;

        protected override void OnStartup(StartupEventArgs e)
        {
            LogManger.OnRaiseError += Log_OnRaiseError;
            OrderStatusCollection.GetAync();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //conf.ConnectionStrings.ConnectionStrings.Remove("DAX_PRODEntities");
            //conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DAX_PRODEntities",
            //    @"metadata=res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.csdl|res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.ssdl|res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.msl;provider=System.Data.SqlClient;provider connection string=';Data Source=AXSQLVM01;Initial Catalog=DBIDAXTEST;Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework';",
            //    "System.Data.EntityClient"));
            //conf.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("ConnectionStrings");

            //the following fixes formating issues like dollar sign in the textboxe
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                CultureInfo.CurrentCulture.IetfLanguageTag)));
            myDispatchOperations = new List<DispatcherOperation>();
            base.OnStartup(e);
        }

        void Log_OnRaiseError(object sender, EventArgs e)
        {
            try
            {
                var s = (IMessage)sender;
                //Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                //{

                //    if (_Notification != null) _Notification.Close();
                //    _Notification = new Presentation.Views.NotificationWindow() { MESSAGE = s.MESSAGE };
                //    _Notification.Show();
                //}));
                MessageBox.Show(s.MESSAGE, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to display the error message");
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            LogManger.Insert1(ex, "Unhandled exception");
            LogManger.Insert1(ex.InnerException, "Unhandled inner exception");
            throw new ApplicationException(ex.InnerException.Message);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            screen = new SplashScreen("splashscreen.JPG");
            screen.Show(false); //close the splash after the welcome message          
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(UIElement), new FrameworkPropertyMetadata(int.MaxValue));
            //ToolTipService.PlacementRectangleProperty.OverrideMetadata(typeof(Rect), new FrameworkPropertyMetadata(Rect.Empty));
            LogManger.Insert("FAPPLICATION", "ENTER");
            _SetReleaseBuild();
            _SetTestBuild();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Save settings
            if (appSetiings != null && !IsAppSettingsSaved)
                IsAppSettingsSaved = appSetiings.Save();
            
            LogManger.Insert("FAPPLICATION", "EXITITING");
            Thread.FreeNamedDataSlot("myOrderStatusList");
            //check on threads
            AbortDispatcherOperations();
            LogManger.Insert("FAPPLICATION", "EXIT");            
        }

        public static void AbortDispatcherOperations()
        {
            try
            {
                if (myDispatchOperations != null && myDispatchOperations.Count() > 0)
                {
                    var incompleteList = myDispatchOperations.Where(d => d.Status == DispatcherOperationStatus.Executing);
                    if (incompleteList != null && incompleteList.Count() > 0)
                        foreach (var op in incompleteList)
                            op.Abort();
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to abort dispatcher operations");
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Save settings
            if (appSetiings != null && !IsAppSettingsSaved)
                IsAppSettingsSaved = appSetiings.Save();
            if (this.DoHandle)
            {
                //Handling the exception within the UnhandledException handler.        
                LogManger.SendEmailExceptionToMe(e.Exception, "DispatcherUnhandledException");
                LogManger.SendEmailExceptionToMe(e.Exception.InnerException, "DispatcherUnhandledInnerException");
                MessageBox.Show(e.Exception.Message, "Exception Caught",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else
            {
                //If you do not set e.Handled to true, the application will close due to crash.

                LogManger.SendEmailExceptionToMe(e.Exception, "DispatcherUnhandledException");
                if (e.Exception.InnerException != null)
                {
                    LogManger.SendEmailExceptionToMe(e.Exception.InnerException, "DispatcherUnhandledException");
                    MessageBox.Show(e.Exception.InnerException.Message + e.Exception.InnerException.StackTrace, "Exception Caught",
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MessageBox.Show("Application is going to close. ", "Uncaught Exception");
                e.Handled = false;
            }            
        }

        [Conditional("Debug")]
        void _SetTestBuild()
        {
            BUILD = daisybrand.forecaster.Controlers.ViewModels.MainWindow.Build.Debug;
        }

        [Conditional("Release")]
        void _SetReleaseBuild()
        {
            BUILD = daisybrand.forecaster.Controlers.ViewModels.MainWindow.Build.Release;
        }
    }
}
