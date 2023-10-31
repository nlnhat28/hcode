namespace HCode.Domain
{
    /// <summary>
    /// Mã lỗi nội bộ
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public enum MISAErrorCode
    {
        #region Mã lỗi chung
        /// <summary>
        /// Không tìm thấy dữ liệu
        /// </summary>
        NotFound = 1000,
        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        InvalidData = 1001,
        /// <summary>
        /// Server error
        /// </summary>
        ConflictData = 1002,
        /// <summary>
        /// Server error
        /// </summary>
        ServerError = 1003,
        /// <summary>
        /// Server error
        /// </summary>
        InvalidPageSize = 1004,
        /// <summary>
        /// Lỗi excel
        /// </summary>
        ExcelError = 1005,
        /// <summary>
        /// Tham số không hợp lệ
        /// </summary>
        InvalidParameter = 1006,
        /// <summary>
        /// Thiếu tham số
        /// </summary>
        Parameterless = 1007,
        /// <summary>
        /// Không thể hoàn thành tác vụ
        /// </summary>
        IncompleteTask = 1008,
        /// <summary>
        /// Không xoá được hết
        /// </summary>
        IncompleteDelete = 1009,
        #endregion

        #region Mã lỗi phân hệ nguyên vật liệu
        /// <summary>
        /// Mã nguyên vật liệu sai định dạng
        /// </summary>
        MaterialCodeWrongFormat = 2001,
        /// <summary>
        /// Trùng mã nguyên vật liệu
        /// </summary>
        MaterialCodeDuplicated = 2002,
        /// <summary>
        /// Trùng mã nguyên vật liệu
        /// </summary>
        MaterialCodeOutOfRange = 2003,
        /// <summary>
        /// Không tìm thấy nguyên vật liệu
        /// </summary>
        MaterialNotFound = 2004,
        /// <summary>
        /// Lỗi xuất file excel
        /// </summary>
        MaterialExportError = 2005,
        /// <summary>
        /// Lỗi tên rỗng
        /// </summary>
        MaterialNameNull = 2006,
        /// <summary>
        /// Lỗi tạo nguyên vật liệu
        /// </summary>
        MaterialCreate = 2010,
        /// <summary>
        /// Lỗi file nhập khẩu
        /// </summary>
        MaterialNotValidImportFile = 2011,
        #endregion

        #region Mã lỗi phân hệ kho
        /// <summary>
        /// Trùng mã nhà kho
        /// </summary>
        WarehouseCodeDuplicated = 3001,
        /// <summary>
        /// Không tìm thấy nhà kho
        /// </summary>
        WarehouseNotFound = 3002,
        #endregion

        #region Mã lỗi phân hệ đơn vị tính
        /// <summary>
        /// Trùng tên đơn vị tính
        /// </summary>
        UnitNameDuplicated = 4001,
        /// <summary>
        /// Không tìm thấy đơn vị tính
        /// </summary>
        UnitNotFound = 4002,
        #endregion

        #region Mã lỗi phân hệ đơn vị chuyển đổi
        /// <summary>
        /// Trùng đơn vị chuyển đổi
        /// </summary>
        ConversionUnitDuplicated = 5001,
        /// <summary>
        /// Trùng đơn vị tính chính
        /// </summary>
        ConversionUnitDuplicatedUnit = 5002,
        /// <summary>
        /// Không tìm thấy đơn vị chuyển đổi
        /// </summary>
        ConversionUnitNotFound = 5003,
        #endregion
    }
}
