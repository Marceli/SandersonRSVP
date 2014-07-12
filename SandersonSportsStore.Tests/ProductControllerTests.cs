using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Entities;
using NUnit.Framework;
using WebUI.Controllers;

namespace Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void List_Presents_Correct_Page_Of_Products()
        {
            IProductRepository repository = MockProductRepository(
                new Product {Name = "P1"}, new Product {Name = "P2"},
                new Product {Name = "P3"}, new Product {Name = "P4"},
                new Product {Name = "P5"});
            ProductsController controller=new ProductsController(repository);
            controller.PageSize = 3;
            ViewResult result = controller.List(null,2);
            Assert.IsNotNull(result,"Didn't reender view");
            var products = result.ViewData.Model as IList<Product>;
            Assert.AreEqual(2,products.Count);
            Assert.AreEqual("P4", products[0].Name);
            Assert.AreEqual("P5", products[1].Name);
            Assert.AreEqual(2, (int) result.ViewData["CurrentPage"], "Wrong page number");
            Assert.AreEqual(2, (int) result.ViewData["TotalPages"], "Wrong page count");
            Assert.AreEqual("P5", products[1].Name);
        }

        [Test]
        public void ListIncludes_All_Products_When_Category_Is_Null()
        {
            IProductRepository repository = MockProductRepository(
                new Product {Name = "Athems", Category = "Greek"}
                , new Product {Name = "Neptune", Category = "Roman"});
            ProductsController controller = new ProductsController(repository);
            controller.PageSize = 10;
            var result = controller.List(null, 1);
            Assert.IsNotNull(result,"Did'nt render the view");
            var products = (IList<Product>) result.ViewData.Model;
            Assert.AreEqual(2,products.Count,"Got wrong number of products.");
            Assert.AreEqual("Athems", products[0].Name);
            Assert.AreEqual("Neptune",products[1].Name);
        }

        [Test]
        public void List_iters_by_Category_when_Requssted()
        {
            IProductRepository repository = MockProductRepository(
                new Product {Name = "SnowBall", Category = "cats"},
                new Product {Name = "Rex", Category = "dogs"},
                new Product {Name = "Catface", Category = "cats"},
                new Product {Name = "Woofer", Category = "dogs"},
                new Product {Name = "Chomper" , Category = "dogs"});
            ProductsController controller=new ProductsController(repository);
            controller.PageSize = 10;
            var result = controller.List("cats", 1);
            Assert.IsNotNull(result);
            var products = (IList<Product>) result.ViewData.Model;
            Assert.AreEqual(2,products.Count());
            Assert.AreEqual("SnowBall",products[0].Name);


        }
        static IProductRepository MockProductRepository(params Product[] prods)
        {
            var mockProductRepos = new Moq.Mock<IProductRepository>();
            mockProductRepos.Setup(x => x.Products).Returns(prods.AsQueryable());
            return mockProductRepos.Object;
        }
    }
}
