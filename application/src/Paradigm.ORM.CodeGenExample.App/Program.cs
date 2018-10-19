using System;
using System.Threading.Tasks;

namespace Paradigm.ORM.CodeGenExample.App
{
    internal static class Program
    {
        private static async Task Main()
        {
            var startup = new Startup();
            startup.Configure();
            await startup.RunAsync();

            Console.ReadKey();
        }
    }
}
