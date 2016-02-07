using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessor.Core.Processors
{
    public class LoadImageException: Exception
    {
        public LoadImageException(string message):
            base(message)
        {
        }
    }
}
