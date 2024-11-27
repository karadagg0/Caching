using System.Runtime.Caching;

public class CacheHelper
{
    private static ObjectCache cache = MemoryCache.Default;

    public static void AddToCache(string key, object value, TimeSpan? expirationTime = null)
    {
        
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cache key cannot be null or empty.");
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Cache value cannot be null.");
        }

       
        CacheItemPolicy policy = new CacheItemPolicy
        {
            AbsoluteExpiration = expirationTime.HasValue ? DateTime.Now.Add(expirationTime.Value) : DateTime.Now.AddMinutes(10),
            
            Priority = CacheItemPriority.Default
        };

        cache.Set(key, value, policy);
    }

    public static object GetFromCache(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cache key cannot be null or empty.");
        }

        return cache.Get(key);
    }

    public static void RemoveFromCache(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cache key cannot be null or empty.");
        }

        cache.Remove(key);
    }

    public static void ClearCache()
    {
        var cacheKeys = cache.Select(x => x.Key).ToList();
        foreach (var key in cacheKeys)
        {
            cache.Remove(key);
        }
    }

    public static void ListCacheKeys()
    {
        foreach (var item in cache)
        {
            Console.WriteLine(item.Key);
        }
    }

    public static bool IsKeyExists(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cache key cannot be null or empty.");
        }

        return cache.Contains(key);
    }

    public static object GetWithTimeout(string key, TimeSpan timeout)
    {
        var startTime = DateTime.Now;

        while (DateTime.Now - startTime < timeout)
        {
            var value = GetFromCache(key);
            if (value != null)
            {
                return value;
            }
            System.Threading.Thread.Sleep(100); 
        }

        return null; 
    }
}
