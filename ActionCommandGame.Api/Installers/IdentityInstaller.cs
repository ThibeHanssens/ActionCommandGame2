﻿using ActionCommandGame.Api.Authentication;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Api.Installers.Abstractions;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace ActionCommandGame.Api.Installers
{
    public class IdentityInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<ActionCommandGameDbContext>();

            services.AddTransient<IIdentityService<AuthenticationResult>, IdentityService>();
        }
    }
}
