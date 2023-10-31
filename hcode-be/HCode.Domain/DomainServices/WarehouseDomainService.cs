using Microsoft.Extensions.Localization;

namespace HCode.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ nhà kho
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class WarehouseDomainService : IWarehouseDomainService
    {
        #region Fields
        /// <summary>
        /// Repository nhà kho
        /// </summary>
        private readonly IWarehouseRepository _repository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public WarehouseDomainService(IWarehouseRepository repository, IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng mã nhà kho
        /// </summary>
        /// <param name="warehouseId">Id nhà kho để check</param>
        /// <param name="warehouseCode">Mã nhà kho để check</param>
        /// <exception cref="ConflictException">Exception mã đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedCodeAsync(Guid warehouseId, string warehouseCode)
        {
            var warehouseExist = await _repository.GetByCodeAsync(warehouseCode);

            // Nếu trùng mã và trùng với kho khác (tránh trường hợp trùng vs chính kho đấy)
            if (warehouseExist != null && warehouseId != warehouseExist?.WarehouseId)
                throw new ConflictException(
                    MISAErrorCode.WarehouseCodeDuplicated,
                    $"{_resource["WarehouseCode"]} <{warehouseCode}> {_resource["Duplicated"]}",
                    new ExceptionData("WarehouseCode", warehouseCode, ExceptionKey.FormItem, "FormItem"));
        }
        /// <summary>
        /// Check tồn tại nhà kho
        /// </summary>
        /// <param name="warehouseId">Id của nhà kho</param>
        /// <exception cref="NotFoundException">Không tìm thấy nhà kho</exception>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistWarehouseAsync(Guid warehouseId)
        {
            _ = await _repository.GetAsync(warehouseId) ??
                throw new NotFoundException(
                    MISAErrorCode.WarehouseNotFound,
                    _resource["WarehouseNotFound"],
                    new ExceptionData("Warehouse", warehouseId.ToString(), ExceptionKey.FormItem, "FormItem"));
        }
        #endregion
    }
}
