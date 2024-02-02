using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace HCode.Application
{
    /// <summary>
    /// Triển khai service contest
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class ContestService : BaseService<ContestDto, Contest>, IContestService
    {
        #region Fields
        /// <summary>
        /// Repo contest
        /// </summary>
        private new readonly IContestRepository _repository;
        /// <summary>
        /// Repo contest account
        /// </summary>
        private readonly IContestAccountRepository _contestAccountRepo;
        /// <summary>
        /// Repo contest problem
        /// </summary>
        private readonly IContestProblemRepository _contestProblemRepo;
        /// <summary>
        /// Repo submission
        /// </summary>
        private readonly ISubmissionRepository _submissionRepo;
        /// <summary>
        /// Repo contest problem account
        /// </summary>
        private readonly IContestProblemAccountRepository _cpaRepo;
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
        /// <summary>
        /// Service problem
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IProblemService _problemService;
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
        public ContestService(IContestRepository repository, ICEService ceService,
                           IStringLocalizer<Resource> resource, IMapper mapper, IAuthService authService,
                           IUnitOfWork unitOfWork, IMemoryCache cache, IContestProblemRepository contestProblemRepo,
                           IContestAccountRepository contestAccountRepo, IProblemService problemService,
                           IContestProblemAccountRepository cpaRepo, ISubmissionRepository submissionRepo)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _cache = cache;
            _ceService = ceService;
            _problemService = problemService;
            _contestProblemRepo = contestProblemRepo;
            _contestAccountRepo = contestAccountRepo;
            _submissionRepo = submissionRepo;
            _cpaRepo = cpaRepo;
        }
        #endregion

        #region Methods
        // View Contest by id
        public async Task GetForSubmitAsync(Guid id, ServerResponse res)
        {
            var accountId = _authService.GetAccountId();
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ContestDto>(entity);

            if (result != null)
            {
                result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);
                result.ContestAccount = _mapper.Map<ContestAccountDto>(entity?.ContestAccount);
                result.Password = string.Empty;
            }

            res.Data = result;
        }

        // View contest by id
        public override async Task ViewAsync(Guid id, ServerResponse res)
        {
            var accountId = _authService.GetAccountId();
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ContestDto>(entity);

            if (result != null)
            {
                result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);
                result.ContestAccount = _mapper.Map<ContestAccountDto>(entity?.ContestAccount);
                result.Password = string.Empty;
                result.ContestProblems?.Skip(1).ToList().ForEach(i => i.ProblemId = Guid.Empty);
            }

            res.Data = result;
        }

        // View contest result by id
        public async Task ViewResultAsync(Guid id, Guid accountId, ServerResponse res)
        {
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ContestDto>(entity);

            if (result != null)
            {
                result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);
                result.ContestAccount = _mapper.Map<ContestAccountDto>(entity?.ContestAccount);
                result.Password = string.Empty;
            }

            res.Data = result;
        }

        // Get by id
        public override async Task GetAsync(Guid id, ServerResponse res)
        {
            var accountId = _authService.GetAccountId();
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ContestDto>(entity);

            if (result != null)
            {
                result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);
            }

            res.Data = result;
        }

        // Get by id for submit
        //public override async Task<ContestDto> GetForSubmitAsync(Guid id)
        //{
        //    var accountId = _authService.GetAccountId();
        //    var entity = await _repository.GetAsync(id, accountId);

        //    var result = _mapper.Map<ContestDto>(entity);

        //    result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);

        //    return result;
        //}

        // Validate
        public override async Task ValidateAsync(Contest contest, ServerResponse res)
        {
            var existed = await _repository.CheckExistedCodeAsync(contest.ContestCode, contest.ContestId);
            if (existed) 
            {
                res.OnError(ErrorCode.ContestExistedCode, new ErrorItem("refContestCode", 
                    string.Format(_resource["ContestExistedCode"], contest.ContestCode)));
            }
        }

        // Create
        public override async Task CreateAsync(ContestDto contestDto, ServerResponse res)
        {
            var (contest, contestProblems) = MapContestDtoToEntity(contestDto);

            await ValidateAsync(contest, res);

            if (!res.Success)
            {
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (string.IsNullOrWhiteSpace(contest.ContestCode))
                {
                    var code = AppHelper.GenerateRandomCode();
                    var length = 6;

                    while (await _repository.CheckExistedCodeAsync(code, contest.ContestId))
                    {
                        code = AppHelper.GenerateRandomCode(length);
                        length++;
                    }

                    contest.ContestCode = code;

                    res.Data = new Contest()
                    {
                        ContestCode = code,
                    };
                }

                await _repository.InsertAsync(contest);
                await _contestProblemRepo.InsertManyAsync(contestProblems);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception exception)
            {
                res.OnError(ErrorCode.ContestCreate, _resource["ContestCreateError"], exception);
            }
            finally
            {
                await _unitOfWork.RollbackAsync();
            }
        }

        // Update
        public override async Task UpdateAsync(Guid contestId, ContestDto contestDto, ServerResponse res)
        {
            var (contest, contestProblems) = MapContestDtoToEntity(contestDto, EditMode.Update);

            await ValidateAsync(contest, res);

            if (!res.Success)
            {
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (string.IsNullOrWhiteSpace(contest.ContestCode))
                {
                    var code = AppHelper.GenerateRandomCode();
                    var length = 6;

                    while (await _repository.CheckExistedCodeAsync(code, contest.ContestId))
                    {
                        code = AppHelper.GenerateRandomCode(length);
                        length++;
                    }

                    contest.ContestCode = code;

                    res.Data = new Contest()
                    {
                        ContestCode = code,
                    };
                }

                await _repository.UpdateAsync(contest);
                await _contestProblemRepo.ReplaceManyAsync(contestProblems, contest.ContestId, "ContestId");

                await _unitOfWork.CommitAsync();
            }
            catch (Exception exception)
            {
                res.OnError(ErrorCode.ContestUpdate, _resource["ContestUpdateError"], exception);
            }
            finally
            {
                await _unitOfWork.RollbackAsync();
            }
        }

         // Map
        public (Contest contest, List<ContestProblem> contestProblems) MapContestDtoToEntity(
            ContestDto contestDto, EditMode? editMode = EditMode.Create, bool? isClone = false)
        {
            var clone = isClone ?? false;

            var contest = new Contest();

            switch (editMode)
            {
                case EditMode.Create:
                    contest = MapCreateDtoToEntity(contestDto);
                    contest.AccountId = _authService.GetAccountId();
                    break;
                case EditMode.Update:
                    contest = MapUpdateDtoToEntity(contestDto);
                    break;
                default:
                    break;
            }

            var contestId = contest.ContestId;

            var contestProblems = new List<ContestProblem>();

            // contestProblemDtos
            var contestProblemDtos = contestDto.ContestProblems;

            if (contestProblemDtos != null && contestProblemDtos.Count > 0)
            {
                contestProblemDtos.ForEach(contestProblem =>
                {
                    if (clone)
                    {
                        contestProblem.ContestProblemId = Guid.NewGuid();
                    }
                    else
                    {
                        contestProblem.ContestProblemId = contestProblem.ContestProblemId != Guid.Empty ? 
                            contestProblem.ContestProblemId : Guid.NewGuid();
                    }

                    contestProblem.ContestId = contestId;
                });
                contestProblems = _mapper.Map<List<ContestProblem>>(contestProblemDtos);
            }

            return (contest, contestProblems);
        }

        
        // Join 1 bài thi
        public async Task JoinAsync(ContestDto contestDto, ServerResponse res) 
        {
            var contest = await _repository.GetAsync(contestDto.ContestId);
            // Kiểm tra xem có password không
            if (contest != null && contest.HasPassword) {
                if (contestDto.Password != contest.Password) {
                    res.OnError(ErrorCode.ContestWrongPassword, new ErrorItem("refPassword", _resource["ContestWrongPassword"]));
                    return;
                }
            }

            // Khởi tạo quan hệ contest_account
            var contestAccount = new ContestAccount(contestDto.ContestId, _authService.GetAccountId());
            await _contestAccountRepo.InsertAsync(contestAccount);
        }

        // Rời bài thi
        public async Task LeaveAsync(Guid contestAccountId, ServerResponse res) 
        {
            await _contestAccountRepo.DeleteAsync(contestAccountId);
        }

        // Bắt đầu bài thi
        public async Task StartAsync(Guid contestAccountId, ServerResponse res) 
        {
            var contestAccount = await _contestAccountRepo.GetAsync(contestAccountId);

            if (contestAccount != null)
            {
                contestAccount.OnStart();
                await _contestAccountRepo.UpdateAsync(contestAccount);
            }
        }

        // Tiếp tục bài thi
        public async Task ContinueAsync(Guid contestAccountId, ServerResponse res)
        {
            var contestAccount = await _contestAccountRepo.GetAsync(contestAccountId);

            if (contestAccount != null && contestAccount.State == ContestAccountState.Finish)
            {
                var contest = await _repository.GetAsync(contestAccount.ContestId);
                contestAccount.OnFinish(contest?.TimeToDo);
                await _contestAccountRepo.UpdateAsync(contestAccount);
                res.OnError(ErrorCode.ContestAccountFinish, _resource["ContestAccountFinish"]);
            }
        }

        // Kết thúc bài thi
        public async Task FinishAsync(Guid contestAccountId, ServerResponse res) 
        {
            var contestAccount = await _contestAccountRepo.GetAsync(contestAccountId);
            if (contestAccount != null)
            {
                contestAccount.OnFinish();
                await _contestAccountRepo.UpdateAsync(contestAccount);
            }
        }

        // Submit 1 câu hỏi
        public async Task SubmitAsync(Guid contestProblemId, ProblemDto problemDto, ServerResponse res) 
        {
            var (_, parameters, testcases) = _problemService.MapProblemDtoToEntity(problemDto, EditMode.Update);
            await _ceService.ExecuteAsync(problemDto, testcases, res);

            // Lưu dư thừa
            if (res.Data is SubmissionData data)
            {
                // Thêm mới quan hệ contest_problem_account và submission
                try
                {
                    var state = ProblemAccountState.Seen;

                    switch (data.StatusId)
                    {
                        case StatusJudge0.Accepted:
                            state = ProblemAccountState.Accepted;
                            break;
                        case StatusJudge0.InQueue:
                        case StatusJudge0.Processing:
                            break;
                        default:
                            state = ProblemAccountState.Wrong;
                            break;
                    };

                    // contest_problem_account
                    var accountId = _authService.GetAccountId();
                    var existed = await _cpaRepo.GetByConstraintAsync(contestProblemId, accountId);
                    var cpaId = existed == null ? Guid.NewGuid() : existed.ContestProblemAccountId;

                    if (existed == null)
                    {
                        // Thêm mới
                        var cpa = new ContestProblemAccount()
                        {
                            ContestProblemAccountId = cpaId,
                            ContestProblemId = contestProblemId,
                            AccountId = accountId,
                            State = state
                        };
                        var cpaRes = await _cpaRepo.InsertAsync(cpa);
                        res.AddData(new BaseResponse()
                        {
                            SuccessCode = SuccessCode.ContestProblemAccountSaved,
                            Data = cpa,
                        });
                    }
                    else if (existed.State != ProblemAccountState.Accepted && existed.State != state)
                    {
                        // Cập nhật
                        var cpa = new ContestProblemAccount()
                        {
                            ContestProblemAccountId = existed.ContestProblemAccountId,
                            ContestProblemId = contestProblemId,
                            AccountId = accountId,
                            State = state
                        };
                        var cpaRes = await _cpaRepo.UpdateAsync(cpa);
                        res.AddData(new BaseResponse()
                        {
                            SuccessCode = SuccessCode.ContestProblemAccountSaved,
                            Data = cpa,
                        });
                    }

                    // submission
                    var testcaseDtos = _mapper.Map<List<TestcaseDto>>(testcases);
                    testcaseDtos?.ForEach(tc => tc.Status = data.Submissions.FirstOrDefault(s => s.testcase_id == tc.TestcaseId));;

                    var parameterJson = JsonSerializer.Serialize(parameters);
                    var testcaseJson = JsonSerializer.Serialize(testcaseDtos);

                    var submission = data.InitSubmission(
                        problemDto.Solution, problemDto.SolutionLanguage?.LanguageId, contestProblemAccountId: cpaId);

                    submission.Parameters = parameterJson;
                    submission.Testcases = testcaseJson;
                    
                    var subRes = await _submissionRepo.InsertAsync(submission);
                    res.AddData(new BaseResponse(SuccessCode.SubmissionSaved));
                }
                catch (Exception ex)
                {
                    res.AddData(ex);
                };
            }
        }
        #endregion
    }
}