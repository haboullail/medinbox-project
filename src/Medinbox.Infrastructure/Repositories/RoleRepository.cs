using Medinbox.Application.Interfaces;
using Medinbox.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> RoleHasPermissionAsync(string roleName, string permissionType)
        {
            return await _db.Roles
                .Where(r => r.Name == roleName)
                .SelectMany(r => r.RolePermissions)
                .AnyAsync(rp => rp.Permission.Type == permissionType);
        }
    }
}
