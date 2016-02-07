using DocumentProcessor.Core.Queue;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace DocumentProcessor.FileService
{
    public class FileProcessor: ServiceBase
    {
        private const string SERVICE_NAME = "FileProcessor";
        private const int MAX_CHAIN_CAPACITY = 3;

        private Thread workThread;
        private ManualResetEvent stopWorkEvent = new ManualResetEvent(false);
        private AutoResetEvent sourceDirectoryChangedEvent = new AutoResetEvent(false);
        private string sourcePath;
        private string destinationPath;
        private Logger logger;

        private FileSystemWatcher fileWatcher;

        public FileProcessor(string inRootPath, string outRootPath)
        {
            this.CanStop = true;
            this.ServiceName = SERVICE_NAME;
            this.AutoLog = false;

            sourcePath = inRootPath;
            destinationPath = outRootPath;
            workThread = new Thread(WorkProcedure);
            Directory.CreateDirectory(inRootPath);
            Directory.CreateDirectory(outRootPath);
            logger = LogManager.GetCurrentClassLogger();
            fileWatcher = new FileSystemWatcher(sourcePath);
            fileWatcher.EnableRaisingEvents = false;

            fileWatcher.Changed += SourceDirectoryChanged;
            fileWatcher.Created += SourceDirectoryChanged;
            fileWatcher.Renamed += SourceDirectoryChanged;
        }

        private void SourceDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            sourceDirectoryChangedEvent.Set();
        }

        /// <summary>
        /// On service start:
        /// starts fileWatcher to monitor working folder
        /// starts workingThread
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            logger.Info("Starting service...");
            logger.Info(sourcePath);
            stopWorkEvent.Reset();
            sourceDirectoryChangedEvent.Reset();
            fileWatcher.EnableRaisingEvents = true;
            workThread.Start();
        }


        protected override void OnStop()
        {
            logger.Info("=====Stopping service...");
            fileWatcher.EnableRaisingEvents = false;
            stopWorkEvent.Set();
            workThread.Join();
        }

        /// <summary>
        /// Scans source folder every 10 seconds
        /// Moving only when at least three files are in the folder
        /// Send message with file chain
        /// If file is not accessible, leaves it
        /// If stopWorkEvent is set, finishes work
        /// </summary>
        /// <param name="obj"></param>
        protected void WorkProcedure(object obj)
        {
            var movedFiles = new List<string>();
            using (var queueManager = new QueueManager())
            {
                do
                {
                    if (stopWorkEvent.WaitOne(TimeSpan.Zero))
                    {
                        return;
                    }
                    logger.Info("Scanning folder");
                    var files = Directory.EnumerateFiles(sourcePath);
                    if (files.Count() >= 3)
                    {
                        logger.Info("Moving files");

                        foreach (var fileInfo in files)
                        {
                            FileStream file;
                            if (TryOpen(fileInfo, out file, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 3))
                            {
                                file.Close();

                                try
                                {
                                    logger.Info("Moving file {0}", fileInfo);
                                    File.Move(fileInfo, Path.Combine(destinationPath, Path.GetFileName(fileInfo)));
                                    movedFiles.Add(Path.GetFileName(fileInfo));
                                    logger.Info("File {0} moved successfully", Path.GetFileName(fileInfo));
                                }
                                catch (IOException ex)
                                {
                                    logger.Warn("File {0} not moved", Path.GetFileName(fileInfo));
                                    logger.Warn(ex.Message);
                                }
                            }
                            else
                            {
                                logger.Warn("File {0} not opened", Path.GetFileName(fileInfo));
                            }
                        }
                        var message = new QueueMessage(movedFiles);
                        queueManager.SendMessage(message);
                        logger.Info("Message {0} sent", message.Id);
                        movedFiles.Clear();
                    }
                    else
                    {
                        logger.Info("No files in directory");
                    }
                }
                while (
                    WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, sourceDirectoryChangedEvent }, TimeSpan.FromMilliseconds(10000)) != 0
                );
            }
        }

        private bool TryOpen(string fileName, out FileStream file, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, int count)
        {
            for (int i = 0; i < count; i++)
            {
                try
                {
                    file = new FileStream(fileName, fileMode, fileAccess, fileShare);
                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(5000);
                }
            }
            file = null;
            return false;
        }
    }
}
