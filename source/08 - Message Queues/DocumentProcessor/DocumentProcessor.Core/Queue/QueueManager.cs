using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using DocumentProcessor.Core;

namespace DocumentProcessor.Core.Queue
{
    public class QueueManager: IDisposable
    {
        private static string QUEUE_NAME = @".\Private$\DocumentProcessorQueue";

        private MessageQueue queue;

        public QueueManager()
        {
            InitializeQueue();
        }

        public void SendMessage(QueueMessage message)
        {
            queue.Send(message);
        }

        public QueueMessage ReceiveMessage()
        {
            if (queue.GetMessageEnumerator2().MoveNext())
            {
                var message = queue.Peek(TimeSpan.FromSeconds(10));
                var queueMessage = (QueueMessage)message.Body;
                queue.Receive();
                return queueMessage;
            }
            return null;
        }

        public void ReceiveById(string id)
        {
            queue.ReceiveById(id);
        }

        private void InitializeQueue()
        {
            if (MessageQueue.Exists(QUEUE_NAME))
            {
                queue = new MessageQueue(QUEUE_NAME);
            }
            else
            {
                queue = MessageQueue.Create(QUEUE_NAME);
            }
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(QueueMessage) });
        }

        /// <summary>
        /// Finishes work with the queue
        /// </summary>
        public void Dispose()
        {
            if (queue != null)
                queue.Close();
        }
    }
}
