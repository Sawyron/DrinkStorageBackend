using DrinkStorage.Application;
using DrinkStorage.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Services.EnsureDbCreated())
{
    app.Services.PopulateDb();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").AsEnumerable()
    .Where(x => x.Value is not null)
    .Select(x => x.Value!)
    .ToArray();

app.UseCors(corsBuilder => corsBuilder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins(allowedOrigins));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
