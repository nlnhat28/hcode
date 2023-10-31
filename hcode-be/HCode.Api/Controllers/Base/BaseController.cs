using Microsoft.AspNetCore.Mvc;
using HCode.Application;

namespace HCode.Api
{
    /// <summary>
    /// Lớp controller nhân viên
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseController<TEntityDto, TEntity> : ReadOnlyController<TEntityDto>
    {
        #region Fields
        /// <summary>
        /// Service cơ sở
        /// </summary>
        /// Created by: nlnhat (15/07/2023)
        private readonly IBaseService<TEntityDto, TEntity> _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo controller cơ sở
        /// </summary>
        /// <param name="service">Service cơ sở</param>
        /// Created by: nlnhat (17/08/2023)
        public BaseController(IBaseService<TEntityDto, TEntity> service)
             : base(service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tạo mới 1 đối tượng
        /// </summary>
        /// <param name="entityDto">Dto tạo đối tượng</param>
        /// <returns>Id bản ghi mới</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TEntityDto entityDto)
        {
            var result = await _service.CreateAsync(entityDto);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        /// <summary>
        /// Sửa 1 đối tượng
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <param name="entityUpdateDto">Dto cập nhật đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] TEntityDto entityDto)
        {
            var result = await _service.UpdateAsync(id, entityDto);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Xoá 1 đối tượng theo id
        /// </summary>
        /// <param name="id">Id của đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Xoá nhiều đối tượng từ danh sách id
        /// </summary>
        /// <param name="ids">Danh sách id đối tượng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (17/08/2023)
        [HttpDelete]
        public async Task<IActionResult> DeleteManyAsync([FromBody] List<Guid> ids)
        {
            var result = await _service.DeleteManyAsync(ids);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        #endregion
    }
}

