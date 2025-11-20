using Application.Services.Impls;
using Application.Services.Interfaces;
using Common.Exceptions;
using Config;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace codebase_api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                //options.AddPolicy("AllowReact", policy =>
                //{
                //    policy.WithOrigins("https://localhost:5173")
                //          .AllowAnyHeader()
                //          .AllowAnyMethod()
                //          .AllowCredentials();
                //});
                options.AddPolicy("AllowReact", policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                    {
                        return origin.StartsWith("http://localhost")
                               || origin.StartsWith("https://localhost");
                    })
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            // Identity DB Context for injection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            });

            // AppSetting.json
            builder.Services.AddSingleton(sp =>
            {
                var jwtSettings = new JwtSettings();
                builder.Configuration.GetSection("jwt").Bind(jwtSettings);
                return jwtSettings;
            });

            var jwtSettings = builder.Configuration.GetSection("jwt").Get<JwtSettings>();

            // Validate JWT settings
            if (string.IsNullOrEmpty(jwtSettings.Key) || jwtSettings.Key.Length < 32)
                throw new InvalidOperationException("JWT Key must be at least 32 characters long.");
            if (string.IsNullOrEmpty(jwtSettings.Issuer) || string.IsNullOrEmpty(jwtSettings.Audience))
                throw new InvalidOperationException("JWT Issuer and Audience must be configured.");
            if (jwtSettings.AccessTokenExpireMinutes <= 0)
                throw new InvalidOperationException("AccessTokenExpireMinutes must be a valid integer.");

            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(op =>
                {
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ClockSkew = TimeSpan.Zero
                    };
                    op.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

                            var handler = context.HttpContext.RequestServices.GetRequiredService<JwtSecurityTokenHandler>();

                            var jwtToken = JwtUtils.ParseAccessTokenFromHeader(context.Request.Headers.Authorization.ToString());

                            if (await tokenService.IsTokenInvalidAsync(jwtToken))
                            {
                                context.Fail("Token revoked");
                            }
                        },

                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = (int)ErrorCode.UNAUTHORIZED.HttpStatusCode;
                            var problem = new ProblemDetails
                            {
                                Status = context.Response.StatusCode,
                                Title = "Forbidden",
                                Detail = ErrorCode.UNAUTHORIZED.Message,
                                Type = "Forbidden"
                            };
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(problem);
                        }
                    };
                });

            builder.Services.AddProblemDetails(configure =>
               configure.CustomizeProblemDetails = context =>
               {
                   context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
               }
           );

            // DI
            builder.Services.AddScoped<JwtUtils>(); // khong dung singleton vi co the su dung services khac ben trong

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ICookieService, CookieService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ITokenInvalidRepository, TokenInvalidRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddSingleton<JwtSecurityTokenHandler>();


            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();


            // Init role
            using (var scope = app.Services.CreateScope())
            {
                var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
                await roleService.InitializeDefaultRolesAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowReact");

            app.UseHttpsRedirection();

            app.UseExceptionHandler();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
