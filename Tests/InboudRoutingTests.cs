using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using WebUI;

namespace Tests
{
    [TestFixture]
    public class InboudRoutingTests
    {
        [Test]
        public void Slash_Goes_To_All_Products_Page()
        {
            TestRoute("~/", new {controller = "Products", action = "List", category = (string) null, Page = 1});
        }

        [Test]
        public void Page2_Goes_To_All_Products_Page2()
        {
            TestRoute("~/Page2",new {controller="Products",action="List",category=(string)null,page=2});
        }

        [Test]
        public void Football_Goes_To_Footbal_Page_1()
        {
            TestRoute("~/Football",new {controller="Products",action="List",category="Football",page=1});
        }
        [Test]
        public void Football_Slash_Page43_Goes_To_Football_Page_43()
        {
            TestRoute("~/Football/Page43",new {controller="Products",action="List",category="Football",page=43});
        }


        private void TestRoute(string url, object expectedValues)
        {
            RouteCollection routes=new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var mockHttpContext = new Moq.Mock<HttpContextBase>();
            var mockRequest = new Moq.Mock<HttpRequestBase>();
            mockHttpContext.Setup(x=>x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);

            RouteData routeData=routes.GetRouteData(mockHttpContext.Object);
            Assert.IsNotNull(routeData);
            var expectedDict = new RouteValueDictionary(expectedValues);
            foreach (var expectedVal in expectedDict)
            {
                if (expectedVal.Value == null)
                    Assert.IsNull(routeData.Values[expectedVal.Key]);
                else
                {
                    Assert.AreEqual(expectedVal.Value.ToString(),routeData.Values[expectedVal.Key].ToString());
                }

            }   
        }
    }
}
