using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace LibraryHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(LibraryAppService.LibraryService));
            //libraryServiceHost.Opened += Host_Opened;
            host.Faulted += Host_Faulted;
            host.Open();
            Console.WriteLine("Library opened");
            Console.ReadKey();
        }

        private static void Host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Error");
        }
    }
}
