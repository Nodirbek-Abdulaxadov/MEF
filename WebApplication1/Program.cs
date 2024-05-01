using GEH;
using MongoDB.Querying;
using WebApplication1.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(new DbContextOptions()
{
    ConnectionString = builder.Configuration.GetConnectionString("MongoDBConection")!,
    DatabaseName = builder.Configuration.GetConnectionString("MongoDBName")!
});

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandler>();

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
