using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DomainModel.Abstract;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository productRepository;

        public NavController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ViewResult Menu(string highlightCategory)
        {
            var navLinks = new List<NavLink>();
            navLinks.Add(new CategoryLink(null));
            IQueryable<string> categories = productRepository.Products.Select(x => x.Category);
            foreach (string category in categories.Distinct().OrderBy(x => x))
            {
                navLinks.Add(new CategoryLink(category){IsSelected = (category==highlightCategory)});
            }
            return View(navLinks);
        }
    }

    public class NavLink
    {
        public string Text { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CategoryLink : NavLink
    {
        public CategoryLink(string categoryPar)
        {
            Text = categoryPar ?? "Home";
            RouteValues = new RouteValueDictionary(
                new {controller = "Products", action = "List", category = categoryPar, page = 1});
        }
    }
}