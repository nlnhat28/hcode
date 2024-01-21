using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Service problem
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IProblemService : IBaseService<ProblemDto, Problem>
    {
        /// <summary>
        /// Map dto với entity
        /// </summary>
        /// <param name="problemDto"></param>
        /// <param name="isClone"></param>
        /// <returns></returns>
        (Problem problem, List<Parameter> parameters, List<Testcase> testcases) MapProblemDtoToEntity(
            ProblemDto problemDto, EditMode? editMode = EditMode.Create, bool? isClone = false);
        /// <summary>
        /// Submit problem
        /// </summary>
        /// <param name="problemDto"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        Task SubmitAsync(ProblemDto problemDto, ServerResponse res);
        /// <summary>
        /// Lấy danh sách bài toán cho bài thi
        /// </summary>
        /// <returns></returns>
        Task GetForContestAsync(ServerResponse res);
    }
}
