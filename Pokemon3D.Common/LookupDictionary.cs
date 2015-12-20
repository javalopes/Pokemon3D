using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.Common
{
    public class LookupDictionary<TValue1, TValue2>
    {
        private readonly Dictionary<TValue1, TValue2> _convert; 
        private readonly Dictionary<TValue2, TValue1> _convertBack; 

        public LookupDictionary(IEnumerable<KeyValuePair<TValue1, TValue2>> pairs)
        {
            _convert = pairs.ToDictionary(p => p.Key, p => p.Value);
            _convertBack = _convert.ToDictionary(d => d.Value, d => d.Key);
        }

        public TValue2 Convert(TValue1 value)
        {
            return _convert[value];
        }

        public TValue1 Convert(TValue2 value)
        {
            return _convertBack[value];
        }
    }
}
