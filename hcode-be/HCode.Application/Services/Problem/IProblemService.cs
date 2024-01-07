using HCode.Domain;

namespace HCode.Application
{
    /// <summary>
    /// Service problem
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public interface IProblemService : IBaseService<ProblemDto, Problem>
    {
        (Problem problem, List<Parameter> parameters, List<Testcase> testcases) MapCreateProblemDtoToEntity(
            ProblemDto problemDto, bool? isClone = false);
    }
}
