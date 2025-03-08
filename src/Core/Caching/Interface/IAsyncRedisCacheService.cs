using System;
using System.Threading.Tasks;

namespace Core.Caching
{
    public interface IAsyncRedisCacheService
    {
        #region Get Method              
        /// <summary>
        /// Lấy đối tượng từ khóa cache và trả về với kiểu được chỉ định
        /// </summary>
        /// <typeparam name="TItem">Kiểu đối tượng cần nhận</typeparam>
        /// <param name="key">Khóa</param>
        /// <returns>TItem object</returns>
        Task<TItem> GetByKeyAsync<TItem>(string key);

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        TItem GetOrCreate<TItem>(string key, Func<TItem> factory, int time);
        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory);

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <param name="time">Thời gian tồn tại cache tính bắng giây</param>
        /// <returns>TItem object</returns>
        Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, int time);

        #endregion Get Method

        #region Set Method

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <returns>TItem</returns>
        Task<TItem> SetValueAsync<TItem>(string key, TItem value);

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <param name="time">Thời gian lưu trong cache tính bằng giây</param>
        /// <returns>Trả lại đối tượng TItem</returns>
        Task<TItem> SetValueAsync<TItem>(string key, TItem value, int time);

        #endregion Set Method

        #region Remove Method

        /// <summary>
        /// Gỡ bỏ cache theo khóa cache bất đồng bộ
        /// </summary>
        /// <param name="key">Khóa cache</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Gõ bỏ cache theo tiền tố
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        Task RemoveByPrefixAsync(string pattern);

        /// <summary>
        /// Xóa hết dữ liệu cache
        /// </summary>
        Task ClearAsync();
        #endregion Remove Method
    }
}