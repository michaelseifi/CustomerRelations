using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRDataStore
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;

    public partial class DAX_PRODEntities
    {
        public DAX_PRODEntities(string connection)
            : base("name=DAX_PRODEntities")
        {
//#if Debug
//            this.Database.Connection.ConnectionString = @"Data Source=AXSQLVM01;Initial Catalog=DBIDAXTEST;Integrated Security=True;Connect Timeout=300";
//#endif
        }
    }
}
