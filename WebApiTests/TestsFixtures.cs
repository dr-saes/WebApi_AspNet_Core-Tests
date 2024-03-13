
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace WebApi_AspNet_Core.Tests
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestsFixtures>
    {

    }


    public class TestsFixtures : IDisposable
    {
        public ProductsController StartPopulatedDatabase()
        {

            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var _context = new ApiDbContext(options);

            InitializeDB(_context);
            var mockConfiguration = new Mock<IConfiguration>();
            var productsController = new ProductsController(_context, mockConfiguration.Object);

            return productsController;
        }

        public ProductsController StartUnPopulatedDatabase()
        {

            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var _context = new ApiDbContext(options);

            InitializeNullDB(_context);
            var mockConfiguration = new Mock<IConfiguration>();
            var productsController = new ProductsController(_context, mockConfiguration.Object);

            return productsController;
        }

        public static void InitializeDB(ApiDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return; // O banco de dados já foi populad
            }

            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = 10.99m, StockQuantity = 100, Description = "Description 1" },
                new Product { Name = "Product 2", Price = 20.50m, StockQuantity = 50, Description = "Description 2" },
                new Product { Name = "Product 3", Price = 15.75m, StockQuantity = 75, Description = "Description 3" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        public static void InitializeNullDB(ApiDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            var products = new List<Product>();

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}