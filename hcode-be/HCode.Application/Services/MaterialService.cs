using AutoMapper;
using Microsoft.Extensions.Localization;
using HCode.Domain;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service nguyên vật liệu từ giao diện
    /// </summary> 
    /// Created by: nlnhat (15/08/2023)
    public class MaterialService : BaseService<MaterialDto, Material>, IMaterialService
    {
        #region Fields
        /// <summary>
        /// Repository nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private new readonly IMaterialRepository _repository;
        /// <summary>
        /// Repository nhật ký nguyên vật liệu
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IMaterialAuditRepository _materialAuditRepository;
        /// <summary>
        /// Repository đơn vị chuyển đổi
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IConversionUnitRepository _conversionUnitRepository;
        /// <summary>
        /// Repository đơn vị tính
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IUnitRepository _unitRepository;
        /// <summary>
        /// Repository kho
        /// </summary>
        /// Created by: nlnhat (18/08/2023)
        private readonly IWarehouseRepository _warehouseRepository;
        /// <summary>
        /// Domain service nguyên vật liệu, validate nghiệp vụ
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IMaterialDomainService _domainService;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service nguyên vật liệu
        /// </summary>
        /// <param name="repository">Repository nguyên vật liệu</param>
        /// <param name="domainService">Domain service nguyên vật liệu</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map nguyên vật liệu</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public MaterialService(IMaterialRepository repository,
                               IMaterialAuditRepository materialAuditRepository,
                               IUnitRepository unitRepository,
                               IWarehouseRepository warehouseRepository,
                               IConversionUnitRepository conversionUnitrepository,
                               IMaterialDomainService domainService,
                               IStringLocalizer<Resource> resource, IMapper mapper, IUnitOfWork unitOfWork)
                             : base(repository, resource, mapper, unitOfWork)
        {
            _repository = repository;
            _materialAuditRepository = materialAuditRepository;
            _unitRepository = unitRepository;
            _warehouseRepository = warehouseRepository;
            _conversionUnitRepository = conversionUnitrepository;
            _domainService = domainService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Map dto tạo mới sang thực thể nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto tạo mới nguyên vật liệu</param>
        /// <returns>Thực thể nguyên vật liệu</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Material MapCreateDtoToEntity(MaterialDto materialDto)
        {
            materialDto.MaterialId = Guid.NewGuid();
            materialDto.IsUnfollow ??= false;
            materialDto.CreatedDate ??= DateTime.UtcNow;

            var material = _mapper.Map<Material>(materialDto);

            return material;
        }
        /// <summary>
        /// Map dto cập nhật sang thực thể nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto cập nhật nguyên vật liệu</param>
        /// <returns>Thực thể nguyên vật liệu</returns>
        /// Created by: nlnhat (17/08/2023)
        public override Material MapUpdateDtoToEntity(MaterialDto materialDto)
        {
            materialDto.ModifiedDate = DateTime.UtcNow;

            var material = _mapper.Map<Material>(materialDto);

            return material;
        }
        /// <summary>
        /// Map đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnitDtos">Danh sách dto đơn vị chuyển đổi</param>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Danh sách đơn vị chuyển đổi tạo mới</returns>
        /// Created by: nlnhat (17/08/2023)
        public List<ConversionUnit> MapCreateConversionUnits(List<ConversionUnitDto>? conversionUnitDtos, Guid materialId)
        {
            var conversionUnits = _mapper.Map<List<ConversionUnit>>(conversionUnitDtos);
            conversionUnits.ForEach(unit =>
            {
                unit.ConversionUnitId = Guid.NewGuid();
                unit.MaterialId = materialId;
            });
            return conversionUnits;
        }
        /// <summary>
        /// Validate tổng thể (input + nghiệp vụ) nguyên vật liệu
        /// </summary>
        /// <param name="material">Entity nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        public override async Task ValidateAsync(Material material)
        {
            // Check trùng mã nguyên vật liệu
            await _domainService.CheckDuplicatedCodeAsync(material.MaterialId, material.MaterialCode);

            // Check tồn tại đơn vị tính
            await _domainService.CheckExistUnitAsync(material.UnitId);

            // Check tồn tại nhà kho
            if (material.WarehouseId != null)
            {
                await _domainService.CheckExistWarehouseAsync((Guid)material.WarehouseId);
            }

            // Check khoảng code cho phép nếu code có dạng TNVL + Số
            var materialCode = material.MaterialCode;
            var materialName = material.MaterialName;
            await ValidateRangeCodeAsync(materialCode, materialName);
        }
        /// <summary>
        /// Check khoảng code nguyên vật liệu
        /// </summary>
        /// <param name="materialCode">Mã nguyên vật liệu</param>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// Created by: nlnhat (17/08/2023)
        private async Task ValidateRangeCodeAsync(string materialCode, string materialName)
        {
            var prefix = ApplicationHelper.GetPrefixCode(materialName);

            var pattern = string.Format(@"^{0}(\d+)$", prefix);
            var matchingCode = Regex.Match(materialCode.ToUpper(), pattern);

            if (matchingCode.Success)
            {
                var number = Convert.ToInt32(matchingCode.Groups[1].Value);
                await _domainService.CheckRangeCodeAsync(prefix, number);
            }
        }
        /// <summary>
        /// Validate danh sách đơn vị chuyển đổi
        /// </summary>
        /// <param name="conversionUnits">Danh sách đơn vị chuyển đổi thêm và cập nhật</param>
        /// <param name="unitId">Id của đơn vị tính chính</param>
        /// <exception cref="ValidateException">Đơn vị chuyển đổi bị trùng nhau hoặc trùng đơn vị tính chính</exception>
        public async Task ValidateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid unitId)
        {
            foreach (var unit in conversionUnits)
            {
                // Check trùng đơn vị tính chính
                if (unit.DestinationUnitId == unitId)
                {
                    throw new ValidateException(
                        MISAErrorCode.ConversionUnitDuplicatedUnit,
                        $"{_resource["ConversionUnit"]} <{unit.DestinationUnitName}> {_resource["Duplicated"]} {_resource["Unit"]}",
                        new ExceptionData("DestinationUnit", unit.ConversionUnitId.ToString()));
                }
                // Check các tên đơn vị chuyển đổi bị trùng nhau
                else if (conversionUnits.Any(otherUnit =>
                    unit.DestinationUnitId == otherUnit.DestinationUnitId && unit != otherUnit))
                {
                    throw new ValidateException(
                        MISAErrorCode.ConversionUnitDuplicated,
                        $"{_resource["ConversionUnit"]} <{unit.DestinationUnitName}> {_resource["Duplicated"]}",
                        new ExceptionData("DestinationUnit", unit.ConversionUnitId.ToString()));
                }
            }

            // Check tồn tại đơn vị muốn chuyển đổi hay không
            var conversionUnitIds = conversionUnits.Select(conversionUnit => conversionUnit.DestinationUnitId).ToList();
            await _domainService.CheckExistDestinationUnitsAsync(conversionUnitIds);
        }
        /// <summary>
        /// Validate danh sách đơn vị chuyển đổi cập nhật
        /// </summary>
        /// <param name="conversionUnits">Danh sách đơn vị chuyển đổi cập nhật</param>
        /// <param name="materialId">Id của nguyên vật liệu</param>
        public async Task ValidateUpdateConversionUnitsAsync(List<ConversionUnit> conversionUnits, Guid materialId)
        {
            // Check tồn tại đơn vị chuyển đổi update hay không
            var conversionUnitIds = conversionUnits.Select(conversionUnit => conversionUnit.ConversionUnitId).ToList();
            await _domainService.CheckExistConversionUnitsAsync(conversionUnitIds, materialId);
        }
        /// <summary>
        /// Lấy nguyên vật liệu theo id
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Dto nguyên vật liệu được tìm thấy</returns>
        /// <exception cref="NotFoundException">Không tìm thấy nguyên vật liệu</exception>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<MaterialDto> GetAsync(Guid materialId)
        {
            var material = await _repository.GetAsync(materialId) ??
                throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"],
                    new ExceptionData("MaterialId", materialId.ToString()));

            var materialDto = _mapper.Map<MaterialDto>(material);
            materialDto.ConversionUnits = _mapper.Map<List<ConversionUnitDto>>(material.ConversionUnits);

            return materialDto;
        }
        /// <summary>
        /// Tạo mới 1 nguyên vật liệu
        /// </summary>
        /// <param name="materialDto">Dto nguyên vật liệu</param>
        /// <returns>Id nguyên vật liệu mới</returns>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<Guid> CreateAsync(MaterialDto materialDto)
        {
            var material = MapCreateDtoToEntity(materialDto);
            await ValidateAsync(material);

            var materialAudit = new MaterialAudit(Guid.NewGuid(), material.MaterialId, EditMode.Create, DateTime.UtcNow);

            var conversionUnitDtos = materialDto.ConversionUnits;

            // Map và validate đơn vị chuyển đổi
            var conversionUnits = MapCreateConversionUnits(conversionUnitDtos, material.MaterialId);
            if (conversionUnits != null)
                await ValidateConversionUnitsAsync(conversionUnits, material.UnitId);

            // Bắt đầu transaction
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.InsertAsync(material);

                await _materialAuditRepository.InsertAsync(materialAudit);

                await _conversionUnitRepository.InsertManyAsync(conversionUnits);

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Cập nhật 1 nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <param name="materialDto">Dto nguyên vật liệu</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Created by: nlnhat (18/08/2023)
        public override async Task<int> UpdateAsync(Guid materialId, MaterialDto materialDto)
        {
            // Kiểm tra nguyên vật liệu tồn tại không
            _ = await _repository.GetAsync(materialId) ??
                throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"],
                    new ExceptionData(materialId.ToString()));

            // Map và validate nguyên vật liệu
            var material = MapUpdateDtoToEntity(materialDto);
            await ValidateAsync(material);

            // Phân loại edit mode 
            var conversionUnitDtos = materialDto.ConversionUnits;

            var createConversionUnits = new List<ConversionUnit>();
            var updateConversionUnits = new List<ConversionUnit>();
            var deleteConversionUnits = new List<Guid>();

            if (conversionUnitDtos != null)
            {
                foreach (var unitDto in conversionUnitDtos)
                {
                    var unit = _mapper.Map<ConversionUnit>(unitDto);

                    switch (unitDto.EditMode)
                    {
                        case EditMode.Create:
                            {
                                unit.MaterialId = material.MaterialId;
                                createConversionUnits.Add(unit);
                                break;
                            }
                        case EditMode.Update:
                            {
                                updateConversionUnits.Add(unit);
                                break;
                            }
                        case EditMode.Delete:
                            {
                                deleteConversionUnits.Add(unit.ConversionUnitId);
                                break;
                            }
                        default: break;
                    }
                }
            }
            // Validate đơn vị chuyển đổi
            var conversionUnits = createConversionUnits.Concat(updateConversionUnits).ToList();
            if (conversionUnits != null)
                await ValidateConversionUnitsAsync(conversionUnits, material.UnitId);

            if (updateConversionUnits != null)
                await ValidateUpdateConversionUnitsAsync(updateConversionUnits, material.MaterialId);

            // Bắt đầu transaction
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.UpdateAsync(material);

                await _conversionUnitRepository.InsertManyAsync(createConversionUnits);

                await _conversionUnitRepository.UpdateManyAsync(updateConversionUnits);

                await _conversionUnitRepository.DeleteManyAsync(deleteConversionUnits);

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Nhập nguyên vật liệu từ excel
        /// </summary>
        /// <param name="materialDtos">Danh sách dto tạo mới nguyên vật liệu</param>
        /// <returns>Danh sách id tạo mới</returns>
        /// Created by: nlnhat (13/09/2023)
        public async Task<IEnumerable<Guid>> ImportFromExcelAsync(IEnumerable<MaterialDto> materialDtos)
        {
            var materials = new List<Material>();
            var conversionUnits = new List<ConversionUnit>();
            var ids = new List<Guid>();

            // Map dtos sang entities
            foreach (var materialDto in materialDtos)
            {
                var material = MapCreateDtoToEntity(materialDto);
                materials.Add(material);
                ids.Add(material.MaterialId);

                var conversionUnitDtos = materialDto.ConversionUnits;
                var _conversionUnits = MapCreateConversionUnits(conversionUnitDtos, material.MaterialId);
                conversionUnits?.AddRange(_conversionUnits);
            }

            // Tạo nhật ký
            var materialAudits = materials.Select(material
                => new MaterialAudit(Guid.NewGuid(), material.MaterialId, EditMode.Create, DateTime.UtcNow));

            // Bắt đầu transaction
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _repository.InsertManyAsync(materials);

                await _materialAuditRepository.InsertManyAsync(materialAudits);

                await _conversionUnitRepository.InsertManyAsync(conversionUnits);

                await _unitOfWork.CommitAsync();

                return ids;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return new List<Guid>();
            }
        }
        /// <summary>
        /// Xoá nguyên vật liệu
        /// </summary>
        /// <param name="materialId">Id nguyên vật liệu</param>
        /// <returns>Số lượng bản ghi thay đổi</returns>
        public override async Task<int> DeleteAsync(Guid materialId)
        {
            var materialAudit = new MaterialAudit(Guid.NewGuid(), materialId, EditMode.Delete, DateTime.UtcNow);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.DeleteAsync(materialId);

                await _materialAuditRepository.InsertAsync(materialAudit);

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Xoá nhiều nguyên vật liệu
        /// </summary>
        /// <param name="materialIds">Danh sách id muốn xoá</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: nlnhat (18/07/2023)
        public override async Task<int> DeleteManyAsync(IEnumerable<Guid> materialIds)
        {
            // Tạo nhật ký
            var materialAudits = materialIds.Select(materialId
                => new MaterialAudit(Guid.NewGuid(), materialId, EditMode.Delete, DateTime.UtcNow));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var result = await _repository.DeleteManyAsync(materialIds);

                await _materialAuditRepository.InsertManyAsync(materialAudits);

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Lấy mã mới nguyên vật liệu
        /// </summary>
        /// <param name="materialName">Tiền tố của mã</param>
        /// <returns>Mã mới nguyên vật liệu</returns>
        /// Created by: nlnhat(17/08/2023)
        public async Task<string> GetNewCodeAsync(string materialName)
        {
            var prefix = ApplicationHelper.GetPrefixCode(materialName);

            var maxCode = await _repository.GetMaxCodeAsync(prefix);
            var result = $"{prefix}{++maxCode}";
            return result;
        }
        /// <summary>
        /// Lọc nâng cao nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="pagingModel">Các thuộc tính phân trang</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<FilterResultModel<Material>> FilterAsync(
            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels)
        {
            if (pagingModel != null && pagingModel.PageNumber < 1)
            {
                pagingModel.PageNumber = 1;
            }

            var result = await _repository.FilterAsync(keySearch, pagingModel, sortModels, filterModels);
            return result;
        }
        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="keySearch">Từ khoá tìm kiếm</param>
        /// <param name="sortModels">Các điều kiện sắp xếp</param>
        /// <param name="filterModels">Các điều kiện lọc</param>
        /// <returns>Excel data</returns>
        /// <exception cref="NotFoundException">Không tìm thấy nguyên vật liệu</exception>
        /// Created by: nlnhat (18/08/2023)
        public async Task<MemoryStream> ExportToExcelAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels)
        {
            //Lấy data
            var materials = await _repository.ExportAsync(keySearch, sortModels, filterModels) ??
                     throw new NotFoundException(
                    MISAErrorCode.MaterialNotFound,
                    _resource["MaterialNotFound"]);

            var materialExcelDtos = new List<MaterialExcelDto>();
            var index = 0;
            foreach (var material in materials)
            {
                var materialDto = _mapper.Map<MaterialExcelDto>(material);
                materialDto.Index = ++index;
                materialDto.ConversionUnits = _mapper.Map<List<ConversionUnitExcelDto>>(material.ConversionUnits);
                materialExcelDtos.Add(materialDto);
            }

            var startRow = 3;
            var materialFirstColumn = MaterialExcelDto.FirstColumn;
            var materialLastColumn = MaterialExcelDto.LastColumn;

            var conversionFirstColumn = ConversionUnitExcelDto.FirstColumn;
            var conversionLastColumn = ConversionUnitExcelDto.LastColumn;

            try
            {
                // Khởi tạo excel package
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using var package = new ExcelPackage();
                var sheetName = _resource["MaterialsList"];
                var sheet = package.Workbook.Worksheets.Add(sheetName);

                // Get type
                var materialType = typeof(MaterialExcelDto);
                var conversionType = typeof(ConversionUnitExcelDto);

                // Get tên bảng
                var materialTableName = materialType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                var conversionTableName = conversionType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

                // Get properties
                var materialProperties = materialType.GetProperties();
                var conversionProperties = conversionType.GetProperties();
                var properties = materialProperties.Concat(conversionProperties);

                // Đổ dữ liệu
                var row = startRow + 2;
                foreach (var material in materialExcelDtos)
                {
                    var conversionUnits = material.ConversionUnits;
                    var conversionLength = conversionUnits?.Count ?? 0;

                    var range = sheet.Cells[$"{materialFirstColumn}{row}:{conversionLastColumn}{row + Math.Max(0, conversionLength - 1)}"];
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);

                    if (conversionLength == 0)
                    {
                        var tempMaterial = new List<MaterialExcelDto>
                        {
                            material
                        };
                        var cells = sheet.Cells[$"{materialFirstColumn}{row}"];
                        cells.LoadFromCollection(tempMaterial);
                        row++;
                    }
                    else
                    {
                        foreach (var property in materialProperties)
                        {
                            var excelDisplayAttribute = property.GetCustomAttribute<ExcelDisplayAttribute>();
                            if (excelDisplayAttribute == null)
                                continue;

                            var columnLetter = excelDisplayAttribute.ColumnLetter;
                            var cells = sheet.Cells[$"{columnLetter}{row}:{columnLetter}{row + conversionLength - 1}"];

                            cells.Merge = true;
                            cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cells.Value = property.GetValue(material);
                        }
                        sheet.Cells[$"{conversionFirstColumn}{row}"].LoadFromCollection(conversionUnits);
                        row += conversionLength;
                    }
                }

                // Chỉnh lại style các cột
                foreach (var property in properties)
                {
                    // Lấy thông tin cột
                    var excelDisplayAttribute = property.GetCustomAttribute<ExcelDisplayAttribute>();
                    if (excelDisplayAttribute == null)
                        continue;

                    var columnLetter = excelDisplayAttribute.ColumnLetter;
                    var columnName = excelDisplayAttribute.ColumnName;
                    var horizontalAlign = excelDisplayAttribute?.HorizontalAlignment ?? ExcelHorizontalAlignment.Left;
                    var numberFormat = excelDisplayAttribute?.NumberFormat;

                    // Chỉnh phần header
                    var headerCell = sheet.Cells[$"{columnLetter}{startRow + 1}"];
                    headerCell.Value = columnName;
                    headerCell.Style.Font.Bold = true;

                    // Format dữ liệu
                    if (numberFormat != null)
                    {
                        sheet.Cells[$"{columnLetter}:{columnLetter}"].Style.Numberformat.Format = numberFormat;
                    }

                    // Chỉnh cho 1 cột
                    var column = sheet.Cells[$"{columnLetter}:{columnLetter}"];
                    column.AutoFitColumns();
                    column.Style.HorizontalAlignment = horizontalAlign;
                    column.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Thêm border
                    sheet.Cells[$"{columnLetter}{startRow}:{columnLetter}{row - 1}"].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                }
                // Chỉnh tiêu đề
                var titleRange = sheet.Cells[$"{materialFirstColumn}1:{conversionLastColumn}1"];
                titleRange.Merge = true;
                titleRange.Value = sheetName;
                titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.Size = 22;

                // Chỉnh tên bảng material
                var materialNameRange = sheet.Cells[$"{materialFirstColumn}{startRow}:{materialLastColumn}{startRow}"];
                materialNameRange.Merge = true;
                materialNameRange.Value = materialTableName;
                materialNameRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                materialNameRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                materialNameRange.Style.Font.Bold = true;
                materialNameRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                materialNameRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                // Chỉnh tên bảng conversion
                var converisonNameRange = sheet.Cells[$"{conversionFirstColumn}{startRow}:{conversionLastColumn}{startRow}"];
                converisonNameRange.Merge = true;
                converisonNameRange.Value = conversionTableName;
                converisonNameRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                converisonNameRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                converisonNameRange.Style.Font.Bold = true;
                converisonNameRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                converisonNameRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                // Chỉnh header
                sheet.Rows[startRow].Height = 22;
                sheet.Rows[startRow + 1].Height = 22;

                var materialHeader = sheet.Cells[$"{materialFirstColumn}{startRow + 1}:{materialLastColumn}{startRow + 1}"];
                materialHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                materialHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                materialHeader.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                var conversionHeader = sheet.Cells[$"{conversionFirstColumn}{startRow + 1}:{conversionLastColumn}{startRow + 1}"];
                conversionHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                conversionHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                conversionHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;

                return memoryStream;
            }
            catch (Exception exception)
            {
                throw new IncompleteException(MISAErrorCode.MaterialExportError, _resource["MaterialExportError"], exception.Message);
            }
        }
        /// <summary>
        ///  Map dữ liệu từ file nhập khẩu
        /// </summary>
        /// <param name="file">File excel</param>
        /// <returns></returns>
        /// Created by: nlnhat (16/08/2023)
        public async Task<IEnumerable<MaterialDto>> MapImportFileAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();

            var invalidFileException = new ValidateException(
                MISAErrorCode.MaterialNotValidImportFile,
                _resource["MaterialNotValidImportFile"],
                new ExceptionData("DownloadLink", null, ExceptionKey.FormItem, "FormItem"));

            if (sheet != null)
            {
                var validHeaders = new List<string>() { "Mã (*)", "Tên (*)", "SL tồn tối thiểu", "Đơn vị tính (*)",
                    "Kho ngầm định", "Hạn sử dụng", "Đơn vị thời gian", "Ghi chú", "Đơn vị chuyển đổi", "Tỷ lệ chuyển đổi", "Phép tính"};

                for (var columnIndex = 1; columnIndex <= validHeaders.Count; columnIndex++)
                {
                    if (sheet.Cells[2, columnIndex].Text != validHeaders[columnIndex - 1])
                    {
                        throw invalidFileException;
                    }
                }

                var rowIndex = 3;
                var materialImportDtos = new List<MaterialImportDto>();

                while (true)
                {
                    var rowData = new List<string>();

                    for (var columnIndex = 1; columnIndex <= validHeaders.Count; columnIndex++)
                    {
                        rowData.Add(sheet.Cells[rowIndex, columnIndex].Text);
                    }

                    if (rowData.All(data => string.IsNullOrEmpty(data)))
                    {
                        break;
                    }

                    // Extract thông tin từ file excel
                    var conversionUnitImportDto = new ConversionUnitImportDto
                    (
                        destinationUnitName: sheet.Cells[$"I{rowIndex}"].Text,
                        rate: sheet.Cells[$"J{rowIndex}"].Text,
                        operatorName: sheet.Cells[$"K{rowIndex}"].Text
                    );

                    var materialImportDto = new MaterialImportDto
                    (
                        materialCode: sheet.Cells[$"A{rowIndex}"].Text,
                        materialName: sheet.Cells[$"B{rowIndex}"].Text,
                        minimumInventory: sheet.Cells[$"C{rowIndex}"].Text,
                        unitName: sheet.Cells[$"D{rowIndex}"].Text,
                        warehouseCode: sheet.Cells[$"E{rowIndex}"].Text,
                        expiryTime: sheet.Cells[$"F{rowIndex}"].Text,
                        timeUnit: sheet.Cells[$"G{rowIndex}"].Text,
                        note: sheet.Cells[$"H{rowIndex}"].Text,
                        conversionUnitImportDto: conversionUnitImportDto
                    );

                    rowIndex++;
                    materialImportDtos.Add(materialImportDto);
                };

                // Nhóm các nguyên vật liệu có cùng đơn vị chuyển đổi
                var materialDtos = materialImportDtos
                .GroupBy(m => new
                {
                    m.MaterialCode,
                    m.MaterialName,
                    m.UnitName,
                    m.WarehouseCode,
                    m.ExpiryTime,
                    m.TimeUnit,
                    m.MinimumInventory,
                    m.Note,
                })
                .Select(group => new MaterialDto()
                {
                    MaterialCode = group.Key.MaterialCode,
                    MaterialName = group.Key.MaterialName,
                    UnitName = group.Key.UnitName,
                    WarehouseCode = group.Key.WarehouseCode,
                    ExpiryTime = group.Key.ExpiryTime,
                    TimeUnit = group.Key.TimeUnit,
                    MinimumInventory = group.Key.MinimumInventory,
                    Note = group.Key.Note,
                    ConversionUnits = group.Select(m => _mapper.Map<ConversionUnitDto>(m.ConversionUnit))
                                           .Where(unit => unit != null).ToList()
                })
                .ToList();

                var result = await ValidateImportMaterialsAsync(materialDtos);
                return materialDtos;
            }
            else
            {
                throw invalidFileException;
            }
        }
        /// <summary>
        /// Map dữ liệu import sang dto hiển thị cho người dùng kết quả check dữ liệu import
        /// </summary>
        /// <param name="materialDtos">Danh sách dto import nguyên vật liệu</param>
        /// <returns>Danh sách dto import nguyên vật liệu kèm dữ liệu hợp lệ hay không</returns>
        /// Created by: nlnhat (12/09/2023)
        public async Task<List<MaterialDto>> ValidateImportMaterialsAsync(List<MaterialDto> materialDtos)
        {
            // Dữ liệu import
            var materialDtoCodes = materialDtos.Select(m => m.MaterialCode).ToList();
            var warehouseCodes = materialDtos.Select(m => m.WarehouseCode ?? string.Empty).ToList();
            var unitNames = materialDtos.Select(m => m.UnitName ?? string.Empty).ToList();
            var destinationUnitNames = materialDtos.SelectMany(materialDto => materialDto.ConversionUnits ?? new List<ConversionUnitDto>())
               .Select(conversionUnit => conversionUnit.DestinationUnitName ?? string.Empty).ToList();

            // Lấy dữ liệu import trong db
            var materials = await _repository.GetManyByCodeAsync(materialDtoCodes);
            var warehouses = await _warehouseRepository.GetManyByCodeAsync(warehouseCodes);
            var units = await _unitRepository.GetManyByNameAsync(unitNames);
            var destinationUnits = await _unitRepository.GetManyByNameAsync(destinationUnitNames);

            // Check xung đột với bản ghi trong db
            foreach (var materialDto in materialDtos)
            {
                var materialCode = materialDto.MaterialCode;
                var warehouseCode = materialDto.WarehouseCode;
                var unitName = materialDto.UnitName;

                // Những đối tượng tồn tại trong db
                var material = materials.Where(material => material.MaterialCode == materialCode).FirstOrDefault();
                var warehouse = warehouses.Where(warehouse => warehouse.WarehouseCode == warehouseCode).FirstOrDefault();
                var unit = units.Where(unit => unit.UnitName == unitName).FirstOrDefault();

                materialDto.MaterialId = Guid.NewGuid();
                materialDto.IsValid = true;
                materialDto.ValidateDescription = _resource["Valid"];

                // Check trống code
                if (string.IsNullOrEmpty(materialDto.MaterialCode))
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["MaterialCode"]} {_resource["CannotEmpty"]}";
                }
                // Check trống tên
                else if (string.IsNullOrEmpty(materialDto.MaterialName))
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["MaterialName"]} {_resource["CannotEmpty"]}";
                }
                // Check trống đơn vị tính
                else if (string.IsNullOrEmpty(materialDto.UnitName))
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["Unit"]} {_resource["CannotEmpty"]}";
                }
                // Trùng code với bản ghi trong db
                else if (material != null)
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["MaterialCode"]} <{materialCode}> {_resource["Used"]}";
                }

                // Check tồn tại kho
                else if (warehouse == null)
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["WarehouseCode"]} <{warehouseCode}> {_resource["NotExist"]}";
                }

                // Check tồn tại đơn vị tính
                else if (unit == null)
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["Unit"]} <{unitName}> {_resource["NotExist"]}";
                }

                else
                {
                    materialDto.UnitId = unit.UnitId;
                    materialDto.WarehouseId = warehouse.WarehouseId;
                }

                // Check đơn vị chuyển đổi
                if (materialDto.IsValid == true)
                {
                    var conversionUnitDtos = materialDto.ConversionUnits;
                    if (conversionUnitDtos?.Count > 0)
                    {
                        foreach (var conversionUnit in conversionUnitDtos)
                        {
                            var destinationUnitName = conversionUnit.DestinationUnitName;

                            var destinationUnit = destinationUnits.Where(unit => unit.UnitName == destinationUnitName).FirstOrDefault();
                            var destinationUnitId = destinationUnit?.UnitId;

                            if (destinationUnit != null)
                            {
                                conversionUnit.DestinationUnitId = destinationUnit.UnitId;
                            }

                            // Check tồn tại đơn vị muốn chuyển đổi không
                            if (destinationUnit == null)
                            {
                                materialDto.IsValid = false;
                                materialDto.ValidateDescription
                                    = $"{_resource["ConversionUnit"]} <{destinationUnitName}> {_resource["NotExist"]}";
                            }

                            // Check đơn vị chuyển đổi bị trùng đơn vị tính
                            else if (destinationUnitId == materialDto.UnitId)
                            {
                                materialDto.IsValid = false;
                                materialDto.ValidateDescription
                                    = $"{_resource["ConversionUnit"]} <{destinationUnitName}> {_resource["Duplicated"]} {_resource["Unit"]}";
                            }

                            // Check đơn vị chuyển đổi bị trùng nhau
                            else if (conversionUnitDtos.Any(otherUnit =>
                                conversionUnit.DestinationUnitId == otherUnit.DestinationUnitId && conversionUnit != otherUnit))
                            {
                                materialDto.IsValid = false;
                                materialDto.ValidateDescription =
                                    $"{_resource["ConversionUnit"]} <{destinationUnitName}> {_resource["Duplicated"]}";
                            }
                        }
                    }
                } 
            }

            // Check xung đột lẫn nhau giữa các bản ghi hợp lệ
            var validMaterialDtos = materialDtos.Where(materialDto => materialDto.IsValid == true).Reverse();

            foreach (var materialDto in validMaterialDtos)
            {
                // Trùng code nhau
                var materialCode = materialDto.MaterialCode;

                if (validMaterialDtos.Any(other => other.MaterialCode == materialCode && other != materialDto))
                {
                    materialDto.IsValid = false;
                    materialDto.ValidateDescription = $"{_resource["MaterialCode"]} <{materialCode}> {_resource["Duplicated"]}";
                }
            }
            return materialDtos;
        }
        /// <summary>
        /// Đếm số lượng theo các năm
        /// </summary>
        /// <returns>Danh sách số lượng theo năm</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<IEnumerable<CountByYearModel>> CountByYear()
        {
            var result = await _materialAuditRepository.CountByYear();
            return result;
        }
        /// <summary>
        /// Đếm số lượng theo các kho
        /// </summary>
        /// <returns>Danh sách số lượng theo kho</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<IEnumerable<CountByWarehouseModel>> CountByWarehouse()
        {
            var result = await _repository.CountByWarehouse();
            return result;
        }
        /// <summary>
        /// Đếm số lượng theo trạng thái theo dõi
        /// </summary>
        /// <returns>Số lượng theo trạng thái</returns>
        /// Created by: nlnhat (08/09/2023)
        public async Task<CountByFollowModel> CountByFollow()
        {
            var result = await _repository.CountByFollow();
            return result;
        }
        #endregion
    }
}