using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service kho từ giao diện
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class WarehouseService : BaseService<WarehouseDto, Warehouse>, IWarehouseService
    {
        #region Fields
        /// <summary>
        /// Domain service kho, validate nghiệp vụ
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IWarehouseDomainService _domainService;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service kho
        /// </summary>
        /// <param name="repository">Repository kho</param>
        /// <param name="domainService">Domain service kho</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public WarehouseService(IWarehouseRepository repository, IWarehouseDomainService domainService,
                               IStringLocalizer<Resource> resource, IMapper mapper, IUnitOfWork unitOfWork)
                             : base(repository, resource, mapper, unitOfWork)
        {
            _domainService = domainService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Map dto tạo mới sang thực thể kho
        /// </summary>
        /// <param name="warehouseDto">Dto tạo mới kho</param>
        /// <returns>Thực thể kho</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Warehouse MapCreateDtoToEntity(WarehouseDto warehouseDto)
        {
            warehouseDto.WarehouseId = Guid.NewGuid();
            warehouseDto.CreatedDate ??= DateTime.UtcNow;

            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            return warehouse;
        }
        /// <summary>
        /// Map dto cập nhật sang thực thể kho
        /// </summary>
        /// <param name="warehouseDto">Dto cập nhật kho</param>
        /// <returns>Thực thể kho</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Warehouse MapUpdateDtoToEntity(WarehouseDto warehouseDto)
        {
            warehouseDto.ModifiedDate = DateTime.UtcNow;

            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            return warehouse;
        }
        /// <summary>
        /// Validate tổng thể (input + nghiệp vụ) kho
        /// </summary>
        /// <param name="warehouse">Thực thể kho</param>
        /// Created by: nlnhat (17/08/2023)
        public override async Task ValidateAsync(Warehouse warehouse)
        {
            // Check trùng mã kho
            await _domainService.CheckDuplicatedCodeAsync(warehouse.WarehouseId, warehouse.WarehouseCode);
        }
        #endregion
    }
}