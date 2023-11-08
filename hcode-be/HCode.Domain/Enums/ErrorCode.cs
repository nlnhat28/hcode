namespace HCode.Domain
{
    /// <summary>
    /// Mã lỗi nội bộ
    /// </summary>
    /// Created by: nlnhat (17/07/2023)
    public enum ErrorCode
    {
        #region Mã lỗi chung
        /// <summary>
        /// Không tìm thấy dữ liệu
        /// </summary>
        NotFound = 1,
        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        InvalidData = 2,
        /// <summary>
        /// Server error
        /// </summary>
        ConflictData = 3,
        /// <summary>
        /// Server error
        /// </summary>
        ServerError = 4,
        /// <summary>
        /// Server error
        /// </summary>
        InvalidPageSize = 5,
        /// <summary>
        /// Lỗi excel
        /// </summary>
        ExcelError = 6,
        /// <summary>
        /// Tham số không hợp lệ
        /// </summary>
        InvalidParameter = 7,
        /// <summary>
        /// Thiếu tham số
        /// </summary>
        Parameterless = 8,
        /// <summary>
        /// Không thể hoàn thành tác vụ
        /// </summary>
        IncompleteTask = 9,
        /// <summary>
        /// Không xoá được hết
        /// </summary>
        IncompleteDelete = 10,
        #endregion

        #region AuthService
        /// <summary>
        /// Signup lỗi
        /// </summary>
        AuthSignup = 101,
        /// <summary>
        /// Verify lỗi
        /// </summary>
        AuthVerify = 102,
        /// <summary>
        /// Đã tồn tại username
        /// </summary>
        AuthExistedUsername = 103,
        /// <summary>
        /// Mã xác thực không đúng
        /// </summary>
        AuthIncorrectVerifyCode = 104,
        /// <summary>
        /// Mã xác thực hết hiệu lực
        /// </summary>
        AuthTimeoutVerifyCode = 105, 
        /// <summary>
        /// Xác thực email lỗi
        /// </summary>
        AuthVerifyError = 106,
        #endregion
    }
}
