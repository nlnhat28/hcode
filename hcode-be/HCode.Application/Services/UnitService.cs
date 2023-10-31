using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service đơn vị tính từ giao diện
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class UnitService : BaseService<UnitDto, Unit>, IUnitService
    {
        #region Fields
        /// <summary>
        /// Domain service đơn vị tính, validate nghiệp vụ
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IUnitDomainService _domainService;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service đơn vị tính
        /// </summary>
        /// <param name="repository">Repository đơn vị tính</param>
        /// <param name="domainService">Domain service đơn vị tính</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public UnitService(IUnitRepository repository, IUnitDomainService domainService,
                               IStringLocalizer<Resource> resource, IMapper mapper, IUnitOfWork unitOfWork)
                             : base(repository, resource, mapper, unitOfWork)
        {
            _domainService = domainService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Map dto tạo mới sang thực thể đơn vị tính
        /// </summary>
        /// <param name="unitDto">Dto tạo mới đơn vị tính</param>
        /// <returns>Thực thể đơn vị tính</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Unit MapCreateDtoToEntity(UnitDto unitDto)
        {
            unitDto.UnitId = Guid.NewGuid();
            unitDto.CreatedDate ??= DateTime.UtcNow;

            var unit = _mapper.Map<Unit>(unitDto);
            return unit;
        }
        /// <summary>
        /// Map dto cập nhật sang thực thể đơn vị tính
        /// </summary>
        /// <param name="unitDto">Dto cập nhật đơn vị tính</param>
        /// <returns>Thực thể đơn vị tính</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Unit MapUpdateDtoToEntity(UnitDto unitDto)
        {
            unitDto.ModifiedDate = DateTime.UtcNow;

            var unit = _mapper.Map<Unit>(unitDto);
            return unit;
        }
        /// <summary>
        /// Validate tổng thể (input + nghiệp vụ) đơn vị tính
        /// </summary>
        /// <param name="unit">Thực thể đơn vị tính</param>
        /// Created by: nlnhat (17/08/2023)
        public override async Task ValidateAsync(Unit unit)
        {
            // Check trùng tên
            await _domainService.CheckDuplicatedNameAsync(unit.UnitId, unit.UnitName);
        }
        #endregion
    }
}