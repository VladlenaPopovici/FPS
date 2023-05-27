using System;
using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new();

        [SerializeField] private List<TValue> values = new();

        // Unity callback before serialization
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in this)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        // Unity callback after deserialization
        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
            {
                throw new Exception(
                    $"The number of keys ({keys.Count}) does not match the number of values ({values.Count}).");
            }

            for (var i = 0; i < keys.Count; i++)
            {
                this[keys[i]] = values[i];
            }
        }
    }
}