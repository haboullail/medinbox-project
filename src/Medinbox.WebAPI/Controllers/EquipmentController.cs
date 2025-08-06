using Medinbox.Application.Constants;
using Medinbox.Application.DTOs;
using Medinbox.Application.Interfaces;
using Medinbox.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Medinbox.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _service;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EquipmentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;


        public EquipmentController(IEquipmentService service,
            IHttpClientFactory httpClientFactory,
            ILogger<EquipmentController> logger,
            IConfiguration configuration,
            IAuthService authService)
        {
            _service = service;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _authService = authService;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Reader";
        }
        private async Task<bool> HasPermissionAsync(string permission)
        {
            var role = GetCurrentUserRole();
            return await _authService.RoleHasPermissionAsync(role, permission);
        }


        /// <summary>
        /// Retrieves a paginated list of equipment.
        /// </summary>
        /// <param name="page">The page number (1-based index).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paginated list of equipment wrapped in a standard API response.</returns>
        //[Authorize(Roles = "Reader,ReaderWriter")]
        [Authorize]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (!await HasPermissionAsync("Read"))
                return Forbid();

            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    ResultCode = 1,
                    Message = "Page and pageSize must be greater than 0.",
                    Data = null
                });
            }
            try
            {
                var result = await _service.GetPagedAsync(page, pageSize);

                return Ok(new ApiResponse<Application.DTOs.PagedResult<EquipmentResponse>>
                {
                    ResultCode = 0,
                    Message = "Paginated equipment list retrieved successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving paginated equipment list (page: {Page}, pageSize: {PageSize})", page, pageSize);

                return StatusCode(500, new ApiResponse<string>
                {
                    ResultCode = 1,
                    Message = "An internal server error occurred while retrieving the data.",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Retrieves the full list of all equipment.
        /// </summary>
        /// <returns>A complete list of equipment wrapped in a standard API response.</returns>
        //[Authorize(Roles = "Reader,ReaderWriter")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EquipmentResponse>>>> GetAll()
        {
            if (!await HasPermissionAsync("Read"))
                return Forbid();

            try
            {
                var equipments = await _service.GetAllAsync();

                var data = equipments.Select(e => new EquipmentResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Location = e.Location,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt 
                }).ToList();

                return Ok(new ApiResponse<List<EquipmentResponse>>
                {
                    ResultCode = 0,
                    Message = "Equipment list retrieved successfully.",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EquipmentService.GetAllAsync");

                return StatusCode(500, new ApiResponse<string>
                {
                    ResultCode = 1,
                    Message = "An internal server error occurred.",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Adds a new equipment to the system.
        /// </summary>
        /// <param name="request">The equipment creation request.</param>
        /// <returns>The created equipment wrapped in a standard API response.</returns>

        //[Authorize(Roles = "Writer,ReaderWriter")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEquipmentRequest request)
        {
            if (!await HasPermissionAsync("Write"))
                return Forbid(); 


            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    ResultCode = 1,
                    Message = "Invalid request data.",
                    Data = null
                });
            }

            try
            {
                var createdEquipment = await _service.AddAsync(request);

                // Send WebSocket notification
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    //var json = JsonSerializer.Serialize(createdEquipment);
                    var json = JsonSerializer.Serialize(createdEquipment, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    var content = new StringContent(json, Encoding.UTF8);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var url = _configuration["WebSocket:NotifyUrl"] ?? "http://host.docker.internal:3002/notify";
                    var response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception wsEx)
                {
                    _logger.LogError(wsEx, "WebSocket notification failed for equipment {Id}", createdEquipment.Id);
                }

                return Ok(new ApiResponse<EquipmentResponse>
                {
                    ResultCode = 0,
                    Message = "Equipment added successfully.",
                    Data = createdEquipment
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding equipment.");

                return StatusCode(500, new ApiResponse<string>
                {
                    ResultCode = 1,
                    Message = "An internal server error occurred while adding the equipment.",
                    Data = null
                });
            }
        }
    }
}
