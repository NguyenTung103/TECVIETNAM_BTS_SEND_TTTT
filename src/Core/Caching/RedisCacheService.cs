using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Caching
{
    public class RedisCacheService : IAsyncCacheService
    {
        #region Properties And Constructor

        /// <summary>
        /// Dịch vụ cache phân tán
        /// </summary>
        private readonly IDistributedCache _distributedCache;

        private readonly RedisCacheOptions _redisCacheOptions;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache distributedCache, IOptions<RedisCacheOptions> option, ILogger<RedisCacheService> logger)
        {
            GuardClauses.Null(option, nameof(option));

            _distributedCache = distributedCache;
            _redisCacheOptions = option.Value;
            _logger = logger;
        }

        #endregion Properties And Constructor

        #region Get Method
        public virtual async Task<DateTime> GetTime()
        {
            return await Task.FromResult(DateTime.Now);
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache và trả về với kiểu được chỉ định
        /// </summary>
        /// <typeparam name="TItem">Kiểu đối tượng cần nhận</typeparam>
        /// <param name="key">Khóa</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetByKeyAsync<TItem>(string key)
        {
            try
            {
                byte[] cacheData = await _distributedCache.GetAsync(key);
                if (cacheData != null)
                {
                    var dataString = Encoding.UTF8.GetString(cacheData);
                    return JsonSerializer.Deserialize<TItem>(dataString);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
            }
            return default;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            TItem cacheData = await GetByKeyAsync<TItem>(key);
            if (!EqualityComparer<TItem>.Default.Equals(cacheData, default))
            {
                return cacheData;
            }
            TItem funtionData = await factory();
            await SetValueAsync(key, funtionData);
            return funtionData;
        }
        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        public virtual TItem GetOrCreate<TItem>(string key, Func<TItem> factory, int time)
        {
            TItem cacheData = GetByKey<TItem>(key);
            if (!EqualityComparer<TItem>.Default.Equals(cacheData, default))
            {
                return cacheData;
            }
            TItem funtionData = factory();
            var data = SetValueAsync(key, funtionData, time).Result;
            return funtionData;
        }
        private TItem GetByKey<TItem>(string key)
        {
            byte[] cacheData = _distributedCache.Get(key);
            if (cacheData != null)
            {
                var dataString = Encoding.UTF8.GetString(cacheData);
                return JsonSerializer.Deserialize<TItem>(dataString);
            }
            return default;
        }
        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <param name="time">Thời gian tồn tại cache tính bắng giây</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, int time)
        {
            GuardClauses.Null(factory, nameof(factory));

            TItem cacheData = await GetByKeyAsync<TItem>(key);
            if (!EqualityComparer<TItem>.Default.Equals(cacheData, default))
            {
                return cacheData;
            }
            TItem funtionData = await factory();
            await SetValueAsync(key, funtionData, time);
            return funtionData;
        }

        #endregion Get Method

        #region Set Method

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <returns>TItem</returns>
        public virtual async Task<TItem> SetValueAsync<TItem>(string key, TItem value)
        {
            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
            return value;
        }

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <param name="time">Thời gian lưu trong cache tính bằng giây</param>
        /// <returns>Trả lại đối tượng TItem</returns>
        public virtual async Task<TItem> SetValueAsync<TItem>(string key, TItem value, int time)
        {
            try
            {
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(time));
                await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)), options);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
            }
            return default;
        }

        #endregion Set Method

        #region Remove Method

        /// <summary>
        /// Gỡ bỏ cache theo khóa cache bất đồng bộ
        /// </summary>
        /// <param name="key">Khóa cache</param>
        public virtual async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        /// <summary>
        /// Gõ bỏ cache theo mẫu khóa cache bất đồng bộ
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        public virtual async Task RemoveByPrefixAsync(string pattern)
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(_redisCacheOptions.Configuration);
            foreach (System.Net.EndPoint endPoints in connectionMultiplexer.GetEndPoints())
            {
                IServer server = connectionMultiplexer.GetServer(endPoints);
                IDatabase db = connectionMultiplexer.GetDatabase();
                IEnumerable<RedisKey> keys = server.Keys(database: db.Database, pattern: $"{_redisCacheOptions.InstanceName}{pattern}*");
                await db.KeyDeleteAsync(keys.ToArray());
            }
        }

        /// <summary>
        /// Gõ bỏ cache theo mẫu khóa cache
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        public virtual void RemoveByPattern(string pattern)
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(_redisCacheOptions.Configuration);
            foreach (System.Net.EndPoint endPoints in connectionMultiplexer.GetEndPoints())
            {
                IServer server = connectionMultiplexer.GetServer(endPoints);
                IDatabase db = connectionMultiplexer.GetDatabase();
                IEnumerable<RedisKey> keys = server.Keys(database: db.Database, pattern: $"{_redisCacheOptions.InstanceName}{pattern}*");
                db.KeyDeleteAsync(keys.ToArray());
            }
        }

        /// <summary>
        /// Xóa hết dữ liệu cache
        /// </summary>
        public virtual async Task ClearAsync()
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(_redisCacheOptions.Configuration);
            foreach (System.Net.EndPoint endPoints in connectionMultiplexer.GetEndPoints())
            {
                IServer server = connectionMultiplexer.GetServer(endPoints);
                IDatabase db = connectionMultiplexer.GetDatabase();

                IEnumerable<RedisKey> keys = server.Keys(database: db.Database);
                await db.KeyDeleteAsync(keys.ToArray());
            }
        }
        #endregion Remove Method

        #region Get all cacheKey
        public Task<IEnumerable<string>> GetAllKeys()
        {
            List<string> lstKey = new List<string>();
            return Task.FromResult<IEnumerable<string>>(null);
        }
        #endregion
    }
}