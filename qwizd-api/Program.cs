using qwizd_api.Data;
using qwizd_api.Service;
using qwizd_api.Service.Contract;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QwizdContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSession();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();


