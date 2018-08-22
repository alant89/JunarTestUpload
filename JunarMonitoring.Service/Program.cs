using ServiceProcess.Helpers;
using System.ServiceProcess;

namespace JunarMonitoring.Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServicesToRun.LoadServices();
        }
    }
}
