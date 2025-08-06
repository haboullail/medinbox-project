using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!; // "Read", "Write"

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
