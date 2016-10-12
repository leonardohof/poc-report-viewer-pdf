using PocReportViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocReportViewer.Repository.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public List<Product> GetAll()
        {
            return _Repository.ToList();
        }
    }
}
