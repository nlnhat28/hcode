
namespace HCode.Domain
{
    /// <summary>
    /// Interface master có detail
    /// </summary>
    /// <typeparam name="TDetail"></typeparam>
    public interface IMaster<TDetail>
    {
        /// <summary>
        /// Detail
        /// </summary>
        public TDetail Detail { get; set; }
        /// <summary>
        /// List detail
        /// </summary>
        public List<TDetail> Details { get; set; }
    }
}
