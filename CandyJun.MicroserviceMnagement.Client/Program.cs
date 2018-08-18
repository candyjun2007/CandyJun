using System;
using System.Runtime.InteropServices;

namespace CandyJun.MicroserviceMnagement.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine(OSPlatform.Linux);
                Console.WriteLine(RuntimeInformation.OSDescription);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine(OSPlatform.Windows);
                Console.WriteLine(RuntimeInformation.OSDescription);
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            Console.WriteLine("ping www.baidu.com & exit".Batch());
            Console.ReadKey();
        }
    }
}
