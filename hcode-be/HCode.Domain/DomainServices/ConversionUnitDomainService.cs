using Microsoft.Extensions.Localization;
using System.Linq;

namespace HCode.Domain
{
    /// <summary>
    /// Domain service check validate nghiệp vụ đơn vị chuyển đổi
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public class ConversionUnitDomainService : IConversionUnitDomainService
    {
        #region Fields
        /// <summary>
        /// Repository đơn vị chuyển đổi
        /// </summary>
        private readonly IConversionUnitRepository _repository;
        /// <summary>
        /// Domain service đơn vị tính
        /// </summary>
        private readonly IUnitDomainService _unitDomainService;
        /// <summary>
        /// Resource lưu trữ thông báo
        /// </summary>
        private readonly IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        public ConversionUnitDomainService(
            IConversionUnitRepository repository,
            IUnitDomainService unitDomainService,
            IStringLocalizer<Resource> resource)
        {
            _repository = repository;
            _unitDomainService = unitDomainService;
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check tồn tại danh sách đơn vị chuyển đổi theo nguyên vật liệu
        /// </summary>
        /// <param name="conversionUnitIds">Danh sách id đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistConversionUnitsAsync(List<Guid> conversionUnitIds, Guid materialId)
        {
            var conversionUnits = await _repository.GetManyAsync(conversionUnitIds);

            var existIds = conversionUnits.Where(conversionUnit => conversionUnit.MaterialId == materialId)
                                          .Select(conversionUnit => conversionUnit.ConversionUnitId);

            foreach (var conversionUnitId in conversionUnitIds)
            {
                if (!existIds.Any(id => id == conversionUnitId))
                {
                    throw new NotFoundException(
                        MISAErrorCode.ConversionUnitNotFound,
                        _resource["ConversionUnitNotFound"],
                        new ExceptionData("ConversionUnitId", conversionUnitId.ToString()));
                }
            }
        }
        /// <summary>
        /// Check tồn tại danh sách đơn vị muốn chuyển đổi
        /// </summary>
        /// <param name="destinationUnitIds">Danh sách id đơn vị muốn chuyển đổi</param>
        /// <exception cref="NotFoundException">Không tìm thấy đơn vị tính</exception>
        /// Created by: nlnhat (30/08/2023)
        public async Task CheckExistDestinationUnitsAsync(List<Guid> destinationUnitIds)
        {
            await _unitDomainService.CheckExistUnitsAsync(destinationUnitIds);
        }
        #endregion
    }
}