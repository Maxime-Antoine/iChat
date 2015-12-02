using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iChat.HostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(iChat.Service.ChatService)))
            {
                host.Open();
                Console.WriteLine("Service running...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
