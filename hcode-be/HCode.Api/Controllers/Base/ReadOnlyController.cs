using Microsoft.AspNetCore.Mvc;
using HCode.Application;
using HCode.Domain;

namespace HCode.Api
{
    /// <summary>
    /// Lớp controller cơ sở chỉ đọc 
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReadOnlyController<TEntityDto, TEntity> : ControllerBase where TEntityDto : BaseDto where TEntity : BaseEntity
    {
        #region Fields
        /// <summary>
        /// Service chỉ đọc
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IReadOnlyService<TEntityDto, TEntity> _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo controller chỉ đọc
        /// </summary>
        /// <param name="service">Service chỉ đọc</param>
        /// Created by: nlnhat (17/08/2023)
        public ReadOnlyController(IReadOnlyService<TEntityDto, TEntity> service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy toàn bộ đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = new ServerResponse();

            result.Data = await _service.GetAllAsync();

            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <returns>Đối tượng có id được truy vấn</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = new ServerResponse();

            result.Data = await _service.GetAsync(id);

            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <returns>Đối tượng có id được truy vấn</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Filter")]
        public async Task<IActionResult> FilterAsync([FromBody]FilterRequestDto filterRequestDto)
        {
            var result = new ServerResponse();

            result.Data = await _service.FilterAsync(filterRequestDto.KeySearch, filterRequestDto.PagingModel, filterRequestDto.SortModels, filterRequestDto.FilterModels);

            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}