using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vproker.Models
{
    public class AuthData
    {
        internal const string ADMIN_ROLE = "Administrator";
        internal const string USER_ROLE = "User";
        
        private static readonly string[] Roles = new string[] { ADMIN_ROLE, USER_ROLE };

        public static async Task SeedAuth(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                // add default roles
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // add default users

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                await AddUser(userManager, "admin", "120385", new string[] { USER_ROLE, ADMIN_ROLE });
                await AddUser(userManager, "user" , "123456", new string[] { USER_ROLE });
            }
        }

        private static async Task AddUser(Microsoft.AspNet.Identity.UserManager<ApplicationUser> userManager, string name, string password, IEnumerable<string> roles)
        {
            ApplicationUser user = await userManager.FindByNameAsync(name);
            if (user == null)
            {
                user = new ApplicationUser();
                user.UserName = name;
                user.Email = name;
                var createUserTask = await userManager.CreateAsync(user, password);
                if (createUserTask.Succeeded)
                {
                    var result = await userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }
}
