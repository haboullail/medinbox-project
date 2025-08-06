using Medinbox.Application.DTOs;
using Medinbox.Application.Interfaces;
using Medinbox.Domain.Entities;
using Medinbox.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing Equipment data using EF Core.
    /// Implements IEquipmentRepository.
    /// </summary>
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _context;
        public EquipmentRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all equipment records ordered by creation date (descending).
        /// </summary>
        public async Task<List<Equipment>> GetAllAsync()
        {
            return await _context.Equipments
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// Get a single equipment record by ID.
        /// </summary>
        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _context.Equipments.FindAsync(id);
        }
        /// <summary>
        /// Add a new equipment and save changes.
        /// </summary>
        public async Task AddAsync(Equipment equipment)
        {
            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();
        }
        
        public async Task<PagedResult<Equipment>> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Equipments
                .OrderByDescending(e => e.CreatedAt); 

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Equipment>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

    }
}
