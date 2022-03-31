using FluentScheduler;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.MsSql;
using Shop.Infrastructure.Implementation.Mailing;
using Shop.Infrastructure.Implementation.Web;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.Infrastructure.Interfaces.Mailing;
using Shop.Infrastructure.Interfaces.Web;
using Shop.UseCases.Orders.Queries.GetOrder;
using Shop.UseCases.Orders.Utils;
using Shop.Web.BackgroundJobs;
using Shop.Web.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddTransient<SendEmailsJob>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddAutoMapper(typeof(OrdersProfile));
builder.Services.AddMediatR(typeof(GetOrderQueryHandler));

builder.Services.AddDbContext<IDbContext, AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));
builder.Services.AddDbContext<IReadDbContext, AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandlerMiddleware();

app.MapControllers();

JobManager.JobFactory = new JobFactory(app.Services);
JobManager.Initialize(new FluentSchedulerRegistry());
JobManager.JobException += info =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(info.Exception, "Unhandled exception in job");
};

app.Run();
