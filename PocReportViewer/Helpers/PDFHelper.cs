using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PocReportViewer.Helpers
{
    public static class PDFHelper
    {
        public static MemoryStream GetStreamByUrl(string fileUrl)
        {
            var httpRequest = WebRequest.Create(fileUrl) as HttpWebRequest;
            httpRequest.Timeout = 600000;
            var httpResponse = httpRequest.GetResponse();

            var httpFileStream = httpResponse.GetResponseStream();
            var fileMemoryStream = new MemoryStream();
            httpFileStream.CopyTo(fileMemoryStream);

            return fileMemoryStream;
        }

        public static MemoryStream MergeFiles(List<MemoryStream> pdfFiles)
        {
            var pdfMergedDoc = new PdfDocument();

            foreach (var pdfFile in pdfFiles)
            {
                var pdfDoc = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);

                int pdfPageCount = pdfDoc.PageCount;

                for (int pdfPageNumber = 0; pdfPageNumber < pdfPageCount; pdfPageNumber++)
                {
                    var pdfPage = pdfDoc.Pages[pdfPageNumber];
                    pdfMergedDoc.AddPage(pdfPage);
                }
            }

            var pdfMergedFile = new MemoryStream();
            pdfMergedDoc.Save(pdfMergedFile, false);

            return pdfMergedFile;
        }

        public static int GetTotalPages(MemoryStream pdf)
        {
            using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.ReadOnly))
            {
                return pdfDoc.PageCount;
            }
        }

    }
}