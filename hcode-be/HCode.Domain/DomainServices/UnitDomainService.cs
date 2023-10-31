using Microsoft.Extensions.Localization;

namespace HCode.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ đơn vị tính
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class UnitDomainService : IUnitDomainService
    {
        #region Fields
        /// <summary>
        /// Repository đơn vị tính
        /// </summary>
        private readonly IUnitRepository _repository;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public UnitDomainService(
            IUnitRepository repository,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check trùng tên đơn vị tính
        /// </summary>
        /// <param name="unitId">Id đơn vị tính để check</param>
        /// <param name="unitName">Tên đơn vị tính để check</param>
        /// <exception cref="ConflictException">Exception tên đã tồn tại</exception>
        /// Created by: nlnhat (17/08/2023)
        public async Task CheckDuplicatedNameAsync(Guid unitId, string unitName)
        {
            var unitExist = await _repository.GetByNameAsync(unitName);

            // Nếu trùng tên và trùng với đơn vị khác (tránh trường hợp trùng vs chính đơn vị đấy)
            if (unitExist != null && unitId != unitExist?.UnitId)
                throw new ConflictException(
                    MISAErrorCode.UnitNameDuplicated,
                    $"{_resource["UnitName"]} <{unitName}> {_resource["Duplicated"]}",
                    new ExceptionData("UnitName", unitName, ExceptionKey.FormItem, "FormItem"));
        }
        /// <summary>
        /// Check tồn tại đơn vị tính
        /// </summary>
        /// <param name="unitId">Id của đơn vị tính</param>
        /// <exception cref="NotFoundException">Không tìm thấy đơn vị tính</exception>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistUnitAsync(Guid unitId)
        {
            _ = await _repository.GetAsync(unitId) ??
                throw new NotFoundException(
                    MISAErrorCode.UnitNotFound,
                    _resource["UnitNotFound"],
                    new ExceptionData("Unit", unitId.ToString(), ExceptionKey.FormItem, "FormItem"));
        }
        /// <summary>
        /// Check tồn tại danh sách đơn vị tính
        /// </summary>
        /// <param name="unitIds">Danh sách id đơn vị để check tính</param>
        /// <exception cref="NotFoundException">Không tìm thấy đơn vị tính</exception>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistUnitsAsync(List<Guid> unitIds)
        {
            var units = await _repository.GetManyAsync(unitIds);
            
            var existIds = units.Select(unit => unit.UnitId);

            foreach (var unitId in unitIds)
            {
                if (!existIds.Any(id => id == unitId))
                {
                    throw new NotFoundException(
                        MISAErrorCode.UnitNotFound,
                        _resource["UnitNotFound"],
                        new ExceptionData("UnitId", unitId.ToString()));
                }
            }
        }
        #endregion
    }
}
