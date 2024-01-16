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

        // Lấy tài khoản theo username
        public async Task<Account?> GetByUsernameAsync(string? username)
        {
            var proc = $"{Procedure}GetByUsername";

            var param = new DynamicParameters();
            param.Add($"p_Username", username);

            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Account>(
                proc, param, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        // Đăng nhập
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

        // Cập nhật đã xác thực
        public async Task<int> UpdateVerifiedAsync(IEnumerable<Guid> accountIds)
        {
            var sql = $"UPDATE {Table} SET IsVerified = 1 WHERE {TableId} IN @accountIds";

            var param = new DynamicParameters();
            param.Add("@accountIds", accountIds);

            var result = await _unitOfWork.Connection.ExecuteAsync(
                sql, param, _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        // Cập nhật password
        public async Task<int> UpdatePasswordAsync(Guid accountId, string newPassword, string salt)
        {
            var sql = @$"UPDATE {Table} SET Password = @password, Salt = @salt
                         WHERE {TableId} = @accountId";

            var param = new DynamicParameters();
            param.Add("@accountId", accountId);
            param.Add("@password", newPassword);
            param.Add("@salt", salt);

            var result = await _unitOfWork.Connection.ExecuteAsync(
                sql, param, _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }
        #endregion
    }
}

