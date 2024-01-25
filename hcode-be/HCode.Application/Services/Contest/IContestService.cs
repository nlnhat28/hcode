

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
        Task JoinAsync(ContestAccountDto contestAccountDto, ServerResponse res);
        /// <summary>
        /// Rời 1 bài thi
        /// </summary>
        /// <param name="contestAccountDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task LeaveAsync(ContestAccountDto contestAccountDto, ServerResponse res);
    }
}
