using System;
using System.Collections.Generic;

namespace daisybrand.forecaster.Controlers.Enums
{
    public enum DState
    {
        None = 0,
        Check = 1,
        Confirmed = 2,

    }

    public class DStateItemString
    {
        public string VALUE { get; set; }
        public override string ToString()
        {
            return VALUE;
        }
    }
}
