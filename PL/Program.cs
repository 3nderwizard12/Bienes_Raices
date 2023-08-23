var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
        name: "NewPassword",
        pattern: "Usuario/NewPassword/{email}",
        defaults: new { controller = "Usuario", Action = "NewPassword" });


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Usuario}/{action=Login}/{id?}");
});

app.Run();