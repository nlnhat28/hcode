using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service problem
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class ProblemService : BaseService<ProblemDto, Problem>, IProblemService
    {
        #region Fields
        /// <summary>
        /// Repo problem
        /// </summary>
        private new readonly IProblemRepository _repository;
        /// <summary>
        /// Repo parameter
        /// </summary>
        private readonly IParameterRepository _parameterRepo;
        /// <summary>
        /// Repo testcase
        /// </summary>
        private readonly ITestcaseRepository _testcaseRepo;
        /// <summary>
        /// Repo ProblemAccount
        /// </summary>
        private readonly IProblemAccountRepository _problemAccountRepo;
        /// <summary>
        /// Repo submission
        /// </summary>
        private readonly ISubmissionRepository _submissionRepo;
        /// <summary>
        /// Cache
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IMemoryCache _cache;
        /// <summary>
        /// Service thực thi code
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly ICEService _ceService;
        #endregion

        #region Constructors
        /// <summary>
        /// Hàm tạo service
        /// </summary>
        /// <param name="repository">Repository account</param>
        /// <param name="resource">Resource thông báo</param>
        /// <param name="mapper">Mapper map đối tượng</param>
        /// <param name="unitOfWork">Unit of work</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemService(IProblemRepository repository, IParameterRepository parameterRepo,
                           ITestcaseRepository testcaseRepo, IProblemAccountRepository problemAccountRepo,
                           ISubmissionRepository submissionRepo,
                           ICEService ceService, IStringLocalizer<Resource> resource, IMapper mapper,
                           IAuthService authService, IUnitOfWork unitOfWork, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _testcaseRepo = testcaseRepo;
            _parameterRepo = parameterRepo;
            _problemAccountRepo = problemAccountRepo;
            _submissionRepo = submissionRepo;
            _cache = cache;
            _ceService = ceService;
        }
        #endregion

        #region Methods
        // Get problem by id
        public override async Task<ProblemDto> GetAsync(Guid id)
        {
            var accountId = _authService.GetAccountId();
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ProblemDto>(entity);

            result.Testcases = _mapper.Map<List<TestcaseDto>>(entity?.Testcases);
            result.Parameters = _mapper.Map<List<ParameterDto>>(entity?.Parameters);

            return result;
        }


        // Tạo problem mới
        public override async Task CreateAsync(ProblemDto problemDto, ServerResponse res)
        {
            var (problem, parameters, testcases) = MapProblemDtoToEntity(problemDto);

            await ValidateAsync(problem, res);

            if (!res.Success)
            {
                return;
            }

            // Lưu thật
            if (problemDto.IsDraft == false)
            {
                await _ceService.ExecuteAsync(problemDto, testcases, res);
            }

            // Nếu thành công thì lưu vào db
            if (res.Success)
            {
                // Nếu mà lưu ở 2 chế độ công khai và riêng tư
                if (problemDto.IsPrivateState == true && problemDto.IsPublicState == true)
                {
                    problem.State = ProblemState.Private;

                    var (problemPublic, paramPublic, testcasePublic) = MapProblemDtoToEntity(problemDto, isClone: true);
                    problemPublic.State = ProblemState.Public;

                    try
                    {
                        await _unitOfWork.BeginTransactionAsync();

                        var newCode = await _repository.GetMaxCodeAsync(ProblemState.Public, _authService.GetAccountId());
                        newCode++;
                        problemPublic.ProblemCode = newCode;
                        await _repository.InsertAsync(problemPublic);
                        await _parameterRepo.InsertManyAsync(paramPublic);
                        await _testcaseRepo.InsertManyAsync(testcasePublic);

                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception exception)
                    {
                        res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], exception);
                    }
                    finally
                    {
                        await _unitOfWork.RollbackAsync();
                    }
                }

                if (!res.Success)
                {
                    return;
                }

                // Lưu đúng chế độ
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    var newCode = await _repository.GetMaxCodeAsync(problemDto.State ?? ProblemState.Private, _authService.GetAccountId());
                    newCode++;
                    problem.ProblemCode = newCode;
                    await _repository.InsertAsync(problem);
                    await _parameterRepo.InsertManyAsync(parameters);
                    await _testcaseRepo.InsertManyAsync(testcases);

                    await _unitOfWork.CommitAsync();
                }
                catch (Exception exception)
                {
                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], exception);
                }
                finally
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }

        // Cập nhật problem
        public override async Task UpdateAsync(Guid problemId, ProblemDto problemDto, ServerResponse res)
        {
            var (problem, parameters, testcases) = MapProblemDtoToEntity(problemDto, EditMode.Update);

            await ValidateAsync(problem, res);

            if (!res.Success)
            {
                return;
            }

            // Lưu thật
            if (problemDto.IsDraft == false)
            {
                await _ceService.ExecuteAsync(problemDto, testcases, res);
            }

            // Nếu thành công thì lưu vào db
            if (res.Success)
            {
                // Lưu đúng chế độ
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    await _repository.UpdateAsync(problem);
                    await _parameterRepo.ReplaceManyAsync(parameters, problemId, "ProblemId");
                    await _testcaseRepo.ReplaceManyAsync(testcases, problemId, "ProblemId");

                    await _unitOfWork.CommitAsync();
                }
                catch (Exception exception)
                {
                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], exception);
                }
                finally
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }

        // Submit
        public async Task SubmitAsync(ProblemDto problemDto, ServerResponse res)
        {
            var (_, _, testcases) = MapProblemDtoToEntity(problemDto, EditMode.Update);
            await _ceService.ExecuteAsync(problemDto, testcases, res);

            try
            {
                if (res.Data is SubmissionData data)
                {
                    var submission = AppHelper.InitSubmission(data, problemDto);
                    var subRes = await _submissionRepo.InsertAsync(submission);
                    res.AddData("Successfully insert submission");
                }
            }
            catch (Exception exception)
            {
                res.AddData(exception);
            }
        }

        // Map
        public (Problem problem, List<Parameter> parameters, List<Testcase> testcases) MapProblemDtoToEntity(
            ProblemDto problemDto, EditMode? editMode = EditMode.Create, bool? isClone = false)
        {
            var clone = isClone ?? false;

            var problem = new Problem();

            switch (editMode)
            {
                case EditMode.Create:
                    problem = MapCreateDtoToEntity(problemDto);
                    problem.AccountId = _authService.GetAccountId();
                    break;
                case EditMode.Update:
                    problem = MapUpdateDtoToEntity(problemDto);
                    break;
                default:
                    break;
            }

            var problemId = problem.ProblemId;

            var parameters = new List<Parameter>();
            var testcases = new List<Testcase>();

            // Parameters
            var parametersDto = problemDto.Parameters;

            if (parametersDto != null && parametersDto.Count > 0)
            {
                parametersDto.ForEach(param =>
                {
                    if (clone)
                    {
                        param.ParameterId = Guid.NewGuid();
                    }
                    else
                    {
                        param.ParameterId = param.ParameterId != Guid.Empty ? param.ParameterId : Guid.NewGuid();
                    }

                    param.ProblemId = problemId;
                });
                parameters = _mapper.Map<List<Parameter>>(parametersDto);
            }

            // Testcases
            var testcasesDto = problemDto.Testcases;
            if (testcasesDto != null && testcasesDto.Count > 0)
            {
                testcasesDto.ForEach(test =>
                {
                    if (clone)
                    {
                        test.TestcaseId = Guid.NewGuid();
                    }
                    else
                    {
                        test.TestcaseId = test.TestcaseId != Guid.Empty ? test.TestcaseId : Guid.NewGuid();
                    }

                    test.ProblemId = problemId;
                });
                testcases = _mapper.Map<List<Testcase>>(testcasesDto);
            }

            return (problem, parameters, testcases);
        }

        // Lấy danh sách bài toán cho bài thi
        public async Task GetForContestAsync(ServerResponse res)
        {
            var accountId = _authService.GetAccountId();

            var problems = await _repository.GetForContestAsync(accountId) ?? new List<Problem>();

            var result = _mapper.Map<IEnumerable<ProblemDto>>(problems) ?? new List<ProblemDto>();

            res.Data = result;
        }

        /// <summary>
        /// Tạo quan hệ bài toán tài khoản
        /// </summary>
        /// <returns></returns>
        public async Task AuditProblemAccountAsync(ProblemAccount problemAccount, ServerResponse res)
        {
            if (problemAccount.ProblemAccountId == Guid.Empty)
            {
                problemAccount.ProblemAccountId = Guid.NewGuid();
            }

            problemAccount.AccountId = _authService.GetAccountId();

            var result = await _problemAccountRepo.AuditProblemAccountAsync(problemAccount);
            res.Data = result;
        }
        #endregion
    }
}