namespace HCode.Domain
{
    /// <summary>
    /// Chủ đề bài toán
    /// </summary>
    public class Topic
    {
        #region Properties
        /// <summary>
        /// Id topic
        /// </summary>
        public Guid TopicId { get; set; }
        /// <summary>
        /// Tên chủ đề
        /// </summary>
        public string TopicName { get; set; }
        /// <summary>
        /// Mô tả chủ đề
        /// </summary>
        public string? Description { get; set; } 
        #endregion
    }
}
