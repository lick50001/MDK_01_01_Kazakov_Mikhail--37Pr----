using Shop.Data.DataBase;
using Shop.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICategorys, DBCategory>();
builder.Services.AddTransient<IItems, DBItems>();

// Логирование (теперь можно!)
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Конфигурация конвейера
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Items}/{action=List}/{id?}");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.Run();