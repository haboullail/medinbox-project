using Medinbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Medinbox.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Roles.AnyAsync()) return;

            var readPermission = new Permission { Type = "Read" };
            var writePermission = new Permission { Type = "Write" };

            var role1 = new Role
            {
                Name = "Reader",              
                Color = "Blue",
                RolePermissions = new List<RolePermission>
                {
                    new RolePermission { Permission = readPermission }
                }
            };

            var role2 = new Role
            {
                Name = "Writer",
                Color = "Yellow",
                RolePermissions = new List<RolePermission>
                {
                    new RolePermission { Permission = writePermission }
                }
            };

            var role3 = new Role
            {
                Name = "ReaderWriter",
                Color = "Green",
                RolePermissions = new List<RolePermission>
                {
                    new RolePermission { Permission = readPermission },
                    new RolePermission { Permission = writePermission }
                }
            };

            await context.Roles.AddRangeAsync(role1, role2, role3);
            await context.SaveChangesAsync();
        }
    }
}
