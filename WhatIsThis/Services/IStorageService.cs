using System.Text.Json;
using WhatIsThis.Data;

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
            var defaultReturnValue = new List<Association>();
            if (!string.IsNullOrEmpty(storedJson))
            {
                return JsonSerializer.Deserialize<IList<Association>>(storedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? defaultReturnValue;
            }
            return defaultReturnValue;
        }

        public void Set(string key, IEnumerable<Association> value)
        {
            var valueSerializedInJson = JsonSerializer.Serialize<IEnumerable<Association>>(value);
            Preferences.Set(key, valueSerializedInJson);
        }
    }
}

