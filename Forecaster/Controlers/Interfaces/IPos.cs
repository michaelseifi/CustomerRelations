using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IPos
    {
        DateTime DATE { get; set; }
        //String DIVISION_NAME { get; set; }
        //string DIVISION_NUMBER { get; set; }
        decimal CASES { get; set; }
        DateTime FIRST_DAY_OF_WEEK { get; set; }
    }
}
