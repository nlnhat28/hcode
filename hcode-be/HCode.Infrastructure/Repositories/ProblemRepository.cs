using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository auth
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ProblemRepository : BaseCodeRepository<Problem>, IProblemRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository tài khoản
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        //
        public override async Task<IEnumerable<Problem>> GetDataFilterAsync(string proc, object? param)
        {
            var data = new List<Problem>();
            var dictionary = new Dictionary<Guid, Problem>();
            var problems = await _unitOfWork.Connection.QueryAsync<Problem, Topic, Problem>(proc, (problem, topic) =>
            {
                if (!dictionary.TryGetValue(problem.ProblemId, out var currentProblem))
                {
                    currentProblem = problem;
                    dictionary.Add(currentProblem.ProblemId, currentProblem);
                }
                currentProblem.Topics ??= new List<Topic>();
                if (topic != null)
                    currentProblem.Topics.Add(topic);

                return currentProblem;
            },
            param: param,
            transaction: _unitOfWork.Transaction,
            splitOn: "TopicId",
            commandType: CommandType.StoredProcedure);

            var result = dictionary.Values.ToList();
            return result;
        }

        #endregion
    }
}

