using System.Data.Common;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using MySqlConnector;
using Hcode.Api;
using HCode.Domain;
using HCode.Application;
using HCode.Infrastructure;

namespace HCode.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            // Add controller
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Add options
            builder.Services.AddOptions();

            // Add cache
            builder.Services.AddMemoryCache();

            var emailOption = builder.Configuration.GetSection("EmailSetting");
            builder.Services.Configure<EmailSetting>(emailOption);

            // Add dbconnection
            builder.Services.AddTransient<DbConnection>(options =>
                    new MySqlConnection(builder.Configuration.GetConnectionString("HCodeDev")));

            // Add unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Add repositories
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // Add mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add middleware
            builder.Services.AddSingleton<ExceptionMiddleware>();


            builder.Services.AddControllersWithViews(options 
                => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            // Add localization services
            builder.Services.AddLocalization();

            // Configure supported cultures
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("vi-VN"),
                };
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            #endregion

            var app = builder.Build();

            #region Configures

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable localization middleware
            app.UseRequestLocalization();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("MyCors");

            app.MapControllers();

            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();

            #endregion
        }
    }
}