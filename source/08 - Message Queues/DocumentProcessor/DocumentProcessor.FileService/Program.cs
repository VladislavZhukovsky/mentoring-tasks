using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace DocumentProcessor.FileService
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDir = @"D:\DocumentProcessor\Source";
            string dstDir =    @"D:\DocumentProcessor\Destination";

            ServiceBase.Run(new FileProcessor(sourceDir, dstDir));
        }
    }
}
