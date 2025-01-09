using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentCRUD.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentCRUDContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StudentCRUDContext") ?? throw new InvalidOperationException("Connection string 'StudentCRUDContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // http 
app.UseStaticFiles();      // css / pngs / www roots 
app.UseRouting();          //  responsible to route the appropriate controller 
app.UseAuthorization();    // authorize the user


app.MapControllerRoute(  
    name: "default",
    pattern: "{controller=Students}/{action=Add}");        ///{id?}

app.Run();
