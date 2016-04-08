using System.Diagnostics;
using System.Configuration;
namespace CRDataStore.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    public sealed partial class Settings
    {

        public Settings()
        {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
             this.SetLiveApplicationSettings();
             this.SetTestApplicationSettings();
        }

        [Conditional("Release")]
        private void SetLiveApplicationSettings()
        {
            // Set the two Settings values to use the resource files designated
            // for the LIVE version of the app:
            this["DAX_PRODConnectionString"] = Resource1.Release_DAX_PRODConnectionString;
            this["ForecasterConnectionString"] = Resource1.Release_ForecasterConnectionString;
            //ConfigurationManager.ConnectionStrings["ForecasterEntities"].ConnectionString = Resource1.Release_ForecasterConnectionString;

            //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //conf.ConnectionStrings.ConnectionStrings.Remove("ForecasterEntities");
            //conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("ForecasterEntities",
            //    "metadata=res://*/ForecasterEntities.csdl|res://*/ForecasterEntities.ssdl|res://*/ForecasterEntities.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DBIMRCMSRV02;initial catalog=Forecaster;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"));
            //conf.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("ConnectionStrings");
        }


        [Conditional("Debug")]
        private void SetTestApplicationSettings()
        {
            // Set the two Settings values to use the resource files designated
            // for the TEST version of the app:
            this["DAX_PRODConnectionString"] = Resource1.Debug_DAX_PRODConnectionString;
            this["ForecasterConnectionString"] = Resource1.Debug_ForecaserConnectionString;

            //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //conf.ConnectionStrings.ConnectionStrings.Remove("ForecasterEntities");
            //conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("ForecasterEntities",
            //    "metadata=res://*/ForecasterEntities.csdl|res://*/ForecasterEntities.ssdl|res://*/ForecasterEntities.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DBIMRCMSRV02TST;initial catalog=Forecaster;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"));
            //conf.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("ConnectionStrings");

            //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //conf.ConnectionStrings.ConnectionStrings.Remove("DAX_PRODEntities");
            //conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DAX_PRODEntities",
            //    "metadata=res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.csdl|res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.ssdl|res://*/FORECASTER_PERFORMANCES_EXCEPTIONS.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=AXSQLVM01;initial catalog=DBIDAXTEST;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"));
            //conf.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("ConnectionStrings");

        }
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
