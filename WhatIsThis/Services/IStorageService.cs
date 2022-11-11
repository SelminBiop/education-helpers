using System;
using System.Text.Json;

namespace WhatIsThis.Services
{
	public interface IAssociationStorageService
	{
        void Add(string key, Association value);
        void Remove(string key, Association value);
        IEnumerable<Association> Get(string key);
        void Set(string key, IEnumerable<Association> value);
    }

    internal sealed class DeviceStorageService : IAssociationStorageService
    {
        public void Add(string key, Association value)
        {
            var storedCollection = Get(key);

            var mutableCollection = storedCollection.ToList();
            mutableCollection.Add(value);
            Set(key, mutableCollection);
        }

        public void Remove(string key, Association value)
        {
            var storedCollection = Get(key);
            var mutableCollection = storedCollection.ToList();
            if (mutableCollection.Remove(value))
            {  
                Set(key, mutableCollection);
            }
        }

        public IEnumerable<Association> Get(string key)
        {
            var storedJson = Preferences.Get(key, null);
            if (!string.IsNullOrEmpty(storedJson))
            {
                return JsonSerializer.Deserialize<IList<Association>>(storedJson);
            }
            return new List<Association>();
        }

        public void Set(string key, IEnumerable<Association> value)
        {
            var valueSerializedInJson = JsonSerializer.Serialize<IEnumerable<Association>>(value);
            Preferences.Set(key, valueSerializedInJson);
        }
    }
}

