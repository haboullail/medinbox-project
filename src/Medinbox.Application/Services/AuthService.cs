using Medinbox.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRoleRepository _roleRepository;

        public AuthService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<bool> RoleHasPermissionAsync(string roleName, string permissionType)
        {
            return _roleRepository.RoleHasPermissionAsync(roleName, permissionType);
        }
    }
}
