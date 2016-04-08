using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Helpers;
namespace daisybrand.forecaster.Exceptions
{
    public class ItemNotInsertedException:ApplicationException
    {
        public ItemNotInsertedException(Exception ex)
            : base(ex.Message)
        {
            LogManger.Insert(ex);
            LogManger.RaiseErrorMessage(new Controlers.Objects.Message { MESSAGE = ex.Message });
        }

    }
}
