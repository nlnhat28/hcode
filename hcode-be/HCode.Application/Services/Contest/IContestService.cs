

using HCode.Domain;

namespace HCode.Application
{
    public interface IContestService : IBaseService<ContestDto, Contest>
    {
        /// <summary>
        /// Map contestDto sang Entity
        /// </summary>
        /// <param name="problemDto"></param>
        /// <param name="editMode"></param>
        /// <param name="isClone"></param>
        /// <returns></returns>
        (Contest contest, List<ContestProblem> contestProblems) MapContestDtoToEntity(
             ContestDto problemDto, EditMode? editMode = EditMode.Create, bool? isClone = false);
        /// <summary>
        /// Join 1 bài thi
        /// </summary>
        /// <param name="contestAccountDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task JoinAsync(ContestDto contestDto, ServerResponse res);
        /// <summary>
        /// Bắt đầu 1 bài thi
        /// </summary>
        /// <param name="contestAccountId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task StartAsync(Guid contestAccountId, ServerResponse res);
        /// <summary>
        /// Tiếp tục 1 bài thi
        /// </summary>
        /// <param name="contestAccountId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task ContinueAsync(Guid contestAccountId, ServerResponse res);
        /// <summary>
        /// Kết thúc 1 bài thi
        /// </summary>
        /// <param name="contestAccountId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task FinishAsync(Guid contestAccountId, ServerResponse res);
        /// <summary>
        /// Rời 1 bài thi
        /// </summary>
        /// <param name="contestAccountDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task LeaveAsync(Guid contestAccountId, ServerResponse res);
        /// <summary>
        /// Lấy contest cho màn submit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task GetForSubmitAsync(Guid id, ServerResponse res);
        /// <summary>
        /// Lấy contest cho màn kết quả
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task ViewResultAsync(Guid id, Guid accountId, ServerResponse res);
        /// <summary>
        /// Submit bài toán trong bài thi
        /// </summary>
        /// <param name="contestProblemId"></param>
        /// <param name="problemDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task SubmitAsync(Guid contestProblemId, ProblemDto problemDto, ServerResponse res);
    }
}
