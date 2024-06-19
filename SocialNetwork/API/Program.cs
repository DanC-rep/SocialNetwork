using Microsoft.AspNetCore.Identity;
using Persistence;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Logic.Interfaces;
using Persistence.Repositories;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var connString = builder.Configuration["Data:NetworkDb:ConnectionString"];

builder.Services.AddDbContext<NetworkDbContext>(options => options.UseSqlServer(connString));

builder.Services.AddIdentity<User, IdentityRole>(
	options =>
	{
		options.SignIn.RequireConfirmedEmail = true;
		options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
		options.Lockout.MaxFailedAccessAttempts = 10;
	}).AddEntityFrameworkStores<NetworkDbContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Account/Login";
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFilesRepository, UserFilesRepository>();
builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<NotificationsService>();
builder.Services.AddScoped<FriendsService>();

builder.Services.AddTransient<ISendEmail, EmailSender>();

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
