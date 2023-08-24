using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//whatever we'll add in service container , it is DI
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//since we don't need CategoryRepository, coz now we are using UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//1.register our DbContext class
//2.here we'll tell DbContext options to UseSqlServer
//3.we'll pass the connection string of particular DB
//whenever we ask for implementation of DbContext, do the below config and give the object
builder.Services.AddDbContext<ApplicationDbContext>( options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) );


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

//if we write the above line wihtout options, it means even user email is not confirmed , user will be login
//since we are passing IdentityUser, so now when we create a user,by default it'll be IdentityUser
//if we use AddDefaultIdentity , means no role be assigned to the new user by default.
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
//if we use AddIdentity, means we have to pass the roles as well
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();




//since Identity uses razor pages for login,logout, register,etc
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

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

//since we are using razor pages in Identity user management
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
