using PocReportViewer.Models;
using PocReportViewer.Repository.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocReportViewer.Repository
{
    public class PocContext : DbContext
    {
        public PocContext()
            : base("PocContext")
        {
            Database.SetInitializer<PocContext>(new CreateDatabaseIfNotExists<PocContext>());
            Database.SetInitializer<PocContext>(new MigrateDatabaseToLatestVersion<PocContext, Configuration>());
        }

        public DbSet<Product> Products { get; set; }
    }
}
