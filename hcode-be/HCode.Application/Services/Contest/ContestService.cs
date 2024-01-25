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
        /// Repo contest
        /// </summary>
        private new readonly IContestAccountRepository _contestAccountRepo;
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
                           IUnitOfWork unitOfWork, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _cache = cache;
            _ceService = ceService;
        }
        #endregion

        #region Methods
        // Validate
        public override async Task ValidateAsync(Contest contest, ServerResponse res)
        {
            var existed = await _repository.CheckExistedCodeAsync(contest.ContestCode, contest.ContestId);
            if (existed) 
            {
                res.OnError(ErrorCode.ContestExistedCode, new ErrorItem("refContestCode", _resource["ContestExistedCode"]))
            }
        }

        // Create
        public override async Task CreateAsync(ContestDto contestDto, ServerResponse res)
        {
            var (contest, contestProblems) = MapcontestDtoToEntity(contestDto);

            await ValidateAsync(contest, res);

            if (!res.Success)
            {
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

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
        public override async Task UpdateAsync(ContestDto contestDto, ServerResponse res)
        {
            var (contest, contestProblems) = MapcontestDtoToEntity(contestDto, EditMode.Update);

            await ValidateAsync(contest, res);

            if (!res.Success)
            {
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _repository.UpdateAsync(contest);
                await _contestProblemRepo.ReplaceManyAsync(contestProblems);

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
        public (Contest contest, List<Contestcontest> contestcontests) MapContestDtoToEntity(
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
                        contestProblem.ContestProblemId = param.ContestProblemId != Guid.Empty ? param.ContestProblemId : Guid.NewGuid();
                    }

                    contestProblem.contestId = contestId;
                });
                contestProblems = _mapper.Map<List<ContestProblem>>(contestProblemDtos);
            }

            return (contest, contestProblems);
        }

        
        // Join 1 bài thi
        public async Task JoinAsync(ContestAccountDto contestAccountDto, ServerResponse res) 
        {
            // Kiểm tra xem tồn tại hay không?

                // Nếu không thì thêm mới, trạng thái chưa thi
        }

        public async Task LeaveAsync(ContestAccountDto contestAccountDto, ServerResponse res) 
        {
            // Xóa đi thôi
        }
        #endregion
    }
}