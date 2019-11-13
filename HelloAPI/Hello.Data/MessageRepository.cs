using System.Collections.Generic;
using System.Linq;
using Hello.Data.Interfaces;
using Hello.Common.Factories;
using Hello.Common.Models;

namespace Hello.Data
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        private IEnumerable<IMessage> messageList = new List<IMessage>
        {
            new Message { id = 1, display = "Hello World" },
            new Message { id = 2, display = "Hello Universe" },
            new Message { id = 3, display = "Hello America" }
        };

        public MessageRepository()
            : this(ModelFactory.Current)
        { }

        public MessageRepository(IModelFactory modelFactory)
            : base(modelFactory)
        { }

        public IEnumerable<IMessage> GetMessages()
        {
            return messageList;
        }

        public IMessage GetMessageById(int id)
        {
            return messageList.Where(p => p.id == id).FirstOrDefault(); 
        }
    }
}
