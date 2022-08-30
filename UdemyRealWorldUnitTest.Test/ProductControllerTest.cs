using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UdemyRealWorldUnitTest.Web.Models;

namespace UdemyRealWorldUnitTest.Test
{
  public class ProductControllerTest
    {
        protected DbContextOptions<UdemyUnitTestDBContext> _contextOptions { get;private set; }

        public void SetContextOptions(DbContextOptions<UdemyUnitTestDBContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }

        public void Seed()
        {
            using (UdemyUnitTestDBContext context=new UdemyUnitTestDBContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Categories.Add(new Category{Name = "Kalemler"});
                context.Categories.Add(new Category{Name = "Defterler"});
                context.SaveChanges();

                context.Products.Add(new Product(){CategoryId = 1, Name = "kalem 10",Price = 100,Color = "Kirmizi"});
                context.Products.Add(new Product(){CategoryId = 2, Name = "kalem 20",Price = 100,Color = "Mavi"});
                context.SaveChanges();
            }
        }
    }
}
