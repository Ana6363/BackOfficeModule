using BackOffice.Infrastructure;
using BackOffice.Domain.Users;
using BackOffice.Infrastructure.Users; 
using Microsoft.EntityFrameworkCore;
using BackOffice.Domain.Shared;
using BackOffice.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwagger();  

var connectionString = builder.Configuration.GetConnectionString("BackOfficeDb");
builder.Services.AddDbContext<BackOfficeDbContext>(options =>
   options.UseSqlServer(connectionString)
    .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IUserRepository, UsersRepository>();
builder.Services.AddTransient<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.  

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
