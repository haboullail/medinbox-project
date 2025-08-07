using Medinbox.Application.DTOs;
using Medinbox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<List<Equipment>> GetAllAsync();
        Task<Equipment?> GetByIdAsync(int id);
        Task AddAsync(Equipment equipment);

        Task<PagedResult<Equipment>> GetPagedAsync(int page, int pageSize);
        Task<bool> ExistsByNameAsync(string name);
    }
}
