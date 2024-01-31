using System.Data.Common;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using MySqlConnector;
using Hcode.Api;
using HCode.Domain;
using HCode.Application;
using HCode.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HCode.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            builder.Services.AddHttpContextAccessor();

            // Add controller
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Add options
            builder.Services.AddOptions();

            // Add cache
            builder.Services.AddMemoryCache();
            //builder.Services.AddDistributedMemoryCache();

            // Config
            builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
            builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("AuthConfig"));
            builder.Services.Configure<CEConfig>(builder.Configuration.GetSection("CEConfig"));

            // Add dbconnection
            builder.Services.AddTransient<DbConnection>(options =>
                    new MySqlConnection(builder.Configuration.GetConnectionString("HCodeDev")));

            // Add unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProblemService, ProblemService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IContestService, ContestService>();
            builder.Services.AddScoped<IContestAccountService, ContestAccountService>();
            builder.Services.AddScoped<ISubmissionService, SubmissionService>();
            builder.Services.AddScoped<ICEService, CEService>();

            // Add repositories
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
            builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
            builder.Services.AddScoped<IParameterRepository, ParameterRepository>();
            builder.Services.AddScoped<ITestcaseRepository, TestcaseRepository>();
            builder.Services.AddScoped<IContestRepository, ContestRepository>();
            builder.Services.AddScoped<IContestProblemRepository, ContestProblemRepository>();
            builder.Services.AddScoped<IContestAccountRepository, ContestAccountRepository>();
            builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            builder.Services.AddScoped<IProblemAccountRepository, ProblemAccountRepository>();
            builder.Services.AddScoped<IContestProblemAccountRepository, ContestProblemAccountRepository>();

            // Add mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add middleware
            builder.Services.AddSingleton<HandleMiddleware>();


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

            // Auth
            var jwtConfig = builder.Configuration.GetSection("JwtConfig");
            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = jwtConfig["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]))
                };
            });

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("MyCors");

            app.MapControllers();

            app.UseMiddleware<HandleMiddleware>();

            app.Run();

            #endregion
        }
    }
}