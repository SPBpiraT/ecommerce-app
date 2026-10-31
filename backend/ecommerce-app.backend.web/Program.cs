using ecommerce_app.backend.web.Common;
using ecommerce_app.backend.web.Middlewares;
using ecommerce_app.backend.web.Services;
using ecommerce_app.backend.web.Services.YooKassaClient;

using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));
builder.Services.AddApplicationServices();
builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddHttpClient<IYooKassaClient, YooKassaClient>();

builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "EcomAuthCookie";

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };

        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevReactPolicy", policy =>
        {
            policy.WithOrigins("https://localhost:5173") //React
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevReactPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
