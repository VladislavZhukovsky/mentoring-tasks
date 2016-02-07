using DocumentProcessor.Core;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessor.Core.Processors
{
    public class PdfProcessor: IProcessor
    {
        private const string PDF_EXTENSION = ".pdf";

        private NamingManager namingManager;
        private string workingFolder;
        private object locker = new object();

        private StringBuilder logBuilder;

        public PdfProcessor()
        {
            namingManager = new NamingManager();
            logBuilder = new StringBuilder();
        }

        public ProcessingResultEntry Process(IEnumerable<string> files, string workingFolder, string documentFolder)
        {
            try
            {
                logBuilder.Clear();
                logBuilder.AppendLine("=====Start processing files:");
                foreach(var item in files)
                {
                    logBuilder.AppendLine(String.Format("===Filename: {0}", item));
                }
                this.workingFolder = workingFolder;
                //using for IDisposable PdfDocument object
                using (var doc = CreatePdf(files))
                {
                    //multi-threading sync for getting next document name
                    lock (locker)
                    {
                        var nextPdfName = namingManager.GetNextDocumentName(documentFolder, PDF_EXTENSION);
                        logBuilder.AppendLine(String.Format("===Document name: {0}", nextPdfName));
                        var pdfPath = Path.Combine(documentFolder, nextPdfName + PDF_EXTENSION);
                        doc.Save(pdfPath);
                    }
                    logBuilder.AppendLine("=====End processing successfully");
                }
                return new ProcessingResultEntry() { Result = ProcessingResult.Success, Log = logBuilder.ToString() };
            }
            catch(Exception ex)
            {
                logBuilder.AppendLine("====ERROR");
                logBuilder.AppendLine(ex.Message);
                return new ProcessingResultEntry() { Result = ProcessingResult.Failed, Log = logBuilder.ToString() };
            }
        }

        private PdfDocument CreatePdf(IEnumerable<string> imagePaths)
        {
            logBuilder.AppendLine("===Creating pdf");
            PdfDocument doc = new PdfDocument();
            foreach (var imagePath in imagePaths)
            {
                PdfPage page = doc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                AddPicture(gfx, page, imagePath, 0, 0);
            }
            return doc;
        }

        private void AddPicture(XGraphics gfx, PdfPage page, string imageName, int xPosition, int yPosition)
        {
            var imagePath = Path.Combine(workingFolder, imageName);
            logBuilder.AppendLine(String.Format("===Processing image: {0}", imagePath));
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException(String.Format("Could not find image {0}.", imagePath));
            }
            XImage xImage;
            try
            {
                //using for IDisposable XImage object (blocks files while deleting after processing)
                using (xImage = XImage.FromFile(imagePath))
                {
                    int imagePdfWidth;
                    int imagePdfHeight;
                    //change image size to place it on pdf page
                    GetPdfImageSize(page, xImage, out imagePdfWidth, out imagePdfHeight);
                    gfx.DrawImage(xImage, xPosition, yPosition, imagePdfWidth, imagePdfHeight);
                }
            }
            catch(Exception)
            {
                throw new LoadImageException(string.Format("Could not load the image {0}", imagePath));
            }
        }

        private void GetPdfImageSize(PdfPage page, XImage image, out int width, out int height)
        {
            if (image.PixelWidth > page.Width)
            {
                var k = (double)image.PixelWidth / page.Width.Value;
                width = (int)page.Width.Value;
                height = (int)Math.Round(image.PixelHeight / k);
            }
            else
            {
                width = image.PixelWidth;
                height = image.PixelHeight;
            }
            if (height > page.Height)
            {
                var k = (double)height / page.Height.Value;
                height = (int)page.Height.Value;
                width = (int)Math.Round(width / k);
            }
        }
    }
}
