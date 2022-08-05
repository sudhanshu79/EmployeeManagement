using EmployeeManagement.Models;
using EmployeeManagement.Models.Employees;
using EmployeeManagement.Models.Users;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration configuaration;
        public Startup(IConfiguration _configuaration)
        {
            configuaration = _configuaration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(configuaration.GetConnectionString("EmployeeDbConnection")));

            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireAssertion(context =>
                        (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type.Equals("Delete Role") && c.Value.Equals("true")))
                         || context.User.IsInRole("Super Admin")

                    ));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("SuperAdminRolePolicy",
                    policy => policy.RequireRole("Super Admin"));

            });
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            services.AddSingleton<IAuthorizationHandler, CanEditAdminsOwnRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.AddAuthentication().AddGoogle(options =>
            {

                options.ClientId = configuaration.GetSection("ExternalLoginProviderStrings").GetSection("Google").GetValue<string>("ClientId");
                options.ClientSecret = configuaration.GetSection("ExternalLoginProviderStrings").GetSection("Google").GetValue<string>("ClientSecret");
            })
             .AddFacebook(options =>
             {
                 options.AppId = configuaration.GetSection("ExternalLoginProviderStrings").GetSection("Facebook").GetValue<string>("AppId");
                 options.AppSecret = configuaration.GetSection("ExternalLoginProviderStrings").GetSection("Facebook").GetValue<string>("AppSecret");
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
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }


            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(route =>
            {

                route.MapRoute("default", "{controller=employees}/{action=index}/{id?}");
            });
        }
    }
}
