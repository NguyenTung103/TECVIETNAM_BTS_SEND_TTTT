using System.Collections.Generic;
using System.Threading.Tasks;

namespace BtsGetwayService.Core
{
    public interface IDapperBaseData<Entity> where Entity : class
    {
        /// <summary>
        /// Tìm đối tượng theo khóa chính đầu tiên
        /// </summary>
        /// <param name="pk">Giá trị khóa chính</param>
        /// <returns>Entity</returns>
        Entity FindByKey(dynamic pk);

        /// <summary>
        /// Tìm đối tượng theo khóa chính đầu tiên
        /// </summary>
        /// <param name="pk">Giá trị khóa chính</param>
        /// <returns>Entity</returns>
        Task<Entity> FindByKeyAsync(dynamic pk);

        /// <summary>
        /// Thêm mới một entity
        /// </summary>
        /// <param name="entity">đối tượng cần thêm mới</param>
        /// <returns>bool</returns>
        bool Insert(Entity entity);

        /// <summary>
        /// Thêm mới một entity bất đồng bộ
        /// </summary>
        /// <param name="entity">đối tượng cần thêm mới</param>
        /// <returns>bool</returns>
        Task<bool> InsertAsync(Entity entity);

        /// <summary>
        /// Cập nhập một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>bool</returns>
        bool Update(Entity entity);

        /// <summary>
        /// Cập nhập một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>bool</returns>
        Task<bool> UpdateAsync(Entity entity);

        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng thật cần xóa</param>
        /// <returns>bool</returns>
        bool Delete(Entity entity);

        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng thật cần xóa</param>
        /// <returns>bool</returns>
        Task<bool> DeleteAsync(Entity entity);

        /// <summary>
        /// Lấy toàn bộ danh sách 1 bảng
        /// </summary>
        /// <returns>IEnumerable Entity</returns>
        IEnumerable<Entity> FindAll();

        /// <summary>
        /// Lấy toàn bộ danh sách 1 bảng
        /// </summary>
        /// <returns>IEnumerable Entity</returns>
        Task<IEnumerable<Entity>> FindAllAsync();

        /// <summary>
        /// Goi thủ tục và trả về một list data
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="spName">Tên sp</param>
        /// <param name="param">Tham số</param>
        /// <returns>IEnumerable Entity</returns>
        IEnumerable<T> CallProcedure<T>(string spName, object param = null);

        /// <summary>
        /// Goi thủ tục và trả về một list data
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="spName">Tên sp</param>
        /// <param name="param">Tham số</param>
        /// <returns>IEnumerable Entity</returns>
        Task<IEnumerable<T>> CallProcedureAsync<T>(string spName, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        Entity QueryFirstOrDefault(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// Chạy bất đồng bộ
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        Task<Entity> QueryFirstOrDefaultAsync(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        IEnumerable<Entity> Query(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// chạy bất đồng bộ
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        Task<IEnumerable<Entity>> QueryAsync(string query, object param = null);

        /// <summary>
        /// Chạy một cấu query với tham số truyền vào
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Thám số</param>
        /// <returns>int</returns>
        int Execute(string query, object param = null);

        /// <summary>
        /// Chạy một cấu query với tham số truyền vào
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Thám số</param>
        /// <returns>int</returns>
        Task<int> ExecuteAsync(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        IEnumerable<T> Query<T>(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// chạy bất đồng bộ
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        T QueryFirstOrDefault<T>(string query, object param = null);

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// Chạy bất đồng bộ
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string query, object param = null);
    }
}
