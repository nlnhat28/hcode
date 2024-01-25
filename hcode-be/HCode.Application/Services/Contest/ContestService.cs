using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
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
        private new readonly IContestAccountRepository _contestAccountRepo;
        /// <summary>
        /// Repo contest problem
        /// </summary>
        private new readonly IContestProblemRepository _contestProblemRepo;
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
        public ContestService(IContestRepository repository, ICEService ceService,
                           IStringLocalizer<Resource> resource, IMapper mapper, IAuthService authService,
                           IUnitOfWork unitOfWork, IMemoryCache cache, IContestProblemRepository contestProblemRepo,
                           IContestAccountRepository contestAccountRepo)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _cache = cache;
            _ceService = ceService;
            _contestProblemRepo = contestProblemRepo;
            _contestAccountRepo = contestAccountRepo;
        }
        #endregion

        #region Methods
        // Get by id
        public override async Task<ContestDto> GetAsync(Guid id)
        {
            var accountId = _authService.GetAccountId();
            var entity = await _repository.GetAsync(id, accountId);

            var result = _mapper.Map<ContestDto>(entity);

            result.ContestProblems = _mapper.Map<List<ContestProblemDto>>(entity?.ContestProblems);

            return result;
        }

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
        public async Task JoinAsync(ContestAccountDto contestAccountDto, ServerResponse res) 
        {
            var contestAccount = _mapper.Map<ContestAccount>(contestAccountDto);
            contestAccount.ContestAccountId = Guid.NewGuid();
            contestAccount.AccountId = _authService.GetAccountId();
            contestAccount.State = ContestAccountState.Pending;
            await _contestAccountRepo.InsertAsync(contestAccount);
        }

        // Rời bài thi
        public async Task LeaveAsync(ContestAccountDto contestAccountDto, ServerResponse res) 
        {
            await _contestAccountRepo.DeleteAsync(contestAccountDto.ContestAccountId);
        }
        #endregion
    }
}