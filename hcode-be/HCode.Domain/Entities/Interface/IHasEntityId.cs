
namespace HCode.Domain
{
    /// <summary>
    /// Entity có id
    /// </summary>
    public interface IHasEntityId
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
