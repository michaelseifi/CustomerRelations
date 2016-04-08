using System;
using System.Collections.Generic;
using System.Linq;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Message:IMessage
    {

        #region IMessage Members
        public string MESSAGE { get; set; }

        #endregion
    }
}
