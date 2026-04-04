using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace PBT.CacheService
{

    public interface IAppCacheService
    {
        void SetCacheValue<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);
        bool TryGetCacheValue<T>(string key, out T value);
            T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory);
        void RemoveCacheValue(string key);
        void ClearCache();
    }

    public class AppCacheService : IAppCacheService
    {
        private readonly IMemoryCache _cache;
        // Token source để clear toàn bộ cache một cách an toàn
        private CancellationTokenSource _resetToken = new CancellationTokenSource();

        public AppCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetCacheValue<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(60),

            };
            cacheEntryOptions.SetSize(size: 1); // Đặt kích thước cho entry, cần thiết nếu cache có giới hạn kích thước

            // Liên kết entry với reset token
            cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(_resetToken.Token));

            _cache.Set(key, value, cacheEntryOptions);
        }

        public bool TryGetCacheValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        // Sử dụng GetOrCreate có sẵn nhưng lưu ý về việc cấu hình options
        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
        {
            return _cache.GetOrCreate(key, entry =>
            {
                entry.AddExpirationToken(new CancellationChangeToken(_resetToken.Token));
                return factory(entry);
            });
        }

        public void RemoveCacheValue(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Xóa toàn bộ Cache bằng cách trigger CancellationToken
        /// </summary>
        public void ClearCache()
        {
            if (_resetToken != null && !_resetToken.IsCancellationRequested && _resetToken.Token.CanBeCanceled)
            {
                _resetToken.Cancel();
                _resetToken.Dispose();
            }

            // Khởi tạo lại token mới cho các lần set cache tiếp theo
            _resetToken = new CancellationTokenSource();
        }
    }
}
