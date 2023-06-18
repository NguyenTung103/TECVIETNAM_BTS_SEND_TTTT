using Core.Entities;
using System;

namespace Core.Interfaces
{
    public interface IWorkerMessageQueueService
    {
        /// <summary>
        /// Xuất bản một đầu việc
        /// </summary>
        /// <param name="queueMessage">Nội dung đầu việc</param>
        void PublishWorkerTask(QueueMessage queueMessage);

        /// <summary>
        /// Nhận một đầu việc
        /// </summary>
        /// <param name="func">Hàm xử lý đầu việc</param>
        void SubscribeWorkerTask(Action<QueueMessage> func);

        /// <summary>
        /// Xóa toàn bộ tài nguyên
        /// </summary>
        void Dispose();
    }
}