using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{
    public class ProductControllerTestWithInMemory:ProductControllerTest
    {
        public ProductControllerTestWithInMemory()
        {
            SetContextOptions(new DbContextOptionsBuilder<UdemyUnitTestDBContext>().UseInMemoryDatabase("UdemyUnitTestInMemoryDB").Options);
        }
        [Fact]
        public async Task Create_ModelValidProduct_ReturnsRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "kalem 30", Price = 200, Stock = 100 };
            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var category = context.Categories.First();
                newProduct.CategoryId = category.Id;
                //var repository=new repository<Product>(context);
                var controller = new ProductsController(context);
                var result = await controller.Create(newProduct);
                var redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);


            }

            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);
                Assert.Equal(newProduct.Name,product.Name);

            }
        }
        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeletedAllProducts(int categoryId)
        {
            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var category = await context.Categories.FindAsync(categoryId);
                context.Categories.Remove(category);
                context.SaveChanges();
            }

            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
                Assert.Empty(products);
            }
        }

    }
}
