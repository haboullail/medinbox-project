using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RoleHasPermissionAsync(string roleName, string permissionType);
    }
}
