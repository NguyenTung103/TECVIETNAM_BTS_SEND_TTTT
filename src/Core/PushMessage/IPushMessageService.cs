using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.PushMessage
{
    public interface IPushMessageService
    {
        Task SendMessageAsync(string message);
    }
}
