using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Common.Token;
using TodoApi.Constants;
using TodoApi.Data;
using TodoApi.Models.Auth;
using TodoApi.Repository;
using TodoApi.Service.Auth;
using TodoApi.Service.Identity;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString(SettingsConstants.ConnectionString));
            });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(PrivateKey.FromConfiguration(builder.Configuration, SettingsConstants.JwtPrivateKey).Bytes()),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddIdentityCore<AppUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddSignInManager()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ------------ custom services --------------
            builder.Services.AddScoped<RefreshTokenRepository>();
            builder.Services.AddScoped<TodoRepository>();

            builder.Services.AddScoped<IdentityService>();
            builder.Services.AddScoped<RoleService>();
            builder.Services.AddScoped<AppUserService>();
            builder.Services.AddScoped<PrincipalService>();
            builder.Services.AddScoped<JwtService>();
            // ------------ custom services --------------

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => new { message = "This is TodoList Api" });

            app.Run();
        }
    }
}