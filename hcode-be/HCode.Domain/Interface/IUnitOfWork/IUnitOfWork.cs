using System.Data.Common;

namespace HCode.Domain
{
    /// <summary>
    /// Giao diện unit of work, quản lý kết nối và giao dịch với cơ sở dữ liệu
    /// </summary>
    /// Created by: nlnhat (18/07/2023)
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Kết nối với cơ sở dữ liệu
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        DbConnection Connection { get; }
        /// <summary>
        /// Giao dịch với cơ sở dữ liệu
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        DbTransaction? Transaction { get; }
        /// <summary>
        /// Bắt đầu giao dịch
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Bắt đầu giao dịch bất đồng bộ
        /// </summary>
        Task BeginTransactionAsync();
        /// <summary>
        /// Mở kết nối
        /// </summary>
        void Open();
        /// <summary>
        /// Mở kết nối bất đồng bộ
        /// </summary>
        Task OpenAsync();
        /// <summary>
        /// Đóng kết nối
        /// </summary>
        void Close();
        /// <summary>
        /// Đóng kết nối bất đồng bộ
        /// </summary>
        Task CloseAsync();
        /// <summary>
        /// Xác nhận giao dịch
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        void Commit();
        /// <summary>
        /// Xác nhận giao dịch bất đồng bộ
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        Task CommitAsync();
        /// <summary>
        /// Hủy bỏ giao dịch
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        void Rollback();
        /// <summary>
        /// Hủy bỏ giao dịch bất đồng bộ
        /// </summary>
        /// Created by: nlnhat (18/07/2023)
        Task RollbackAsync();
    }
}
