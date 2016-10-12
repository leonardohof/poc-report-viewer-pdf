using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PocReportViewer.Helpers
{
    public static class ReportHelper
    {
        public static FileContentResult ExportToPDF(ReportViewer rv)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string fileNameExtension;

            var bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

            return new FileContentResult(bytes, mimeType);
        }

        public static byte[] ExportToPdfInBytes(ReportViewer rv)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string fileNameExtension;

            var bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

            return bytes;
        }
    }
}