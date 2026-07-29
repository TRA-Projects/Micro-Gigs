using Micro_Gigs.Repositories;
using Micro_Gigs.Repositories.Implementations;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Micro_Gigs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Register DbContext
            builder.Services.AddDbContext<MicroGigsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Register repositories
            builder.Services.AddScoped<GigApplicationsRepo>();
            builder.Services.AddScoped<GigAssignmentsRepo>();
            builder.Services.AddScoped<GigAttachmentsRepo>();
            builder.Services.AddScoped<GigCategoriesRepo>();
            builder.Services.AddScoped<GigReviewsRepo>();
            builder.Services.AddScoped<GigsRepo>();
            builder.Services.AddScoped<UsersRepo>();

            // 3. Register services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<GigApplicationsServices>();
            builder.Services.AddScoped<GigAssignmentsServices>();
            builder.Services.AddScoped<GigAttachmentsServices>();
            builder.Services.AddScoped<GigCategoriesServices>();
            builder.Services.AddScoped<GigReviewsServices>();
            builder.Services.AddScoped<GigsServices>();
            builder.Services.AddScoped<UsersServices>();
            builder.Services.AddScoped<EmailService>();

            // 4. Controllers
            builder.Services.AddControllers();

            // 4.1 CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // 5. JWT Authentication
            var jwtKey = builder.Configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JwtSettings:SecretKey مفقود من appsettings.json");
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                                       Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

            // 6. Swagger مع JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Micro-Gigs API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });


            // ─── MIDDLEWARE PIPELINE ───

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Micro-Gigs API v1");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            var uploadsPath = Path.Combine(
                env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                "uploads");
            if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

          
            app.UseCors("AllowAll");

            //  معلّق مؤقتاً لحل مشكلة 401 في التطوير المحلي
            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}