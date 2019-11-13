using System.Collections.Generic;
using Hello.Common.Models;

namespace Hello.Data.Interfaces
{
    public interface IMessageRepository
    {
        IEnumerable<IMessage> GetMessages();

        IMessage GetMessageById(int id);        
    }
}
