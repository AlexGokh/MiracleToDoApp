using Microsoft.AspNetCore.Identity;

namespace MiracleToDoApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Check if roles exist
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }

            // Check if the Admin user exists
            if (!userManager.Users.Any(u => u.UserName == "admin@todolist.com"))
            {
                var adminUser = new IdentityUser
                {
                    UserName = "admin@todolist.com",
                    Email = "admin@todolist.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(adminUser, "Admin@123").Wait();
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }

            // Check if the regular User exists
            if (!userManager.Users.Any(u => u.UserName == "user@todolist.com"))
            {
                var regularUser = new IdentityUser
                {
                    UserName = "user@todolist.com",
                    Email = "user@todolist.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(regularUser, "User@123").Wait();
                userManager.AddToRoleAsync(regularUser, "User").Wait();
            }

            // Seed initial Todo items (if necessary)
            //if (!context.TodoItems.Any())
            //{
            //    var todoItems = new TodoItem[]
            //    {
            //        new TodoItem{ Title = "Learn ASP.NET Core", IsCompleted = false },
            //        new TodoItem{ Title = "Build a Todo List app", IsCompleted = false },
            //        new TodoItem{ Title = "Deploy the app to Azure", IsCompleted = false }
            //    };

            //    foreach (var item in todoItems)
            //    {
            //        context.TodoItems.Add(item);
            //    }

            //    context.SaveChanges();
            //}
        }
    }
}
