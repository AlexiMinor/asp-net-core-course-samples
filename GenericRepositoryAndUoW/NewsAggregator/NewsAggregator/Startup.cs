using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.MiddlewareComponents;
using NewsAggregators.Services.Implementation;

namespace NewsAggregator
{
    public class Startup
    {
        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetSection("ConnectionStrings")
                .GetValue<string>("DefaultConnection");
            services.AddDbContext<NewsAggregatorContext>(opt =>
                opt.UseSqlServer(connString));


            services.AddTransient<IRepository<News>, NewsRepository>(); // for all repositories
            services.AddTransient<IRepository<RssSourse>, RssSourseRepository>(); // for all repositories

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRssSourseService, RssSourseService>();

            services.AddTransient<IWebPageParser, OnlinerParser>();
            services.AddTransient<IWebPageParser, TutByParser>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(); // Endpoint Routing Middleware 

            app.UseAuthorization();
            //app.UseMiddleware<TokenMiddleware>();


            app.MapWhen(context => context.Request.Query.ContainsKey("admin") && context.Request.Query["admin"] == "true", 
                GenerateCustomIndex);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("added from middleware", new StringValues("12345"));
            //    await next.Invoke();
            //});

            //app.Run(async (context) =>
            //{
            //    context.Request.Headers.Add("added from middleware2", new StringValues("12345"));
            //});

            
        }

        private static void GenerateCustomIndex(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("12345");
            });
        }
    }
}
