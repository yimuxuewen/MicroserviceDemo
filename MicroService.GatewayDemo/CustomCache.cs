using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.GatewayDemo
{
    public class CustomCache : IOcelotCache<CachedResponse>
    {
        private class CacheDataModel
        {
            public CachedResponse CachedResponses { get; set; }

            public DateTime TimeOut { get; set; }

            public string Region { get; set; }
        }

        private static Dictionary<string, CacheDataModel> CustomCacheDictionary = new Dictionary<string, CacheDataModel>();


        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"This is {nameof(CustomCache)}--{nameof(Add)}");
            CustomCacheDictionary[key] = new CacheDataModel()
            {
                CachedResponses = value,
                TimeOut = DateTime.Now.Add(ttl),
                Region = region
            };
        }

        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"This is {nameof(CustomCache)}--{nameof(AddAndDelete)}");
            CustomCacheDictionary[key] = new CacheDataModel()
            {
                CachedResponses = value,
                TimeOut = DateTime.Now.Add(ttl),
                Region = region
            };
        }

        public void ClearRegion(string region)
        {
            Console.WriteLine($"This is {nameof(CustomCache)}--{nameof(ClearRegion)}");
            var keylist = CustomCacheDictionary.Where(kv => kv.Value.Region == region).Select(kv=>kv.Key);
            foreach (var key in keylist)
            {
                CustomCacheDictionary.Remove(key);
            }
        }

        public CachedResponse Get(string key, string region)
        {
            Console.WriteLine($"This is {nameof(CustomCache)}--{nameof(Get)}");
            if (CustomCacheDictionary.ContainsKey(key) && CustomCacheDictionary[key]!=null && CustomCacheDictionary[key].TimeOut>DateTime.Now && CustomCacheDictionary[key].Region==region)
            {
                return CustomCacheDictionary[key].CachedResponses;
            }
            else
            {
                return null;
            }
        }
    }
}
