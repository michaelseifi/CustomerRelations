using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Exceptions
{
    public class DeleteException:ApplicationException
    {
        public DeleteException(Exception ex)
            : base(ex.Message)
        {
            LogManger.Insert(ex);
            LogManger.RaiseErrorMessage(new Message { MESSAGE = ex.Message });
        }

        public DeleteException(string message)
            : base(message)
        {
            LogManger.RaiseErrorMessage(new Message { MESSAGE = message });
        }
    }
}
