using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Objects;
namespace daisybrand.forecaster.Controlers.Interfaces
{
    public interface IField<T>
    {
        string WEEK_ID { get; set; }
        T VALUE { get; set; }
        Comment COMMENT { get; set; }
        string TOOLTIP { get; }
    }
}
