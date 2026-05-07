//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Shop.Data.DataBase;
//using Shop.Data.Interfaces;

//namespace Shop
//{
//    public class Startup
//    {
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllersWithViews();

//            services.AddTransient<ICategorys, DBCategory>();
//            services.AddTransient<IItems, DBItems>();
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseStatusCodePages();
//            app.UseStaticFiles();
//            app.UseRouting();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Items}/{action=List}/{id?}");
//            });


//        }
//    }
//}