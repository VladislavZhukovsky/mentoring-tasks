using DocumentProcessor.Core;
using DocumentProcessor.Core.Processors;
using DocumentProcessor.Core.Queue;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentProcessor.ProcessorService
{
    public class DocumentProcessor : ServiceBase
    {
        private const string SERVICE_NAME = "DocumentProcessor";
        private const string FAILED_FOLDER_NAME = "Failed";

        private Thread workThread;
        private ManualResetEvent stopWorkEvent = new ManualResetEvent(false);
        private QueueManager queueManager;
        private string workingFolder;
        private string documentFolder;
        private string failedFolder;
        private Logger logger;
        private List<Task> tasks; //container for all executing processing tasks

        public DocumentProcessor(string workingFolder, string documentFolder)
        {
            this.CanStop = true;
            this.ServiceName = SERVICE_NAME;
            this.AutoLog = false;

            this.workingFolder = workingFolder;
            this.documentFolder = documentFolder;
            this.failedFolder = Path.Combine(workingFolder, FAILED_FOLDER_NAME);
            Directory.CreateDirectory(workingFolder);
            Directory.CreateDirectory(documentFolder);
            Directory.CreateDirectory(failedFolder);
            logger = LogManager.GetCurrentClassLogger();
            tasks = new List<Task>();
            workThread = new Thread(WorkProcedure);
        }

        /// <summary>
        /// On service start:
        /// starts workingThread
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            logger.Info("=====Starting service...");
            stopWorkEvent.Reset();
            workThread.Start();
        }

        /// <summary>
        /// On service stop:
        /// set stopWorkEvent to stop the working thread
        /// wait the end of working thread
        /// </summary>
        protected override void OnStop()
        {
            logger.Info("Stopping service...");
            stopWorkEvent.Set();
            workThread.Join();
            logger.Info("=====Finish");
        }

        /// <summary>
        /// Scans working folder every 7 seconds
        /// starts processing tasks that could be executed simultaneously
        /// Receives messages from the queue
        /// </summary>
        private void WorkProcedure()
        {
            try
            {
                //using for IDisposable QueueManager
                using (queueManager = new QueueManager())
                {
                    do
                    {
                        if (stopWorkEvent.WaitOne(TimeSpan.Zero))
                        {
                            continue;
                        }
                        logger.Info("Receiving message...");
                        var message = queueManager.ReceiveMessage();
                        if (message != null)
                        {
                            logger.Info("Processing message with id {0}", message.Id);
                            var task = new Task(() => ProcessMessage(message)); //multi-threading WOW!!!
                            tasks.Add(task);
                            task.Start();
                        }
                        else
                        {
                            logger.Info("Queue is empty");
                        }
                        //remove all completed tasks
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                    while (!stopWorkEvent.WaitOne(TimeSpan.FromSeconds(7)));
                    //wait for all processing tasks before service stop
                    logger.Info("Waiting for processing documents");
                    Task.WaitAll(tasks.ToArray(), TimeSpan.FromMinutes(1).Milliseconds);
                    logger.Info("All documents were processed");
                }
            }
            catch(Exception ex)
            {
                logger.Fatal("Fatal error occured in main thread");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// Processes message
        /// logBuilder for output whole log as one
        /// If message processed successfully, delete files
        /// If message failed, moves files from invalid chain to Failed folder
        /// </summary>
        /// <param name="message"></param>
        private void ProcessMessage(QueueMessage message)
        {
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine(string.Format("INFO | {0} | Start processing message with id {1}", DateTime.Now.ToLongDateString(), message.Id));
            if (message.Files != null)
            {
                IProcessor processor = new PdfProcessor();
                var result = processor.Process(message.Files, workingFolder, documentFolder);
                logBuilder.AppendLine(string.Format("INFO | {0} | Document processor log", DateTime.Now.ToLongDateString()));
                logBuilder.AppendLine(result.Log);
                if (result.Result == ProcessingResult.Failed)
                {
                    logBuilder.AppendLine(string.Format("WARN | {0} | Message {1} processing failed", DateTime.Now.ToLongDateString(), message.Id));
                    if (message.Attempt < 3)
                    {
                        message.Attempt++;
                        logBuilder.AppendLine(string.Format("INFO | {0} |  Return message {1} to queue; attempt: {2}", DateTime.Now.ToLongDateString(), message.Id, message.Attempt));
                        queueManager.SendMessage(message);
                    }
                    else
                    {
                        logBuilder.AppendLine(string.Format("ERROR | Message {0} failed, moving to {1} folder", message.Id, FAILED_FOLDER_NAME));
                        var failedPath = MoveToFailed(message.Files);
                        logBuilder.AppendLine(string.Format("INFO | Message {0} failed, moved to {1} folder", message.Id, failedPath));
                    }
                }
                if (result.Result == ProcessingResult.Success)
                {
                    logBuilder.AppendLine(string.Format("INFO | Message {0} processed successfully", message.Id));
                    logBuilder.AppendLine(string.Format("INFO | Deleting files"));
                    DeleteFiles(message.Files);
                    logBuilder.AppendLine(string.Format("INFO | Files deleted"));
                }
            }
            else
            {
                logBuilder.AppendLine(string.Format("INFO | No files in message {0}", message.Id));
            }
            logBuilder.AppendLine(string.Format("INFO | {0} |  Processing message {1} finished", DateTime.Now.ToLongDateString(), message.Id));
            logger.Info(logBuilder.ToString());
        }

        private string MoveToFailed(IEnumerable<string> files)
        {
            var chainFolderName = Guid.NewGuid().ToString();
            var chainFolderPath = Path.Combine(workingFolder, failedFolder, chainFolderName);
            Directory.CreateDirectory(chainFolderPath);
            foreach (var file in files)
            {
                var filepath = Path.Combine(workingFolder, file);
                if (File.Exists(filepath))
                    File.Move(filepath, Path.Combine(chainFolderPath, file));
            }
            return chainFolderPath;
        }

        private void DeleteFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var filepath = Path.Combine(workingFolder, file);
                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
        }
    }
}
