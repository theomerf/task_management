using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace TaskManagementAPI.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            RepositoryContext context = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                .AddSupportedUICultures("tr-TR")
                .SetDefaultCulture("tr-TR");
            }
            );
        }

        public static async System.Threading.Tasks.Task ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminEmail = "omerfarukyalcin08@gmail.com";
            const string adminPassword = "Admin+123456";

            UserManager<Account> userManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<Account>>();

            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .CreateAsyncScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            Account? user = await userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                user = new Account()
                {
                    FirstName = "Admin",
                    LastName = "Root",
                    Email = adminEmail,
                    PhoneNumber = "05425946284",
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Admin kullanıcısı oluşturulurken hata oluştu.");
                }

                var roleResult = await userManager.AddToRolesAsync(user,
                    roleManager
                    .Roles
                    .Select(r => r.Name!)
                    .ToList()
                );

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Sistem admin kullanıcısının rollerini atarken hata oluştu.");
                }
            }
        }

    }
}
