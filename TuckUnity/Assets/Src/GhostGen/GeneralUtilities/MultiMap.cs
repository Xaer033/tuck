using System.Collections;
using System.Collections.Generic;

namespace GhostGen
{
    public class MultiMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, List<TValue>> _dict = new Dictionary<TKey, List<TValue>>();

        public void Add(TKey key, TValue value)
        {
            if(!_dict.ContainsKey(key))
                _dict[key] = new List<TValue>();

            _dict[key].Add(value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach(var list in _dict)
                foreach(var value in list.Value)
                    yield return new KeyValuePair<TKey, TValue>(list.Key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
