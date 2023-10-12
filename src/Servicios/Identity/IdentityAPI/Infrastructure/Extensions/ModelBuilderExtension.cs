using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        //Crea roles.
        List<IdentityRole> roles = new() {
            new IdentityRole { Name = "SuperAdmin",
                               NormalizedName = "SUPERADMIN",
                               ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Name = "Moderator",
                               NormalizedName = "MODERATOR",
                               ConcurrencyStamp = Guid.NewGuid().ToString() }
            };
        modelBuilder.Entity<IdentityRole>().HasData(roles);


        //Crea usuarios.
        List<ApplicationUser> users = new() {
                new ApplicationUser {
                    UserName = "superadmin",
                    NormalizedUserName = "SUPERADMIN",
                    Email = "superadmin@email",
                    NormalizedEmail = "SUPERADMIN@EMAIL"
                }
            };
        modelBuilder.Entity<ApplicationUser>().HasData(users);


        //Agrega un password para los usuarios creados anteriormente.
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        users[0].PasswordHash = passwordHasher.HashPassword(users[0], "As!123456");


        //Agrega roles a los usuarios creados anteriormente.
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(q => q.Name == "SuperAdmin").Id
            }
        };
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
    }
}