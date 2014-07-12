using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlProductRepository:IProductRepository
    {
        private Table<Product> productTable ;

        public SqlProductRepository(string connectionString)
        {
            var dataContext = new DataContext(connectionString);
           
            productTable = dataContext.GetTable<Product>();
        }

        public IQueryable<Product> Products
        {
            get { return productTable; }
        }
    }
}
