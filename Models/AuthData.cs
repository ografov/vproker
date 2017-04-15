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
    
        private static readonly string[] Roles = new string[] { "Administrator", "User" };

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
                await AddUser(userManager, "admin", "120385", new string[] { "User", "Administrator" });
                await AddUser(userManager, "user" , "123456", new string[] { "User" });
            }
        }

        private static async Task AddUser(Microsoft.AspNet.Identity.UserManager<ApplicationUser> userManager, string name, string password, IEnumerable<string> roles)
        {
            ApplicationUser user = await userManager.FindByNameAsync(name);
            if (user == null)
            {
                user = new ApplicationUser();
                user.UserName = name;
                var createUserTask = userManager.CreateAsync(user, password);
                createUserTask.Wait();
                if (createUserTask.IsCompleted && createUserTask.Result == IdentityResult.Success)
                {
                    var result = userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }
}
