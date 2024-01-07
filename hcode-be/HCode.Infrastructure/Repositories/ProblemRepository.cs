﻿using Dapper;
using HCode.Domain;
using System.Data;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository problem
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class ProblemRepository : BaseCodeRepository<Problem>, IProblemRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository problem
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        //public override async Task<IEnumerable<Problem>> GetDataFilterAsync(string proc, object? param)
        //{
        //    var data = new List<Problem>();
        //    var dictionary = new Dictionary<Guid, Problem>();
        //    var problems = await _unitOfWork.Connection.QueryAsync<Problem, Topic, Problem>(proc, (problem, topic) =>
        //    {
        //        if (!dictionary.TryGetValue(problem.ProblemId, out var currentProblem))
        //        {
        //            currentProblem = problem;
        //            dictionary.Add(currentProblem.ProblemId, currentProblem);
        //        }
        //        currentProblem.Topics ??= new List<Topic>();
        //        if (topic != null)
        //            currentProblem.Topics.Add(topic);

        //        return currentProblem;
        //    },
        //    param: param,
        //    transaction: _unitOfWork.Transaction,
        //    splitOn: "TopicId",
        //    commandType: CommandType.StoredProcedure);

        //    var result = dictionary.Values.ToList();
        //    return result;
        //}
        public override async Task<Problem?> GetAsync(Guid problemId) 
        {
            var proc = $"{Procedure}Get";

            var param = new DynamicParameters();
            param.Add($"p_{TableId}", problemId);

            using var multi = await _unitOfWork.Connection.QueryMultipleAsync(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);

            var problem = await multi.ReadFirstOrDefaultAsync<Problem>();

            if (problem != null)
            {
                problem.Parameters = (await multi.ReadAsync<Parameter>()).ToList();
                problem.Testcases = (await multi.ReadAsync<Testcase>()).ToList();
            }

            return problem;

        }

        //
        /// <summary>
        /// Lấy mã lớn nhất
        /// </summary>
        /// <param name="state">Công khai hay riêng tư</param>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public async Task<int> GetMaxCodeAsync(ProblemState state, Guid AccountId)
        {
            var proc = $"{Procedure}GetMaxCode";

            var param = new DynamicParameters();
            param.Add($"p_State", state);
            param.Add($"p_AccountId", AccountId);

            var result = await _unitOfWork.Connection.QueryFirstOrDefault(
                proc, param, transaction: _unitOfWork.Transaction, commandType: CommandType.StoredProcedure) ?? 0;

            return result;
        }
        #endregion
    }
}

