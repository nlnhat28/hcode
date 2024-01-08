namespace HCode.Domain
{
    /// <summary>
    /// Enum các kiểu so sánh
    /// </summary>
    /// Created by: nlnhat (29/08/2023)
    public enum CompareType
    {
        /// <summary>
        /// Chứa
        /// </summary>
        Contain = 1,
        /// <summary>
        /// Không chứa
        /// </summary>
        NotContain = 2,
        /// <summary>
        /// Bằng
        /// </summary>
        Equal = 3,
        /// <summary>
        /// Nhỏ hơn
        /// </summary>
        Less = 4,
        /// <summary>
        /// Lớn hơn
        /// </summary>
        More = 5,
        /// <summary>
        /// Trong khoảng
        /// </summary>
        Range = 6,
        /// <summary>
        /// Rỗng
        /// </summary>
        Empty = 7,
        /// <summary>
        /// Bắt đầu với
        /// </summary>
        StartWith = 8,
        /// <summary>
        /// Kết thúc với
        /// </summary>
        EndWidth = 9,
        /// <summary>
        /// Nhỏ hơn hoặc bằng
        /// </summary>
        LessEqual = 10,
        /// <summary>
        /// Lớn hơn hoặc bằng
        /// </summary>
        MoreEqual = 11,
        /// <summary>
        /// Không bằng
        /// </summary>
        NotEqual = 12,
    }
}
