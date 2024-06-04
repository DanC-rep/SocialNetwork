using Microsoft.AspNetCore.Identity;
using Persistence;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Logic.Interfaces;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var connString = builder.Configuration["Data:NetworkDb:ConnectionString"];

builder.Services.AddDbContext<NetworkDbContext>(options => options.UseSqlServer(connString));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NetworkDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseStatusCodePages();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
