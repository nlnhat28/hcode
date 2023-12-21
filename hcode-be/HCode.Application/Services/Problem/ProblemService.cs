using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
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
        private new readonly IParameterRepository _parameterRepo;
        /// <summary>
        /// Repo testcase
        /// </summary>
        private new readonly ITestcaseRepository _testcaseRepo;
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
                           ITestcaseRepository testcaseRepo, ICEService ceService,
                           IStringLocalizer<Resource> resource, IMapper mapper, IAuthService authService,
                           IUnitOfWork unitOfWork, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork, authService)
        {
            _repository = repository;
            _testcaseRepo = testcaseRepo;
            _parameterRepo = parameterRepo;
            _cache = cache;
            _ceService = ceService;
        }
        #endregion

        #region Methods
        // Get problem by id
        public override async Task<ProblemDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);

            var result = _mapper.Map<ProblemDto>(entity);

            result.Testcases = _mapper.Map<List<TestcaseDto>>(entity?.Testcases);
            result.Parameters = _mapper.Map<List<ParameterDto>>(entity?.Parameters);

            return result;
        }

        // Tạo problem mới
        public override async Task CreateAsync(ProblemDto problemDto, ServerResponse res)
        {
            // Lưu nháp
            if (problemDto.State == ProblemState.Draft)
            {
                var problem = MapCreateDtoToEntity(problemDto);

                await ValidateAsync(problem, res);

                if (res.Success)
                {
                    res.Data = await _repository.InsertAsync(problem);
                }
            }
            // Lưu thật
            else
            {
                var parames = BuildSubmissionParams(problemDto, problemDto.SolutionLanguage?.JudgeId);

                var resCreateBatch = await _ceService.CreateBatchAsync(parames);

                if (resCreateBatch.Success)
                {
                    var submissionResponses = resCreateBatch.Data as List<SubmissionResponse>;
                    var tokens = submissionResponses?.Select(r => r.token).ToList() ?? new List<string>();

                    var resGetBatch = await _ceService.GetBatchAsync(tokens);

                    var data = resGetBatch.Data as List<SubmissionResponse>;
                    res.OnSuccess(data);

                    var problem = MapCreateDtoToEntity(problemDto);

                    await ValidateAsync(problem, res);

                    if (res.Success)
                    {
                        await _repository.InsertAsync(problem);
                    }
                }

            }
        }
        private List<SubmissionParam> BuildSubmissionParams(ProblemDto problemDto, int? judgeId)
        {
            var parames = new List<SubmissionParam>();

            var testcases = problemDto.Testcases;
            var parameters = problemDto.Parameters;

            var limitTime = problemDto.LimitTime;
            var limitMemory = problemDto.LimitMemory;

            var sourceCode = problemDto.Solution;
            var languageId = judgeId;

            if (testcases != null && parameters != null)
            {
                for (int i = 0; i < testcases.Count; i++)
                {
                    var stdin = JsonSerializer.Serialize(testcases[i].Inputs);
                    var expectedOutput = testcases[i].ExpectedOutput?.ToString();

                    var submissison = new SubmissionParam()
                    {
                        source_code = sourceCode,
                        language_id = (int)(languageId ?? 0),
                        stdin = stdin,
                        expected_output = expectedOutput,
                        cpu_time_limit = limitTime,
                        memory_limit = limitMemory,
                    };

                    parames.Add(submissison);
                }

            }
            return parames;
        }
        #endregion
    }
}