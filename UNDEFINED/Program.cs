using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UNDEFINED.Areas;
using UNDEFINED.Areas.Admin.Services;
using UNDEFINED.Data;
using UNDEFINED.Services;
using UNDEFINED.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	//Here you can modify password configuration
	//options.Password.
	options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
	options.TokenLifespan = TimeSpan.FromHours(5);
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailingService, MailingService>();

builder.Services.AddTransient<IUploadService, UploadService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAdminServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var provider = scope.ServiceProvider;

	//Migration for production
	//var dbContext = provider.GetRequiredService<ApplicationDbContext>();
	//dbContext.Database.Migrate();

	var userService = provider.GetRequiredService<IUserService>();
	await userService.IntializeAsync();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
