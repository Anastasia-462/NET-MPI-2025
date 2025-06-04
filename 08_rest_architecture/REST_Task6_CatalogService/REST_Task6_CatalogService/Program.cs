using Microsoft.Extensions.Configuration;
using REST_Task6_CatalogService.Context;
using REST_Task6_CatalogService.Middleware;
using REST_Task6_CatalogService.Services.Categories;
using REST_Task6_CatalogService.Services.Items;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var liteDbConnectionString = builder.Configuration.GetConnectionString("LiteDbConnection");

// ???????????? ILiteContext ? ????????? ?????? ???????????
builder.Services.AddScoped<ILiteContext>(provider => new LiteContext(liteDbConnectionString));
builder.Services.AddScoped<ICategoryProcessor, CategoryProcessor>();
builder.Services.AddScoped<IItemProcessor, ItemProcessor>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
