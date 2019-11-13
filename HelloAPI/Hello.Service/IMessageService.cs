using Hello.Common.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Service
{
    public interface IMessageService
    {
        Task<IMessage> GetMessageById(int messageId, string url, CancellationToken cancellationToken);
        Task<IEnumerable<IMessage>> GetMessages(string url, CancellationToken cancellationToken);
    }
}
