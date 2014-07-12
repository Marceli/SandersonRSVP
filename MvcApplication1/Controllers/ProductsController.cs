using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DomainModel.Abstract;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            PageSize = 3;
            this.productRepository = productRepository;
        }

        public int PageSize { get; set; }


        public ViewResult List(String category,int page)
        {
            int numProducts = productRepository.Products.Count();
            ViewData["TotalPages"] = (int) Math.Ceiling((double) numProducts/PageSize);
            ViewData["CurrentPage"] = page;
            ViewData["CurrentCategory"] = category;
            var products = productRepository.Products;
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);
            products= products.Skip((page-1)*PageSize).Take(PageSize);
            return View(products.ToList());
        }

    }
}
