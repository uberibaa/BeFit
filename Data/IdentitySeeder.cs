using Microsoft.AspNetCore.Identity;

namespace BeFit.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            const string role = "Administrator";

            if (!await roleManager.RoleExistsAsync(role))
            {
                EnsureSuccess(await roleManager.CreateAsync(
                    new IdentityRole(role)));
            }

            var email = configuration["Admin:Email"];
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                EnsureSuccess(await userManager.CreateAsync(
                    admin, password));
            }
            else if (!await userManager.CheckPasswordAsync(admin, password))
            {
                throw new InvalidOperationException(
                    "Niepoprawne hasło istniejącego administratora.");
            }

            if (!await userManager.IsInRoleAsync(admin, role))
            {
                EnsureSuccess(await userManager.AddToRoleAsync(
                    admin, role));
            }
        }

        private static void EnsureSuccess(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        result.Errors.Select(e => e.Description)));
            }
        }
    }
}
