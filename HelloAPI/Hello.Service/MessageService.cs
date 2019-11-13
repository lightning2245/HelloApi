using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hello.Common.Models;
using System.Collections.Generic;
using System;

namespace Hello.Service
{
    public class MessageService : IMessageService
    {  
        public async Task<IMessage> GetMessageById(int messageId, string url, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                UriBuilder uriBuilder = new UriBuilder($"{url}{messageId}");                

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
                { 
                    using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<Message>(content);
                    }
                }
            }
        }

        public async Task<IEnumerable<IMessage>> GetMessages(string url, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
                {                    
                    using (HttpResponseMessage response = await client.SendAsync(request, cancellationToken))
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<IEnumerable<Message>>(content);
                    }
                }
            }
        }
    }                        
}
