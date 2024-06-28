using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.TValle.Tools.Runtime
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public IReadOnlyList<TKey> serializedKeys => keys;
        public IReadOnlyList<TValue> serializedValues => values;


        public SerializableDictionary()
        {

        }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }
        public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public bool TryAddInmediate(TKey key, TValue value)
        {
            if(TryAdd(key, value))
            {
                keys.Add(key);
                values.Add(value);
                return true;
            }
            return false;
        }

        public void LoadFrom(IReadOnlyDictionary<TKey, TValue> source)
        {
            keys = new List<TKey>();
            values = new List<TValue>();
            foreach(var par in source)
            {
                keys.Add(par.Key);
                values.Add(par.Value);
            }
        }
        public void LoadFrom(IDictionary<TKey, TValue> source)
        {
            keys = new List<TKey>();
            values = new List<TValue>();
            foreach(var par in source)
            {
                keys.Add(par.Key);
                values.Add(par.Value);
            }
        }
        public void SaveTo(IDictionary<TKey, TValue> result)
        {
            result.Clear();
            keys = new List<TKey>();
            values = new List<TValue>();
            var c = Mathf.Min(keys.Count, values.Count);
            for(int i = 0; i < c; i++)
            {
                result.Add(keys[i], values[i]);
            }

        }

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach(KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if(keys.Count != values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for(int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }


    [System.Serializable]
    sealed public class AssetReferenceDictionary : SerializableDictionary<string, AssetReference>
    {
        public AssetReferenceDictionary()
        {
        }

        public AssetReferenceDictionary(IEqualityComparer<string> comparer) : base(comparer)
        {
        }


    }

}
