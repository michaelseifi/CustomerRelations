using System;
using System.Collections.Generic;
using daisybrand.forecaster.Controlers.Enums;

namespace daisybrand.forecaster.Controlers.Enums
{
    public enum MState
    {
        Confirmed = 0,
        DidNotKnow = 1
    }

    public class MStateItemString
    {
        public string VALUE { get; set; }
        public override string ToString()
        {
            return VALUE;
        }
    }
}
