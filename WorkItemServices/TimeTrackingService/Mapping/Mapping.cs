using System.Collections.Generic;
using System.Linq;

namespace TimeTrackingService.Mapping
{
    public class Mapping<T1, T2> : Dictionary<T1, T2>
    {
        public T1 this[T2 index] => this.First(x => x.Value.Equals(index)).Key;
    }
}
