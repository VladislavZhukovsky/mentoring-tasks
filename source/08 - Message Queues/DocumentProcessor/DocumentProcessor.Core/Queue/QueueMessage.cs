using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessor.Core.Queue
{
    [Serializable]
    public class QueueMessage
    {
        public List<string> Files { get; set; }
        public int Attempt { get; set; }
        public int Id { get; set; }

        public QueueMessage()
        {
            Id = Math.Abs(Guid.NewGuid().GetHashCode());
        }

        public QueueMessage(IEnumerable<string> files): this()
        {
            Files = files.ToList();
            Attempt = 0;
        }

        public QueueMessage(IEnumerable<string> files, int @try): this(files)
        {
            //Files = files.ToList();
            Attempt = @try;
        }
    }
}
 