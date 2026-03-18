using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Data;
using VShop.IdentityServer.Configuration;
using System.Security.Claims;
using IdentityModel;
using System.Text.Json;

namespace VShop.IdentityServer.SeedDatabase;

public class DatabaseIdentityServerInitializer : IDatabaseSeedInitializer
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseIdentityServerInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeSeedRoles()
    {
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
            _roleManager.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();

        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
            _roleManager.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

    }

    public void InitializeSeedUsers()
    {
        if (_userManager.FindByEmailAsync("admin@vshop.com").Result == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@vshop.com",
                NormalizedEmail = "ADMIN@VSHOP.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                FirstName = "Usuario",
                LastName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            IdentityResult result = _userManager.CreateAsync(admin, "Admin@123").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

                var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
                }).GetAwaiter().GetResult();
            }
        }
        if (_userManager.FindByEmailAsync("client@vshop.com").Result == null)
        {
            var client = new ApplicationUser
            {
                UserName = "client",
                Email = "client@vshop.com",
                NormalizedEmail = "CLIENT@VSHOP.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                FirstName = "Usuario",
                LastName = "Client",
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            IdentityResult result = _userManager.CreateAsync(client, "Client@123").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();
                var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, client.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, client.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
                }).GetAwaiter().GetResult();
            }
        }
    }
}
