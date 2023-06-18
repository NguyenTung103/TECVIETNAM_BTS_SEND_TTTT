using Core.Entities;
using System;

namespace Core.Interfaces
{
    public interface IMasterMessageQueueService
    {
        /// <summary>
        /// Xuất bản một đầu việc update thông tin bộ nhớ
        /// </summary>
        /// <param name="queueMessage">Nội dung dầu việc</param>
        void BroadcastUpdateMemoryTask(QueueMessage queueMessage);

        /// <summary>
        /// Lắng nghe một đầu việc
        /// </summary>
        /// <param name="func">fuction xử lý</param>
        void SubscribeUpdateMemoryTask(Action<QueueMessage> func);

        /// <summary>
        /// Xóa toàn bộ các tài nguyên
        /// </summary>
        void Dispose();
    }
}