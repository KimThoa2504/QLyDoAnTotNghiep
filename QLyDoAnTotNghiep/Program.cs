
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLyDoAnTotNghiep.Configurations;
using QLyDoAnTotNghiep.Data;
using QLyDoAnTotNghiep.Services.BoardMembers;
using QLyDoAnTotNghiep.Services.Dashboards;
using QLyDoAnTotNghiep.Services.Documents;
using QLyDoAnTotNghiep.Services.EvaluationBoards;
using QLyDoAnTotNghiep.Services.Evaluations;
using QLyDoAnTotNghiep.Services.Faculties;
using QLyDoAnTotNghiep.Services.ProjectMembers;
using QLyDoAnTotNghiep.Services.Projects;
using QLyDoAnTotNghiep.Services.Reports;
using QLyDoAnTotNghiep.Services.Users;
using System.Text;

namespace QLyDoAnTotNghiep
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //MySqlConnection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("MySqlConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
                    )
                );

            //cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200",           // Angular mặc định
                        "https://localhost:4200"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();   
                });
            });

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; 
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAuthorization();

            //cloud
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddSingleton<Cloudinary>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account);
            });

            //add Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFacultiesService, FacultiesService>();
            builder.Services.AddScoped<IProjectsService, ProjectsService>();
            builder.Services.AddScoped<IProjectMembersService, ProjectMembersService>();
            builder.Services.AddScoped<IEvaluationBoardsService, EvaluationBoardsService>();
            builder.Services.AddScoped<IEvaluationsService, EvaluationsService>();
            builder.Services.AddScoped<IBoardMembersService, BoardMembersService>();
            builder.Services.AddScoped<IDocumentsService, DocumentsService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await SeedAdminAccount(app);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static async Task SeedAdminAccount(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            var userService = services.GetRequiredService<IUserService>();

            // Migrate database
            await context.Database.MigrateAsync();

            // Kiểm tra và tạo Admin nếu chưa có
            if (!await context.Users.AnyAsync())
            {
                await userService.CreateAdminAsync(
                    "admin",
                    "Admin@123",
                    "Quản Trị Viên",
                    "admin@school.edu.vn"
                );

                Console.WriteLine("✅ Đã tạo tài khoản Admin mặc định thành công!");
            }
        }
    }
}
