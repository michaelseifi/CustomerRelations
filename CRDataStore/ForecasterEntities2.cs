
namespace CRDataStore
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.Diagnostics;
    public partial class ForecasterEntities
    {
        public ForecasterEntities(string connection)
            : base("name=ForecasterEntities")
        {
            if (!string.IsNullOrEmpty(connection))
                this.Database.Connection.ConnectionString = connection;
            else
            {
                GetTestConnection();
                GetProdConnection();
            }

        }

        [Conditional("Debug"), Conditional("TestTest")]
        void GetTestConnection()
        {
            this.Database.Connection.ConnectionString 
                = @"Data source=DBIMRCMSRV02tst;initial catalog=Forecaster;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        }

        [Conditional("Release"), Conditional("TestProd"), Conditional("DevProd")]
        void GetProdConnection()
        {
            this.Database.Connection.ConnectionString 
                = @"Data source=DBIMRCMSRV02;initial catalog=Forecaster;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        }





    }
}
