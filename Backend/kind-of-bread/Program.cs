using Microsoft.EntityFrameworkCore;
using kind_of_bread.Context;
using kind_of_bread.Repositories;
using kind_of_bread.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<BreadContext>(options => { options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!); });
builder.Services.AddAutoMapper(typeof(BreadProfile));

builder.Services.AddScoped<BreadRepository>();
builder.Services.AddScoped<BreadFactoryResolver>();

builder.Services.AddTransient<LiquidBreadFactory>();
builder.Services.AddTransient<SimpleSandwichFactory>();
builder.Services.AddTransient<DoubleSandwichFactory>();
builder.Services.AddTransient<MultiSandwichFactory>();
builder.Services.AddTransient<BoatlikeFactory>();
builder.Services.AddTransient<ExtrudedFactory>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");
app.UseRouting();
app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Bread}/{action=Index}/{id?}");
app.UseHttpsRedirection();

app.Run();