using Medinbox.Application.DTOs;
using Medinbox.Application.Interfaces;
using Medinbox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medinbox.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _repository;

        public EquipmentService(IEquipmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EquipmentResponse>> GetAllAsync()
        {
            var equipments = await _repository.GetAllAsync();

            return equipments.Select(e => new EquipmentResponse
            {
                Id = e.Id,
                Name = e.Name,
                Location = e.Location,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            }).ToList();
        }

        public async Task<EquipmentResponse> AddAsync(AddEquipmentRequest request)
        {
            var equipment = new Equipment
            {
                Name = request.Name,
                Location = request.Location,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(equipment);

            return new EquipmentResponse
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Location = equipment.Location,
                Status = equipment.Status,
                CreatedAt = equipment.CreatedAt
            };
        }

        

        public async Task<PagedResult<EquipmentResponse>> GetPagedAsync(int page, int pageSize)
        {
            var pagedResult = await _repository.GetPagedAsync(page, pageSize);

            var equipmentResponses = pagedResult.Items
                .Select(e => new EquipmentResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Location = e.Location,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt
                }).ToList();

            return new PagedResult<EquipmentResponse>
            {
                Items = equipmentResponses,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _repository.ExistsByNameAsync(name);
        }

    }
}
