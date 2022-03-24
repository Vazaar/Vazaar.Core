// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Vazaar.Core.Membership;
using Vazaar.Core.Membership.DbContexts;
using Vazaar.Core.Membership.Entities;
using Vazaar.Core.Membership.Security;
using Vazaar.Core.Membership.Services;

namespace Vazaar.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {            
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var assemblyName = typeof(Startup).Assembly.FullName;
            //var migrationAssemblyName = Assembly.GetExecutingAssembly().FullName;

            builder.RegisterModule(new ApiModule());
            builder.RegisterModule(new MembershipModule(connectionString, assemblyName));
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var assemblyName = typeof(Startup).Assembly.FullName;

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(assemblyName)
                )
            );

            services.AddScoped<DbContext>(sp => new ApplicationDbContext(connectionString, assemblyName));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager>()
                .AddRoleManager<RoleManager>()
                .AddSignInManager<SignInManager>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"]
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccessPermission", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new ApiRequirement());
                });
            });

            services.AddSingleton<IAuthorizationHandler, ApiRequirementHandler>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "Vazaar.Core.Api",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "Vazaar.Core.Api v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
