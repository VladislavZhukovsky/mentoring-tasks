using System.ServiceProcess;

namespace DocumentProcessor.ProcessorService
{
    class Program
    {
        static void Main(string[] args)
        {
            var workingFolder =  @"D:\DocumentProcessor\Destination";
            var documentFolder = @"D:\DocumentProcessor\Docs";
            ServiceBase.Run(new DocumentProcessor(workingFolder, documentFolder));
        }
    }
}
