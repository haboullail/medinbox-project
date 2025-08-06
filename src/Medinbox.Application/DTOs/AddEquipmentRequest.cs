using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.DTOs
{
    public class AddEquipmentRequest
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;
    }
}
