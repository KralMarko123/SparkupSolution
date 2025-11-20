using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Application.Mapping;
using SparkUpSolution.Extensions;
using SparkUpSolution.Infrastructure.Persistence;
using SparkUpSolution.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

# region Services Configuration

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString!)
); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BonusProfile>();
});
builder.Services.AddHttpContextAccessor();
builder.WithAppServices();
builder.WithAuthentication();
builder.Services.AddAuthorization();
builder.WithSwaggerEnabled();

# endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.WithDatabaseReset();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();

app.Run();
