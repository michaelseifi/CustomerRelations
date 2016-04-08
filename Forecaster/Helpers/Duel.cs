using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Helpers
{
    public class Duel<T, J>
    {
        public Duel(){}
        public Duel(T key, J value)
        {
            KEY = key;
            VALUE = value;
        }
        #region properties
        public T KEY { get; set; }
        public J VALUE { get; set; }
        #endregion
    }

    public class DuelCollection<T, J> : List<Duel<T, J>>
    {
        public DuelCollection()
        {
            
        }
        public DuelCollection(int capacity)
            : base(capacity)
        {
            
        }
        public DuelCollection(IEnumerable<Duel<T, J>> collection)
            : base(collection)
        {
            
        }

    }
}
