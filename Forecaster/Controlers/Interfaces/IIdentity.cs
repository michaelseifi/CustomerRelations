using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    interface IIdentity
    {
        Guid SessionId { get; }
        bool IsReadOnly { get; }
        bool IsAdmin { get; }
        bool IsAuthenticated { get; }
        string NetWorkAlias { get; }
        string Damian { get;  }      

    }
}
