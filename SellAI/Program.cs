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
	   builder => builder
          .WithOrigins("http://localhost:8080", "http://localhost:8081", "http://localhost:8081/users/login")
          .AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials()
		);
});

// Add Connection with MongoDB.
builder.Services.AddSingleton<IMongoClient, MongoClient> (sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

// Add DB Options Middleware
builder.Services.Configure<ContextMongoDB>(builder.Configuration.GetSection("MongoDBGestion"));

// Add Middleware
builder.Services.AddScoped<IPassword, PasswordService>();
builder.Services.AddScoped<IClaim, ClaimService>();
builder.Services.AddScoped<IAnalyzeAction, AnalyzeActionService>();

#region Add connection with DB but without controllers
builder.Services.AddTransient<IAnalyzeContext, AnalyzeContextService>();
builder.Services.AddTransient<IData, DataService>();
builder.Services.AddTransient<IRestApi, RestApiService>();
builder.Services.AddTransient<ISysLog, LogService>();
builder.Services.AddTransient<ISysContext, SysContextService>();
builder.Services.AddTransient<ISysMenu, SysMenuService>();
builder.Services.AddTransient<IUserMenu, MenuService>();
#endregion

#region Controllers Interface
builder.Services.AddTransient<IAuthentication, AuthenticationService>();
builder.Services.AddTransient<IBrand, BrandService>();
builder.Services.AddTransient<ICategory, CategoryService>();
builder.Services.AddTransient<IEntity, EntityService>();
builder.Services.AddTransient<IInterpreter, InterpreterService>();
builder.Services.AddTransient<IIntent, IntentService>();
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

app.UseCors("CorsApi");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
