using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Dict<K, V> : Dictionary<K, Dictionary<K, V>>
    {
        public Dict()
        {

        }

        public void Add(K key, K key2, V value){
            if (ContainsKey(key))
            {
                if (!this[key].ContainsKey(key2))
                    this[key].Add(key2, value);
            }
            else
            {
                Dictionary<K, V> dic = new Dictionary<K, V>();
                dic.Add(key2, value);
                Add(key, dic);
            }                
        }
    }
}
