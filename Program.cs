using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Application.Mapping;
using SparkUpSolution.Extensions;
using SparkUpSolution.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

# region Services Configuration

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString!)
); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BonusProfile>();
});
builder.WithServices();
builder.WithAuthentication();
builder.Services.AddAuthorization();

# endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.WithDatabaseReset();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
