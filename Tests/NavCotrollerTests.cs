using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Entities;
using NUnit.Framework;
using WebUI.Controllers;

namespace Tests
{

    [TestFixture]
    public class NavCotrollerTests
    {
        [Test]
        public void Takes_IproductRepository_As_constructor_Param()
        {
            new NavController((IProductRepository) null);
        }

        [Test]
        public void Highlights_Current_category()
        {
            IQueryable<Product> products = new[]
            {
                new Product {Name = "A", Category = "Animal"},
                new Product {Name = "B", Category = "Vegetable"}
            }.AsQueryable();
            var mockProductRepos = new Moq.Mock<IProductRepository>();
            mockProductRepos.Setup(x => x.Products).Returns(products);
            var controller = new NavController(mockProductRepos.Object);

            var result = controller.Menu("Vegetable");
            var highightedLinks = ((IEnumerable<NavLink>) result.ViewData.Model).Where(x => x.IsSelected).ToList();
            Assert.AreEqual(1,highightedLinks.Count);
            Assert.AreEqual("Vegetable", highightedLinks[0].Text);
        }

        [Test]
        public void Produces_Home_Plus_Navlink_For_Each_Distinct_Category()
        {
            IQueryable<Product> products = new[]
            {
                new Product {Name = "A", Category = "Animal"},
                new Product {Name = "B", Category = "Vegetable"},
                new Product {Name = "C", Category = "Mineral"},
                new Product {Name = "D", Category = "Vegetable"},
                new Product {Name = "E", Category = "Animal"}
            }.AsQueryable();
            var mockProductRepos = new Moq.Mock<IProductRepository>();
            mockProductRepos.Setup(x=>x.Products).Returns(products);
            var controller = new NavController(mockProductRepos.Object);
            ViewResult result=controller.Menu(null);
            var links=((IEnumerable<NavLink>)result.ViewData.Model).ToList();
            Assert.IsEmpty(result.ViewName);
            Assert.AreEqual(4,links.Count);
            Assert.AreEqual("Home",links[0].Text);
            Assert.AreEqual("Animal",links[1].Text);
            Assert.AreEqual("Mineral",links[2].Text);
            Assert.AreEqual("Vegetable",links[3].Text);
            foreach (var link in links)
            {
                Assert.AreEqual("Products", link.RouteValues["controller"]);
                Assert.AreEqual("List", link.RouteValues["action"]);
                Assert.AreEqual(1, link.RouteValues["page"]);
                if(links.IndexOf(link)==0)
                    Assert.IsNull(link.RouteValues["category"]);
                else
                {
                    Assert.AreEqual(link.Text, link.RouteValues["category"]);
                }
            }
        }
    }
}
