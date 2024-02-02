const errorCode = {
    // Mã lỗi chung
    /// <summary>
    /// Không tìm thấy dữ liệu
    /// </summary>
    NotFound: 1,
    /// <summary>
    /// Dữ liệu không hợp lệ
    /// </summary>
    InvalidData: 2,
    /// <summary>
    /// Server error
    /// </summary>
    ConflictData: 3,
    /// <summary>
    /// Server error
    /// </summary>
    ServerError: 4,
    /// <summary>
    /// Server error
    /// </summary>
    InvalidPageSize: 5,
    /// <summary>
    /// Lỗi excel
    /// </summary>
    ExcelError: 6,
    /// <summary>
    /// Tham số không hợp lệ
    /// </summary>
    InvalidParameter: 7,
    /// <summary>
    /// Thiếu tham số
    /// </summary>
    Parameterless: 8,
    /// <summary>
    /// Không thể hoàn thành tác vụ
    /// </summary>
    IncompleteTask: 9,
    /// <summary>
    /// Không xoá được hết
    /// </summary>
    IncompleteDelete: 10,

    // AuthService
    /// <summary>
    /// Signup lỗi
    /// </summary>
    AuthSignup: 101,
    /// <summary>
    /// Verify lỗi
    /// </summary>
    AuthVerify: 102,
    /// <summary>
    /// Đã tồn tại username
    /// </summary>
    AuthExistedUsername: 103,
    /// <summary>
    /// Mã xác thực không đúng
    /// </summary>
    AuthIncorrectVerifyCode: 104,
    /// <summary>
    /// Mã xác thực hết hiệu lực
    /// </summary>
    AuthTimeoutVerifyCode: 105,
    /// <summary>
    /// Xác thực email lỗi
    /// </summary>
    AuthVerifyEmail: 106,
    /// <summary>
    /// Đăng nhập không thành công
    /// </summary>
    AuthLogin: 107,
    /// <summary>
    /// Tên tài khoản không tồn tại
    /// </summary>
    AuthNotExistsUsername: 108,
    /// <summary>
    /// Mật khẩu không chính xác
    /// </summary>
    AuthIncorrectPassword: 109,

    // Problem
    /// <summary>
    /// Tạo bài toán bị lỗi
    /// </summary>
    ProblemCreate: 200,
    /// <summary>
    /// Tạo bài toán bị lỗi test case
    /// </summary>
    ProblemTestcaseCreate: 201,
    /// <summary>
    /// Lỗi source code
    /// </summary>
    ProblemSourceCode: 202,
    /// <summary>
    /// Lỗi chạy quá thời gian
    /// </summary>
    ProblemTimeLimitExceeded: 203,
};

export default errorCode;
