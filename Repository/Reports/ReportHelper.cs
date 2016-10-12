using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PocReportViewer.Repository.Reports
{
    public static class ReportHelper
    {
        public static Stream GetReportDefinition(string name)
        {
            var asm = Assembly.GetAssembly(typeof(ReportHelper));
            var resource = asm.GetManifestResourceStream("PocReportViewer.Repository.Reports." + name);

            return resource;
        }
    }
}
