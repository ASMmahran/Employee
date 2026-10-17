using Employee.Data;
using Employee.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContextFactory<AppDbContext>(o => o.UseSqlServer(cs), ServiceLifetime.Scoped);
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(cs));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();