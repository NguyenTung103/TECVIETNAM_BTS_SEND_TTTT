using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Logging
{
    /// <summary>
    /// Dịch vụ ghi log ra file
    /// Chú ý trước khi dùng phải load cấu hình trước. Thường đặt ở application start
    /// XmlConfigurator.Configure(LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy)), File.OpenRead("log4net.config"));
    /// </summary>
    public class Log4Service : ILoggingService
    {
        private readonly ILog logger;
        /// <summary>
        /// 
        /// </summary>
        public Log4Service()
        {
            logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Hàm ghi lỗi log dạng info
        /// </summary>
        /// <param name="message">Nội dung ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public void Info(string message, string fullMessage = "")
        {
            logger.Info(message + " " + fullMessage);
        }

        /// <summary>
        /// Hàm ghi lỗi log dạng info
        /// </summary>
        /// <param name="message">Nội dung ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public async Task InfoAsync(string message, string fullMessage = "")
        {
            await Task.Run(() => logger.Info(message + " " + fullMessage)).ConfigureAwait(false);
        }

        /// <summary>
        /// Hàm ghi lỗi log lỗi
        /// </summary>
        /// <param name="ex">Một error</param>
        public void Error(Exception ex)
        {
            logger.Error(ex);
        }

        /// <summary>
        /// Hàm ghi lỗi log lỗi
        /// </summary>
        /// <param name="ex">Một error</param>
        public async Task ErrorAsync(Exception ex)
        {
            await Task.Run(() => logger.Error(ex)).ConfigureAwait(false);
        }

        /// <summary>
        /// Hàm ghi cảnh báo trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public void Warn(string message, string fullMessage = "")
        {
            logger.Warn(message + " " + fullMessage);
        }

        /// <summary>
        /// Hàm ghi cảnh báo trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public async Task WarnAsync(string message, string fullMessage = "")
        {
            await Task.Run(() => logger.Warn(message + " " + fullMessage)).ConfigureAwait(false);
        }

        /// <summary>
        /// Hàm ghi Debug trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public void Debug(string message, string fullMessage = "")
        {
            logger.Debug(message + " " + fullMessage);
        }

        /// <summary>
        /// Hàm ghi Debug trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public async Task DebugAsync(string message, string fullMessage = "")
        {
            await Task.Run(() => logger.Debug(message + " " + fullMessage)).ConfigureAwait(false);
        }

        /// <summary>
        /// Hàm ghi Fatal trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public void Fatal(string message, string fullMessage = "")
        {
            logger.Fatal(message + " " + fullMessage);
        }

        /// <summary>
        /// Hàm ghi Fatal trên hệ thống
        /// </summary>
        /// <param name="message">Tiêu đề ngắn</param>
        /// <param name="fullMessage">Nội đầy đủ</param>
        public async Task FatalAsync(string message, string fullMessage = "")
        {
            await Task.Run(() => logger.Fatal(message + " " + fullMessage)).ConfigureAwait(false);
        }
    }
}
