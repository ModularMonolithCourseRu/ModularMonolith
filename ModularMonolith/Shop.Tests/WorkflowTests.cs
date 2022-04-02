using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.DataAccess.MsSql;
using Shop.Entities.Models;
using Shop.Infrastructure.Implementation.Mailing;
using Shop.Infrastructure.Implementation.Web;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.Infrastructure.Interfaces.Mailing;
using Shop.Infrastructure.Interfaces.Web;
using Shop.UseCases.Orders.Commands.CreateOrder;
using Shop.UseCases.Orders.Dtos;
using Shop.UseCases.Orders.Queries.GetOrder;
using Shop.UseCases.Orders.Utils;
using Xunit;

namespace Shop.Tests
{
    public class WorkflowTests
    {
        [Fact]
        public async Task Should_Create_Order_And_Email()
        {
            //arrange
            var (connectionString, configuration) = CreateConfiguration();

            var services = CreateServiceProvider(configuration);
            services.AddDbContext<IDbContext, AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MsSqlConnection")));
            services.AddDbContext<IReadDbContext, AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MsSqlConnection")));
            var serviceProvider = services.BuildServiceProvider();

            var dbContext = await CreateDatabase(connectionString);
            
            var sender = serviceProvider.GetRequiredService<ISender>();
            var dto = new CreateOrderDto { Items = new[] { new OrderItemDto { Count = 1, ProductId = 1 } } };
            
            //act
            var orderId = await sender.Send(new CreateOrderCommand { Dto = dto });

            //assert
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            var email = await dbContext.Emails.FirstOrDefaultAsync(x => x.OrderId == orderId);
            
            Assert.NotNull(order);
            Assert.NotNull(email);

            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task Should_Not_Create_Order_And_Email_On_Error()
        {
            //arrange
            var (connectionString, configuration) = CreateConfiguration();

            var services = CreateServiceProvider(configuration);
            services.AddDbContext<IReadDbContext, AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MsSqlConnection")));
            services.AddDbContext<IDbContext, AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("MsSqlConnection")));
            services.Decorate<IDbContext, TestDbContext>();
            
            var serviceProvider = services.BuildServiceProvider();

            var dbContext = await CreateDatabase(connectionString);

            var sender = serviceProvider.GetRequiredService<ISender>();
            var dto = new CreateOrderDto { Items = new[] { new OrderItemDto { Count = 1, ProductId = 1 } } };

            //act
            await Assert.ThrowsAsync<Exception>(() => sender.Send(new CreateOrderCommand { Dto = dto }));

            //assert
            var ordersCount = await dbContext.Orders.CountAsync();
            var emailsCount = await dbContext.Emails.CountAsync();

            Assert.Equal(0, ordersCount);
            Assert.Equal(0, emailsCount);

            await dbContext.Database.EnsureDeletedAsync();
        }

        class TestDbContext : IDbContext
        {
            private readonly IDbContext _context;

            public TestDbContext(IDbContext context)
            {
                _context = context;
            }

            public DbSet<Order> Orders => _context.Orders;
            
            public DbSet<Product> Products => _context.Products;

            public DbSet<Email> Emails => _context.Emails;

            public int SaveChanges()
            {
                throw new Exception("Fail");
            }

            public Task<int> SaveChangesAsync(CancellationToken token = default)
            {
                throw new Exception("Fail");
            }
        }

        private (string, IConfigurationRoot) CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = $"Data Source=.;Initial Catalog=Test_{Guid.NewGuid()};Integrated Security=True";
            configuration.GetSection("ConnectionStrings")["MsSqlConnection"] = connectionString;
            
            return (connectionString, configuration);
        }

        private ServiceCollection CreateServiceProvider(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddAutoMapper(typeof(OrdersProfile));
            services.AddMediatR(typeof(GetOrderQueryHandler));

            return services;
        }

        private async Task<AppDbContext> CreateDatabase(string connectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new AppDbContext(options);
            await dbContext.Database.MigrateAsync();

            return dbContext;
        }
    }
}