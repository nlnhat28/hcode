//using Dapper;
//using HCode.Domain;
//using System.Data;
//using System.Text.Json;
//using static Dapper.SqlMapper;

//namespace HCode.Infrastructure
//{
//    /// <summary>
//    /// Repository nguyên vật liệu
//    /// </summary>
//    /// <typeparam name="Material">Entity nguyên vật liệu</typeparam>
//    /// Created by: nlnhat (17/08/2023)
//    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
//    {
//        #region Constructors
//        /// <summary>
//        /// Hàm tạo repository nguyên vật liệu
//        /// </summary>
//        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
//        /// Created by: nlnhat (17/08/2023)
//        public MaterialRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
//        {
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Lấy nguyên vật liệu theo theo id
//        /// </summary>
//        /// <param name="id">Mã nguyên vật liệu</param>
//        /// <returns>Nguyên vật liệu có mã tương ứng</returns>
//        public new async Task<MaterialModel> GetAsync(Guid id)
//        {
//            var proc = $"{Procedure}Get";

//            var param = new DynamicParameters();
//            param.Add("p_MaterialId", id);

//            var dictionary = new Dictionary<Guid, MaterialModel>();
//            var materials = await _unitOfWork.Connection.QueryAsync<MaterialModel, ConversionUnit, MaterialModel>(proc, (material, conversionUnit) =>
//            {
//                if (conversionUnit != null)
//                    conversionUnit.MaterialId = material.MaterialId;
//                if (!dictionary.TryGetValue(material.MaterialId, out var currentMaterial))
//                {
//                    currentMaterial = material;
//                    dictionary.Add(currentMaterial.MaterialId, currentMaterial);
//                }
//                currentMaterial.ConversionUnits ??= new List<ConversionUnit>();
//                if (conversionUnit != null)
//                    currentMaterial.ConversionUnits.Add(conversionUnit);

//                return currentMaterial;

//            },
//            param: param,
//            splitOn: "ConversionUnitId",
//            commandType: CommandType.StoredProcedure
//        );
//            var result = dictionary.Values.FirstOrDefault() ?? new MaterialModel();
//            return result;
//        }
//        /// <summary>
//        /// Lấy nguyên vật liệu theo mã
//        /// </summary>
//        /// <param name="code">Mã nguyên vật liệu</param>
//        /// <returns>Nguyên vật liệu có mã tương ứng</returns>
//        public async Task<Material> GetByCodeAsync(string code)
//        {
//            var proc = $"{Procedure}GetByCode";

//            var param = new DynamicParameters();
//            param.Add($"p_{Table}Code", code);

//            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Material>(
//                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
//            return result;
//        }
//        /// <summary>
//        /// Lấy nguyên vật liệu theo danh sách mã
//        /// </summary>
//        /// <param name="codes">Danh sách mã nguyên vật liệu</param>
//        /// <returns>Danh sách guyên vật liệu có mã tương ứng</returns>
//        public async Task<IEnumerable<Material>> GetManyByCodeAsync(List<string>? codes)
//        {
//            if (codes?.Count > 0)
//            {
//                var proc = $"{Procedure}GetManyByCode";

//                var param = new DynamicParameters();
//                var codesJson = JsonSerializer.Serialize(codes);
//                param.Add($"p_{Table}Codes", codesJson);

//                var result = await _unitOfWork.Connection.QueryAsync<Material>(
//                    proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
//                return result;
//            }
//            return new List<Material>();
//        }
//        /// <summary>
//        /// Lấy mã nguyên vật liệu lớn nhất hiện tại
//        /// </summary>
//        /// <param name="prefix">Tiền tố mã</param>
//        /// <returns>Mã nguyên vật liệu lớn nhất</returns>
//        /// Created by: nlnhat (17/08/2023)
//        public async Task<int> GetMaxCodeAsync(string prefix)
//        {
//            var proc = $"{Procedure}GetMaxCode";

//            var param = new DynamicParameters();
//            param.Add($"p_Prefix", prefix);

//            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<int>(
//                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
//            return result;
//        }
//        /// <summary>
//        /// Lọc nguyên vật liệu (Tìm kiếm, phân trang, sắp xếp, lọc theo cột)
//        /// </summary>
//        /// <param name="keySearch">Từ khoá tìm kiếm</param>
//        /// <param name="pagingModel">Các thuộc tính phân trang</param>
//        /// <param name="sortModels">Các điều kiện sắp xếp</param>
//        /// <param name="filterModels">Các điều kiện lọc</param>
//        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
//        /// Created by: nlnhat (16/08/2023)
//        public async Task<FilterResultModel<Material>> FilterAsync(
//            string? keySearch, PagingModel? pagingModel, List<SortModel>? sortModels, List<FilterModel>? filterModels)
//        {
//            var proc = $"{Procedure}Filter";

//            var param = InfrastructureHelper.GetParamFromEntity(pagingModel);
//            param.Add("p_KeySearch", keySearch);

//            if (sortModels != null)
//            {
//                var querySort = GenerateQuerySort(sortModels);
//                param.Add("p_QuerySort", querySort);
//            }
//            else param.Add("p_QuerySort", string.Empty);

//            if (filterModels != null)
//            {
//                var queryFilter = GenerateQueryFilter(filterModels);
//                param.Add("p_QueryFilter", queryFilter);
//            }
//            else param.Add("p_QueryFilter", string.Empty);

//            param.Add("p_TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
//            param.Add("p_AllRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
//            param.Add("p_PageNumberOut", dbType: DbType.Int32, direction: ParameterDirection.Output);

//            var data = await _unitOfWork.Connection.QueryAsync<Material>(
//                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

//            var totalRecord = param.Get<int>("p_TotalRecord");
//            var allRecord = param.Get<int>("p_AllRecord");
//            var pageNumber = param.Get<int>("p_PageNumberOut");

//            var result = new FilterResultModel<Material>()
//            {
//                TotalRecord = totalRecord,
//                AllRecord = allRecord,
//                PageNumber = pageNumber,
//                Data = data
//            };

//            return result;
//        }
//        /// <summary>
//        /// Lọc nguyên vật liệu để export
//        /// </summary>
//        /// <param name="keySearch">Từ khoá tìm kiếm</param>
//        /// <param name="sortModels">Các điều kiện sắp xếp</param>
//        /// <param name="filterModels">Các điều kiện lọc</param>
//        /// <returns>Kết quả nguyên vật liệu thoả mãn điều kiện lọc</returns>
//        /// Created by: nlnhat (16/08/2023)
//        public async Task<IEnumerable<MaterialModel>> ExportAsync(string? keySearch, List<SortModel>? sortModels, List<FilterModel>? filterModels)
//        {
//            var proc = $"{Procedure}Export";

//            var param = new DynamicParameters();
//            param.Add("p_KeySearch", keySearch);

//            if (sortModels != null)
//            {
//                var querySort = GenerateQuerySort(sortModels);
//                param.Add("p_QuerySort", querySort);
//            }
//            else param.Add("p_QuerySort", string.Empty);

//            if (filterModels != null)
//            {
//                var queryFilter = GenerateQueryFilter(filterModels);
//                param.Add("p_QueryFilter", queryFilter);
//            }
//            else param.Add("p_QueryFilter", string.Empty);

//            var dictionary = new Dictionary<Guid, MaterialModel>();
//            var materials = await _unitOfWork.Connection.QueryAsync<MaterialModel, ConversionUnit, MaterialModel>(proc, (material, conversionUnit) =>
//            {
//                if (conversionUnit != null)
//                {
//                    conversionUnit.MaterialId = material.MaterialId;
//                    conversionUnit.UnitName = material.UnitName;
//                }
//                if (!dictionary.TryGetValue(material.MaterialId, out var currentMaterial))
//                {
//                    currentMaterial = material;
//                    dictionary.Add(currentMaterial.MaterialId, currentMaterial);
//                }
//                currentMaterial.ConversionUnits ??= new List<ConversionUnit>();
//                if (conversionUnit != null)
//                    currentMaterial.ConversionUnits.Add(conversionUnit);

//                return currentMaterial;
//            },
//            param: param,
//            transaction: _unitOfWork.Transaction,
//            splitOn: "ConversionUnitId",
//            commandType: CommandType.StoredProcedure);

//            var result = dictionary.Values.ToList();
//            return result;
//        }
//        /// <summary>
//        /// Đếm số lượng theo các năm
//        /// </summary>
//        /// <returns>Danh sách số lượng theo năm</returns>
//        /// Created by: nlnhat (08/09/2023)
//        public async Task<IEnumerable<CountByYearModel>> CountByYear()
//        {
//            var proc = $"{Procedure}CountByYear";

//            var result = await _unitOfWork.Connection.QueryAsync<CountByYearModel>(
//                proc, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

//            return result;
//        }
//        /// <summary>
//        /// Đếm số lượng theo các kho
//        /// </summary>
//        /// <returns>Danh sách số lượng theo kho</returns>
//        /// Created by: nlnhat (08/09/2023)
//        public async Task<IEnumerable<CountByWarehouseModel>> CountByWarehouse()
//        {
//            var proc = $"{Procedure}CountByWarehouse";

//            var result = await _unitOfWork.Connection.QueryAsync<CountByWarehouseModel>(
//                proc, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

//            return result;
//        }
//        /// <summary>
//        /// Đếm số lượng theo trạng thái theo dõi
//        /// </summary>
//        /// <returns>Số lượng theo trạng thái</returns>
//        /// Created by: nlnhat (08/09/2023)
//        public async Task<CountByFollowModel> CountByFollow()
//        {
//            var proc = $"{Procedure}CountByFollow";

//            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<CountByFollowModel>(
//                proc, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

//            return result;
//        }
//        /// <summary>
//        /// Tạo các câu truy vấn Orderby 
//        /// </summary>
//        /// <param name="sortModels">Danh sách các models sắp xếp</param>
//        /// <returns>Các câu truy vấn Orderby</returns>
//        /// Created by: nlnhat (19/08/2023)
//        private static string GenerateQuerySort(List<SortModel> sortModels)
//        {
//            var queryItems = new List<string>();

//            foreach (var sortModel in sortModels)
//            {
//                if (sortModel == null)
//                    continue;

//                var column = sortModel.ColumnName;
//                var sortType = sortModel.SortType;

//                // Kiểm tra cột có trong table không
//                if (!InfrastructureHelper.IsPropertyInClass<Material>(column))
//                    continue;

//                // Phân loại sắp xếp
//                var sortTypeString = InfrastructureHelper.GetSortTypeQuery(sortType);

//                string baseQuery = $"{column} {sortTypeString}";
//                queryItems.Add(baseQuery);
//            }
//            var query = queryItems.Count > 0 ? $"ORDER BY {string.Join(", ", queryItems)} " : string.Empty;
//            return query;
//        }
//        /// <summary>
//        /// Tạo các câu truy vấn lọc
//        /// </summary>
//        /// <param name="filterModels">Danh sách các models lọc</param>
//        /// <returns>Các câu truy vấn lọc</returns>
//        /// Created by: nlnhat (19/08/2023)
//        private static string GenerateQueryFilter(List<FilterModel> filterModels)
//        {
//            var query = string.Empty;
//            var orQueries = new List<string>();
//            var orQuery = string.Empty;

//            foreach (var filterModel in filterModels)
//            {
//                var column = filterModel.ColumnName;
//                var compare = filterModel.CompareType;
//                var logic = filterModel.LogicType;
//                var value = filterModel.FilterValue;
//                if (!string.IsNullOrEmpty(value))
//                {
//                    value = InfrastructureHelper.EscapeSpecialCharacters(value);
//                }

//                var queryItem = string.Empty;

//                // Kiểm tra cột có trong table không
//                if (!InfrastructureHelper.IsPropertyInClass<Material>(column))
//                    continue;

//                // Phân loại so sánh
//                switch (compare)
//                {
//                    case CompareType.Empty:
//                        queryItem = $"({column} IS NULL OR {column} = '')";
//                        break;
//                    case CompareType.Equal:
//                        queryItem = $"({column} LIKE '{value}')";
//                        break;
//                    case CompareType.Contain:
//                        queryItem = $"({column} LIKE '%{value}%')";
//                        break;
//                    case CompareType.NotContain:
//                        queryItem = $"({column} NOT LIKE '%{value}%')";
//                        break;
//                    case CompareType.Less:
//                        queryItem = $"({column} < {value})";
//                        break;
//                    case CompareType.More:
//                        queryItem = $"({column} > {value})";
//                        break;
//                    case CompareType.LessEqual:
//                        queryItem = $"({column} < {value} OR {column} = {value})";
//                        break;
//                    case CompareType.MoreEqual:
//                        queryItem = $"({column} > {value} OR {column} = {value})";
//                        break;
//                    case CompareType.NotEqual:
//                        queryItem = $"({column} <> {value})";
//                        break;
//                    case CompareType.StartWith:
//                        queryItem = $"({column} LIKE '{value}%')";
//                        break;
//                    case CompareType.EndWidth:
//                        queryItem = $"({column} LIKE '%{value}')";
//                        break;
//                    default:
//                        break;
//                }

//                // Phân loại logic
//                switch (logic)
//                {
//                    case LogicType.And:
//                        queryItem = $" AND {queryItem} ";
//                        query += queryItem;
//                        break;
//                    case LogicType.Or:
//                        orQueries.Add(queryItem);
//                        break;
//                    default:
//                        break;
//                }
//            }

//            if (orQueries.Count > 1)
//            {
//                orQuery = $" AND ({string.Join(" OR ", orQueries)}) ";
//            }

//            query += orQuery;

//            return query;
//        }
//    }
//    #endregion
//}

