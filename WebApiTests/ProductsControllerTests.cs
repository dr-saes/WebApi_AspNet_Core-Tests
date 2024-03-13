using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace WebApi_AspNet_Core.Tests
{
    public class ProductsControllerTests
    {

        //GetProducts() - _context not null
        [Fact(DisplayName = "Retornar todos Produtos")]
        [Trait("Verb", "Get()")]
        public async Task GetProducts_ProductsNotNull_ReturnProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);

                // Act
                var result = await productsController.GetProducts();

                // Assert
                Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
                Assert.NotNull(result.Value);
            }
        }

        //GetProducts() - _context null
        [Fact(DisplayName = "Retornar 'NotFound' (repositorio invalido)")]
        public async Task GetProducts_ProductsNull_ReturnNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);

                // Act
                var result = await productsController.GetProducts();

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }


        //GetProduct()  - _context not null and product not null
        [Fact]
        public async Task GetProductById_ProductsNotNull_ReturnProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);
                var id = 1;

                // Act
                var result = await productsController.GetProduct(id);

                // Assert
                Assert.IsType<ActionResult<Product>>(result);
                Assert.NotNull(result.Value);
            }
        }

        //GetProduct() - _context null
        [Fact]
        public async Task GetProductById_ProductsNull_ReturnNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.InitializeNull(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);
                var id = 1;

                // Act
                var result = await productsController.GetProduct(id);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        //GetProduct() - _context not null and product null
        [Fact]
        public async Task GetProductById_ProductsNotNullAndProductNull_ReturnNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);
                var id = 111111;

                // Act
                var result = await productsController.GetProduct(id);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }


        //PostProduct() - _context not null
        [Fact]
        public async Task PostProduct_ProductsNotNull_ReturnProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);
                var product = new Product
                {
                    Name = "Product 4",
                    Price = 13.75m,
                    StockQuantity = 30,
                    Description = "Description 4"

                };

                // Act
                var result = await productsController.PostProduct(product);

                // Assert
                Assert.IsType<ActionResult<Product>>(result);
                Assert.NotNull(result.Result);
            }
        }

        //PostProduct() - _context null
        [Fact]
        public async Task PostProduct_ProductsNull_ReturnProblem()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApiDbContext(options))
            {
                context.Products = null;

                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(context, mockConfiguration.Object);

                // Act
                var result = await productsController.PostProduct(null);

                // Assert
                Assert.IsType<ObjectResult>(result.Result);
                var objectResult = result.Result as ObjectResult;
                var problemDetails = objectResult.Value as ProblemDetails;
                var detail = problemDetails.Detail;

                Assert.Equal("Error creating the product, contact support.", detail);
            }


        }


        //PutProduct() - _context not null and product not null
        [Fact]
        public async Task PutProductById_ProductsAndProductNotNull_ReturnNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);

                var id = 1;
                var product = _context.Products.FirstOrDefault(item => item.Id == id);
                product.StockQuantity = 2000;
                product.Description = "Description test Put";

                // Act
                var result = await productsController.PutProduct(id, product);

                // Assert
                Assert.IsType<NoContentResult>(result.Result);
            }
        }

        //PutProduct() - _context not null and product null and product.Id null
        [Fact]
        public async Task PutProductById_ProductsNotNullAndProductAndIDNull_ReturnBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);

                var id = 111111;
                var product = _context.Products.FirstOrDefault(item => item.Id == id);

                // Act
                var result = await productsController.PutProduct(id, product);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        //PutProduct() - _context not null and product not null but product.Id null
        [Fact]
        public async Task PutProductById_ProductsNotNullAndProductNotNullAndIDNull_ReturnBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var _context = new ApiDbContext(options))
            {
                InMemoryDatabaseInitializer.Initialize(_context);
                var mockConfiguration = new Mock<IConfiguration>();
                var productsController = new ProductsController(_context, mockConfiguration.Object);

                var id = 111111;
                var product = _context.Products.FirstOrDefault(item => item.Id == 1);

                // Act
                var result = await productsController.PutProduct(id, product);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);

            }




        }

    }
}