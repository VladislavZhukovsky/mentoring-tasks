using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessor.Core.Processors
{
    public interface IProcessor
    {
        ProcessingResultEntry Process(IEnumerable<string> files, string workingFolder, string documentFolder);
    }
}
