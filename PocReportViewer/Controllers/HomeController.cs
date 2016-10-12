using Microsoft.Reporting.WebForms;
using PocReportViewer.Helpers;
using PocReportViewer.Repository;
using PocReportViewer.Repository.Base;
using PocReportViewer.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PocReportViewer.Controllers
{
    public class HomeController : Controller
    {
        private ProductRepository _productRepository;

        public HomeController()
        {
            _productRepository = new ProductRepository();
        }

        public ActionResult Index()
        {
            if (_productRepository.GetAll().Count == 0)
            {
                for (int i = 1; i <= 300; i++)
                {
                    _productRepository.Create(new Models.Product()
                    {
                        Id = i,
                        Name = string.Format("Product {0}", i)
                    });
                }

                UnitOfWork.Save();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Report()
        {
            var products = _productRepository.GetAll();

            using (var rv = new ReportViewer())
            {
                rv.LocalReport.EnableHyperlinks = true;
                rv.LocalReport.EnableExternalImages = true;
                rv.LocalReport.LoadReportDefinition(Repository.Reports.ReportHelper.GetReportDefinition("Report1.rdlc"));

                //Params
                rv.LocalReport.SetParameters(new ReportParameter("InitialPage", "10"));

                //Datasets                    
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsProducts", products));

                return ReportHelper.ExportToPDF(rv);
            }
        }

        [HttpGet]
        public FileResult Reports()
        {
            var initialPage = 0;
            List<MemoryStream> pdfFiles = new List<MemoryStream>()
            {
                //PDFHelper.GetStreamByUrl(Url.Action("Report", "Home", new { }, this.Request.Url.Scheme)),
                //PDFHelper.GetStreamByUrl(Url.Action("Report2", "Home", new { }, this.Request.Url.Scheme)),
            };

            pdfFiles.Add(Report_1(initialPage));
            initialPage += PDFHelper.GetTotalPages(pdfFiles.Last());
            pdfFiles.Add(Report_2(initialPage));

            var pdfName = string.Format("Reports.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var pdfMergedFiles = PDFHelper.MergeFiles(pdfFiles);

            return File(pdfMergedFiles, "application/pdf", pdfName);
        }

        #region Functions

        private MemoryStream Report_1(int initialPage)
        {
            var products = _productRepository.GetAll();

            using (var rv = new ReportViewer())
            {
                rv.LocalReport.EnableHyperlinks = true;
                rv.LocalReport.EnableExternalImages = true;
                rv.LocalReport.LoadReportDefinition(Repository.Reports.ReportHelper.GetReportDefinition("Report1.rdlc"));

                //Params
                rv.LocalReport.SetParameters(new ReportParameter("Header", "<b>Utilizando Tags Html</b>&nbsp;<i>Italic</i>"));
                rv.LocalReport.SetParameters(new ReportParameter("InitialPage", initialPage.ToString()));

                //Datasets                    
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsProducts", products));

                var bytes = ReportHelper.ExportToPdfInBytes(rv);

                return new MemoryStream(bytes);
            }
        }

        private MemoryStream Report_2(int initialPage)
        {
            var products = _productRepository.GetAll();

            using (var rv = new ReportViewer())
            {
                rv.LocalReport.EnableHyperlinks = true;
                rv.LocalReport.EnableExternalImages = true;
                rv.LocalReport.LoadReportDefinition(Repository.Reports.ReportHelper.GetReportDefinition("Report2.rdlc"));

                //Params
                rv.LocalReport.SetParameters(new ReportParameter("InitialPage", initialPage.ToString()));

                //Datasets                    
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsProducts", products));

                var bytes = ReportHelper.ExportToPdfInBytes(rv);

                return new MemoryStream(bytes);
            }
        }

        #endregion
    }
}