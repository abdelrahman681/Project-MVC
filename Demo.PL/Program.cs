using Demo.BLL.Interfacies;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

            #region Configure services that Allow dependancy enjection

            Builder.Services.AddControllersWithViews();

            //Builder.Services.AddScoped<AppDbContext>();
            Builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // default AddScoped

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //Builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            //Builder.Services.AddScoped<RoleManager<IdentityRole>>();

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 2;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                config.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            Builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

            //MailKit
            Builder.Services.Configure<MailSettings>(Builder.Configuration.GetSection("MailSettings"));
            Builder.Services.AddTransient<IEmailSettings, EmailSettings>();

            #endregion

            var app = Builder.Build();

            #region Configure Http Qequest Piplines
            if (Builder.Environment.IsDevelopment())
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            }); 
            #endregion

            app.Run();

        }


    }
}
