using Hello.Service;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Application
{
    class Program
    {
        private static readonly string baseMessageurl = ConfigurationManager.AppSettings["messageApiUrl"];
        
        static async Task Main(string[] args)
        {           
            await GetHelloWorldMessage();
        }

        private static async Task GetHelloWorldMessage()
        {
            IMessageService msgService = new MessageService();
            
            using (CancellationTokenSource cancellationToken = new CancellationTokenSource())
            {                
                var result = await msgService.GetMessageById(1, baseMessageurl, cancellationToken.Token);               
                
                Console.WriteLine(result.display);
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
            }

        }
    }
}
