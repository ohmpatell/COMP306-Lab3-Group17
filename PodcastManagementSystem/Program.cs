using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;

namespace PodcastManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var awsAccessKey = builder.Configuration["AWS:AccessKey"];
            var awsSecretKey = builder.Configuration["AWS:SecretKey"];
            var awsRegion = builder.Configuration["AWS:Region"];

            var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

            builder.Configuration.AddSystemsManager("/podcast/rds", new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(awsRegion),
                Credentials = credentials
            });

            var connectionStringParameter = new SqlConnectionStringBuilder(
                builder.Configuration.GetConnectionString("Connection2RDS")
            );
            connectionStringParameter.UserID = builder.Configuration["username"]; 
            connectionStringParameter.Password = builder.Configuration["password"];

            builder.Services.AddDbContext<PodcastDbContext>(options =>
                options.UseSqlServer(connectionStringParameter.ConnectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
