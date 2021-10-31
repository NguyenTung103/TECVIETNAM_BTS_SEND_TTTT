using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BtsGetwayService.Core
{
    public class DapperBaseData<Entity> where Entity : class
    {
        protected readonly string connectionString = "data source=14.177.239.150,1433;initial catalog=Observation;user id=sa;password=tecvietnam@1234";

        public DapperBaseData()
        {
            
        }

        /// <summary>
        /// Tìm đối tượng theo khóa chính đầu tiên
        /// </summary>
        /// <param name="pk">Giá trị khóa chính</param>
        /// <returns>Entity</returns>
        public virtual Entity FindByKey(dynamic pk)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return SqlMapperExtensions.Get<Entity>(connection, pk);
            }
        }

        /// <summary>
        /// Tìm đối tượng theo khóa chính đầu tiên
        /// </summary>
        /// <param name="pk">Giá trị khóa chính</param>
        /// <returns>Entity</returns>
        public virtual async Task<Entity> FindByKeyAsync(dynamic pk)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await SqlMapperExtensions.GetAsync<Entity>(connection, pk).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thêm mới một entity
        /// </summary>
        /// <param name="entity">đối tượng cần thêm mới</param>
        /// <returns>bool</returns>
        public virtual bool Insert(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                long updateInfor = connection.Insert(entity);
                return updateInfor > 0;
            }
        }

        /// <summary>
        /// Thêm mới một entity bất đồng bộ
        /// </summary>
        /// <param name="entity">đối tượng cần thêm mới</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertAsync(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.InsertAsync(entity).ConfigureAwait(false) > 0;
            }
        }

        /// <summary>
        /// Cập nhập một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>bool</returns>
        public virtual bool Update(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Update(entity);
            }
        }

        /// <summary>
        /// Cập nhập một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>bool</returns>
        public virtual async Task<bool> UpdateAsync(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.UpdateAsync(entity).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng thật cần xóa</param>
        /// <returns>bool</returns>
        public virtual bool Delete(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Delete(entity);
            }
        }

        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng thật cần xóa</param>
        /// <returns>bool</returns>
        public virtual async Task<bool> DeleteAsync(Entity entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.DeleteAsync(entity).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách 1 bảng
        /// </summary>
        /// <returns>IEnumerable Entity</returns>
        public virtual IEnumerable<Entity> FindAll()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.GetAll<Entity>();
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách 1 bảng
        /// </summary>
        /// <returns>IEnumerable Entity</returns>
        public virtual async Task<IEnumerable<Entity>> FindAllAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.GetAllAsync<Entity>().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Goi thủ tục và trả về một list data
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="spName">Tên sp</param>
        /// <param name="param">Tham số</param>
        /// <returns>IEnumerable Entity</returns>
        public virtual IEnumerable<T> CallProcedure<T>(string spName, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(spName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Goi thủ tục và trả về một list data
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="spName">Tên sp</param>
        /// <param name="param">Tham số</param>
        /// <returns>IEnumerable Entity</returns>
        public virtual async Task<IEnumerable<T>> CallProcedureAsync<T>(string spName, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>(spName, param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual Entity QueryFirstOrDefault(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Entity>(query, param);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// Chạy bất đồng bộ
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual async Task<Entity> QueryFirstOrDefaultAsync(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Entity>(query, param).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual IEnumerable<Entity> Query(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Entity>(query, param);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách Entity, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// chạy bất đồng bộ
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual async Task<IEnumerable<Entity>> QueryAsync(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Entity>(query, param).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chạy một cấu query với tham số truyền vào
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Thám số</param>
        /// <returns>int</returns>
        public virtual int Execute(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Execute(query, param);
            }
        }

        /// <summary>
        /// Chạy một cấu query với tham số truyền vào
        /// </summary>
        /// <param name="query">Câu query</param>
        /// <param name="param">Thám số</param>
        /// <returns>int</returns>
        public virtual async Task<int> ExecuteAsync(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.ExecuteAsync(query, param).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual IEnumerable<T> Query<T>(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(query, param);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// chạy bất đồng bộ
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>(query, param).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual T QueryFirstOrDefault<T>(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(query, param);
            }
        }

        /// <summary>
        /// Chạy cầu query và trả về danh sách đối tượng, hỗ trợ trả về các lớp mở rộng rộng hoặc lớp tối giản so với lớp chuẩn Entity
        /// Chạy bất đồng bộ
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng được trả về</typeparam>
        /// <param name="query">Câu query</param>
        /// <param name="param">Danh sách tham số</param>
        /// <returns>T</returns>
        public virtual async Task<T> QueryFirstOrDefaultAsync<T>(string query, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<T>(query, param).ConfigureAwait(false);
            }
        }
    }
}
