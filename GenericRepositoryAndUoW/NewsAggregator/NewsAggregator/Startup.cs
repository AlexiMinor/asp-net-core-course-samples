using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.Filters;
using NewsAggregators.Services.Implementation;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.AuthorizationPolicies;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregators.Services.Implementation.Mapping;


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
            services.AddTransient<IRepository<User>, UserRepository>(); // for all repositories
            services.AddTransient<IRepository<Role>, RoleRepository>(); // for all repositories
            services.AddTransient<IRepository<Comment>, CommentsRepository>(); // for all repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRssSourseService, RssSourseService>();
            services.AddTransient<OnlinerParser>();
            services.AddTransient<TutByParser>();
            services.AddScoped<CheckDataFilterAttribute>();
            services.AddScoped<CustomExceptionFilterAttribute>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = new PathString("/Account/Login");
                    opt.AccessDeniedPath = new PathString("/Account/Login");
                });

            //Claims-based authorization
            //services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("18-Content", policy =>
            //        {
            //            policy.RequireClaim("age", "18");
            //        });
            //});

            //Policy-based authorization
            //services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("18+Content", policy =>
            //        {
            //            policy.Requirements.Add(new MinAgeRequirement(18));
            //        });
            //});
            //services.AddSingleton<IAuthorizationHandler, MinAgeHandler>();


            services.AddControllersWithViews()
                .AddMvcOptions(opt
                    =>
                {
                    //opt.MaxModelValidationErrors = 50;
                    opt.Filters.Add(new ChromeFilterAttribute(14, 20));
                    //opt.Filters.Add(typeof(CustomExceptionFilterAttribute));
                });

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
                //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(); // Endpoint Routing Middleware 

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<TokenMiddleware>();


            //app.MapWhen(context => context.Request.Query.ContainsKey("admin") && context.Request.Query["admin"] == "true", 
            //    GenerateCustomIndex);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //null,
                //new { controller = new RegexRouteConstraint("^Ne.*")});

                //endpoints.MapControllerRoute(
                //    name: "custom",
                //    pattern: "{action}/{controller}/");
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
