using Medinbox.Application.DTOs;
using Medinbox.Domain.Entities;

namespace Medinbox.Application.Interfaces
{
    /// <summary>
    /// Application-level service interface for managing equipment.
    /// Defines business operations exposed to the controllers.
    /// </summary>
    public interface IEquipmentService
    {
        /// <summary>
        /// Retrieves all equipment, ordered by creation date (most recent first).
        /// </summary>
        /// <returns>A list of equipment in response DTO format.</returns>
        Task<List<EquipmentResponse>> GetAllAsync();
        /// <summary>
        /// Adds a new equipment to the system.
        /// </summary>
        /// <param name="request">The equipment details provided by the client.</param>
        /// <returns>The created equipment as a response DTO.</returns>
        Task<EquipmentResponse> AddAsync(AddEquipmentRequest request);
        /// <summary>
        /// Retrieves a paginated list of equipment.
        /// </summary>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paged result containing a list of equipment and pagination metadata.</returns>
        Task<PagedResult<EquipmentResponse>> GetPagedAsync(int page, int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name);
    }
}
