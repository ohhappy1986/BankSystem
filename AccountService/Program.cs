using AccountService.BLLs.Contracts;
using AccountService.BLLs;
using AccountService.DbContexts.Contracts;
using AccountService.DbContexts;
using AccountService.Repositories.Contracts;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using AccountService.ClientProxies.Contracts;
using AccountService.ClientProxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add api versioning to reduce upgrade risk.
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

//Register DB Context.
builder.Services.AddDbContext<IInMemoryDbContext, InMemoryDbContext>(db => db.UseInMemoryDatabase(databaseName: "bankDB"), ServiceLifetime.Singleton);

//Register Repositories.
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();

//Register BLLs.
builder.Services.AddScoped<IAccountBll, AccountBll>();

//Register Proxies.
builder.Services.AddScoped<ITransactionServiceProxy, TransactionServiceProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
