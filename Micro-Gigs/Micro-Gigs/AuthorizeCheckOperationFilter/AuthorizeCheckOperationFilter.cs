using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Micro_Gigs
{
    /// <summary>
    /// Adds the lock icon in Swagger only for endpoints decorated with [Authorize].
    /// Public endpoints such as Register and Login remain unlocked.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check whether the endpoint or controller has the [Authorize] attribute.
            var hasAuthorize = context.MethodInfo.DeclaringType!
                                   .GetCustomAttributes(true)
                                   .OfType<AuthorizeAttribute>()
                                   .Any()
                               || context.MethodInfo
                                   .GetCustomAttributes(true)
                                   .OfType<AuthorizeAttribute>()
                                   .Any();

            if (!hasAuthorize) return;

            // Add 401 and 403 response descriptions.
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized — Missing or invalid token." });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden — Insufficient permissions." });

            // Link the endpoint to the Bearer security scheme.
            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [bearerScheme] = new List<string>()
                }
            };
        }
    }
}