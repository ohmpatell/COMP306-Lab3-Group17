using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PodcastManagementSystem.Data;
using PodcastManagementSystem.Models;
using PodcastManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString;
var awsCredentials = new BasicAWSCredentials(
    builder.Configuration["AWS:AccessKey"],
    builder.Configuration["AWS:SecretKey"]
);
var awsRegion = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);

// If RDS is deleted after demo, app can be ran locally using LocalDB after running the db script
try
{
    var ssmClient = new AmazonSimpleSystemsManagementClient(awsCredentials, awsRegion);

    var username = (await ssmClient.GetParameterAsync(new GetParameterRequest { Name = "/podcast/rds/username" })).Parameter.Value;
    var password = (await ssmClient.GetParameterAsync(new GetParameterRequest { Name = "/podcast/rds/password", WithDecryption = true })).Parameter.Value;
    var endpoint = (await ssmClient.GetParameterAsync(new GetParameterRequest { Name = "/podcast/rds/endpoint" })).Parameter.Value;

    connectionString = $"Server={endpoint};Database=PodcastDB;User Id={username};Password={password};TrustServerCertificate=True;Encrypt=False;";
}
catch
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? builder.Configuration.GetConnectionString("RDSConnection")
        ?? "Server=(localdb)\\mssqllocaldb;Database=PodcastDB_Group17;Trusted_Connection=True;MultipleActiveResultSets=true";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
    new AmazonDynamoDBClient(awsCredentials, awsRegion));

builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(awsCredentials, awsRegion));

builder.Services.AddScoped<IEpisodeService, EpisodeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();