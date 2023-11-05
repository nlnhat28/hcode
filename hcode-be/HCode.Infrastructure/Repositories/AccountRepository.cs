using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository auth
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository tài khoản
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tài khoản theo tài khoản
        /// </summary>
        /// <returns>Tài khoản có mã tương ứng</returns>
        public async Task<Account?> GetByUsernameAsync(string? username)
        {
            var proc = $"{Procedure}GetByUsername";

            var param = new DynamicParameters();
            param.Add($"p_Username", username);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Account>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns>Tài khoản có mã tương ứng</returns>
        public async Task<Account?> LoginAsync(string? username, string? password)
        {
            var proc = $"{Procedure}Login";

            var param = new DynamicParameters();
            param.Add($"p_Username", username);
            param.Add($"p_Password", password);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Account>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
    #endregion
}

