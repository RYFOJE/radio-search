using Microsoft.Extensions.Logging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Radio_Search.Importer.Canada.Services.Interfaces;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    /// <inheritdoc/>
    public class PDFProcessingServices : IPDFProcessingServices
    {
        private readonly ILogger<PDFProcessingServices> _logger;

        public PDFProcessingServices(ILogger<PDFProcessingServices> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public Stream MergePDFToSinglePage(Stream fileStream)
        {
            try
            {
                // Reset position to start of stream
                fileStream.Position = 0;

                // Read the input stream into a byte array (we need this for XPdfForm)
                byte[] pdfBytes;
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    pdfBytes = ms.ToArray();
                }

                // Open the PDF document for import
                var src = PdfReader.Open(new MemoryStream(pdfBytes), PdfDocumentOpenMode.Import);
                var dst = new PdfDocument();

                XUnit maxWidth = XUnit.FromPoint(0);
                XUnit totalHeight = XUnit.FromPoint(0);

                _logger.LogInformation("Merging a total of {PageCount} Pages", src.Pages);

                // Calculate max width and total height for vertical stacking
                foreach (var currPage in src.Pages)
                {
                    var height = currPage.Height;
                    var width = currPage.Width;

                    if (maxWidth < width)
                        maxWidth = width;
                    totalHeight += height;
                }

                // Create a large custom page for vertical merge
                var page = dst.AddPage();
                page.Width = maxWidth;
                page.Height = totalHeight;

                var gfx = XGraphics.FromPdfPage(page);
                double y = 0;

                // Create a single XPdfForm for the whole document
                using var sharedFormStream = new MemoryStream(pdfBytes);
                var form = XPdfForm.FromStream(sharedFormStream);

                // Draw each page from the form
                for (int i = 0; i < src.PageCount; i++)
                {
                    var srcPage = src.Pages[i];

                    // Set which page in the form to use
                    form.PageNumber = i + 1;

                    // Draw the page on the output document
                    gfx.DrawImage(form, 0, y, srcPage.Width.Point, srcPage.Height.Point);
                    y += srcPage.Height.Point;
                }

                // Save to a MemoryStream and return
                var outputStream = new MemoryStream();
                dst.Save(outputStream, false);
                outputStream.Position = 0;
                return outputStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to merge the PDF");
                throw new Exception($"Error combining PDF pages: {ex.Message}", ex);
            }
        }
    }
}
