using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Services;
using SellAI.Models;

var builder = WebApplication.CreateBuilder(args);

#region Middleware

// Add services to the container.
builder.Services.AddCors (options => {
	options.AddPolicy("CorsApi",
	   builder => builder.WithOrigins ("http://localhost:8080", "https://localhost:8080")
	.AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

// Add Connection with MongoDB.
builder.Services.AddSingleton<IMongoClient, MongoClient> (sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

// Add DB Options Middleware
builder.Services.Configure<ContextMongoDB>(builder.Configuration.GetSection("MongoDBGestion"));

// Add Password Middleware
builder.Services.AddSingleton<IPassword, PasswordService>();

#region Controllers Interface
builder.Services.AddTransient<IAuthentication, AuthenticationService>();
builder.Services.AddTransient<IInterpreter, InterpreterService>();
builder.Services.AddTransient<IRestApi, RestApiService>();
builder.Services.AddTransient<IUserMenu, MenuService>();
builder.Services.AddTransient<IClaim, ClaimService>();
#endregion


// Add Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Add Jwt Bearer
.AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:Audience"],
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
	};
});
builder.Services.AddAuthorization();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("CorsApi");

app.MapControllers();

app.Run();
