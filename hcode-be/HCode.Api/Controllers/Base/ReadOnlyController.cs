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
    public class ReadOnlyController<TEntityDto> : ControllerBase
    {
        #region Fields
        /// <summary>
        /// Service chỉ đọc
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IReadOnlyService<TEntityDto> _service;
        /// <summary>
        /// Web host environment
        /// </summary>
        /// Created by: nlnhat (11/09/2023)
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo controller chỉ đọc
        /// </summary>
        /// <param name="service">Service chỉ đọc</param>
        /// Created by: nlnhat (17/08/2023)
        public ReadOnlyController(IReadOnlyService<TEntityDto> service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
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
        #endregion
    }
}