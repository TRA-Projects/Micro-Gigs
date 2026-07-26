
using Micro_Gigs.Repositories;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Micro_Gigs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1.register context / ADD DbContaxt
            builder.Services.AddDbContext<MicroGigsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // service lifetimes 
            
            // 2. register repositories 
            builder.Services.AddScoped<GigApplicationsRepo>();
            builder.Services.AddScoped<GigAssignmentsRepo>();
            builder.Services.AddScoped<GigAttachmentsRepo>();
            builder.Services.AddScoped<GigCategoriesRepo>();
            builder.Services.AddScoped<GigReviewsRepo>();
            builder.Services.AddScoped<GigsRepo>();
            builder.Services.AddScoped<UsersRepo>();

            // 3. register services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<GigApplicationsServices>();
            builder.Services.AddScoped<GigAssignmentsServices>();
            builder.Services.AddScoped<GigAttachmentsServices>();
            builder.Services.AddScoped<GigCategoriesServices>();
            builder.Services.AddScoped<GigReviewsServices>();
            builder.Services.AddScoped<GigsServices>();
            builder.Services.AddScoped<UsersServices>();

            // 4. Controllers
            builder.Services.AddControllers();

            // 5. JWT Authentication
            var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
                            Encoding.UTF8.GetBytes(jwtKey!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var error = context.Exception.Message;
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var claims = context.Principal.Claims.ToList();
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var error = context.Error;
                            var desc = context.ErrorDescription;
                            return Task.CompletedTask;
                        }
                    };

                });

            builder.Services.AddAuthorization();

            // 6. Swagger مع JWT

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the box belo"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });


          
            // AREA 2: MIDDLEWARE PIPELINE
         

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
      