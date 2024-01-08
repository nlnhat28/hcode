namespace HCode.Domain
{
    /// <summary>
    /// Đếm số lượng theo trạng thái theo dõi
    /// </summary>
    /// Created by: nlnhat (08/09/2023)
    public class CountByFollowModel
    {
        /// <summary>
        /// Số lượng đang theo dõi
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// Số lượng ngừng theo dõi
        /// </summary>
        public int UnfollowCount { get; set; }
        /// <summary>
        /// Tất cả
        /// </summary>
        public int TotalCount { get; set; }
    }
}
