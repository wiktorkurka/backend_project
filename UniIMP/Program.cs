using Microsoft.EntityFrameworkCore;
using Quartz;
using UniIMP.DataAccess;
using UniIMP.DataAccess.Repositories;
using UniIMP.Services;
using UniIMP.Services.Jobs;
using UniIMP.Utility.JsonConverters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            var settings = options.SerializerSettings;
            
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; // Fix JSON Object Reference Loop

            // Register JSON Converters
            settings.Converters.Add(new IpAddressConverter());  
        });
        


services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});

// Add Job scheduling service from Quartz library
services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var pollerJob = new JobKey("PollerJob");

    q.AddJob<SnmpPollerJob>(options => {
         options.WithIdentity(pollerJob);
    });

    q.AddTrigger(options => {
        options.ForJob(pollerJob)
        .WithIdentity("PollerJob-trigger")
        .WithCronSchedule("* */5 * * * ?");
    });
});

services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});


services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
services.AddScoped(typeof(SnmpPollerService));

var app = builder.Build();

// Create after-build servise scope
using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider
                                    .GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.EnsureCreated(); // Ensure creation of the database
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
