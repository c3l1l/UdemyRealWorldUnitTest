using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{

    
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsController _controller;
        private List<Product> products;

        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsController(_mockRepo.Object);
            products = new List<Product>() { new Product
            {
                Id = 1,Name="Kalem",Price = 100, Stock = 50, Color = "Kirmizi"
            },
                new Product
                {
                    Id = 2,Name="Kalem",Price = 200, Stock = 500, Color = "Mavi"
                }
            };
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);
            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result); // 1-Geriye viewResult donuyor mu?
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model); //2-ViewResult'in Modeli ProductList mi?
            Assert.Equal<int>(2,productList.Count()); //3- Gelen product listin sayisi 2 mi?

        }

        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _controller.Details(null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index",redirect.ActionName);
        }
        [Fact]
        public async void Details_IdInValid_ReturnNotFound()
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetById(0)).ReturnsAsync(product);
            var result =await _controller.Details(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404,redirect.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        public async void Details_ValidId_ReturnProduct(int productId)
        {
            Product product = products.First(x => x.Id == productId);
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.Details(productId);
            //**Alttaki 4 Durum'u saglamasi gerekir.
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);

            Assert.Equal(product.Id,resultProduct.Id);
            Assert.Equal(product.Name,resultProduct.Name);
        }
    }
}
