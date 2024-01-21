using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Data;
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
            var (problem, parameters, testcases) = MapCreateProblemDtoToEntity(problemDto);

            await ValidateAsync(problem, res);

            if (!res.Success)
            {
                return;
            }

            // Lưu thật
            if (problemDto.IsDraft == false)
            {
                var parames = BuildSubmissionParams(problemDto, problemDto.SolutionLanguage?.JudgeId);

                var resCreateBatch = await _ceService.CreateBatchAsync(parames);

                if (resCreateBatch.Success)
                {
                    var submissionResponses = resCreateBatch.Data as List<SubmissionResponse>;
                    var tokens = submissionResponses?.Select(r => r.token).ToList() ?? new List<string>();

                    var allResponses = new List<SubmissionResponse>();
                    var processingTokens = new List<string>(tokens);
                    var count = 0;
                    var limitCount = 3;
                    var resError = new object();

                    while (processingTokens.Count > 0 && count < limitCount)
                    {
                        count++;

                        await Task.Delay(2000);

                        var _res = await _ceService.GetBatchAsync(processingTokens);

                        processingTokens.Clear();

                        if (_res.Data is List<SubmissionResponse> _data && _data.Count > 0)
                        {
                            // Lần cuối rồi thì Add hết
                            if (count >= limitCount)
                            {
                                allResponses.AddRange(_data);
                            }
                            else
                            {
                                foreach (var item in _data)
                                {
                                    if (item.status_id == StatusJudge0.InQueue && item.status_id == StatusJudge0.Processing)
                                    {
                                        processingTokens.Add(item.token);
                                    }
                                    else
                                    {
                                        allResponses.Add(item);
                                    }
                                }
                            }
                        }
                        else
                        {
                            resError = _res;
                        }
                    }

                    if (allResponses.Count > 0)
                    {
                        var submissionData = new SubmissionData()
                        {
                            Submissions = allResponses
                        };

                        foreach (var subRes in allResponses)
                        {
                            var index = allResponses.IndexOf(subRes);

                            if (index >= 0 && index < testcases.Count && testcases[index] != null)
                            {
                                subRes.testcase_id = testcases[index].TestcaseId;
                            }

                            switch (subRes.status_id)
                            {
                                case StatusJudge0.Accepted:
                                    break;
                                case StatusJudge0.Error:
                                case StatusJudge0.InQueue:
                                case StatusJudge0.Processing:
                                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemTryAgain"], submissionData);
                                    break;
                                case StatusJudge0.WrongAnswer:
                                    // Lỗi quá tài nguyên
                                    var time = Convert.ToDecimal(subRes.time);
                                    var memory = subRes.memory;
                                    var errors = new List<string>();

                                    if (problem.LimitTime != null && problem.LimitTime > 0 && time > problem.LimitTime)
                                    {
                                        var msg = string.Format(_resource["ProblemOverLimitTime"], time + "s");
                                        errors.Add(msg);
                                    }
                                    if (problem.LimitMemory != null && problem.LimitMemory > 0 && memory > problem.LimitMemory)
                                    {
                                        var msg = string.Format(_resource["ProblemOverLimitMemory"], memory + "kb");
                                        errors.Add(msg);
                                    }

                                    if (errors?.Count > 0)
                                    {
                                        var userMsg = string.Join(" ", errors);
                                        subRes.status_id = StatusJudge0.OverLimit;
                                        subRes.user_msg = userMsg;
                                    }

                                    res.OnError(ErrorCode.ProblemTestcaseCreate, _resource["ProblemTestcaseCreateError"], submissionData);
                                    break;
                                case StatusJudge0.TimeLimitExceeded:
                                    res.OnError(ErrorCode.ProblemTimeLimitExceeded, _resource["ProblemTimeLimitExceeded"], submissionData);
                                    break;
                                default:
                                    res.OnError(ErrorCode.ProblemSourceCode, _resource["ProblemSourceCodeError"], submissionData);
                                    break;
                            }
                        };

                        if (res.Success)
                        {
                            submissionData.CalculateAverage();
                            res.Data = submissionData;
                        }
                    }
                    else
                    {
                        res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], resError);
                    }
                }
                else
                {
                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], resCreateBatch);
                }
            }

            // Nếu thành công thì lưu vào db
            if (res.Success)
            {
                // Nếu mà lưu ở 2 chế độ công khai và riêng tư
                if (problemDto.IsPrivateState == true && problemDto.IsPublicState == true)
                {
                    problem.State = ProblemState.Private;

                    var (problemPublic, paramPublic, testcasePublic) = MapCreateProblemDtoToEntity(problemDto);
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

                    var newCode = await _repository.GetMaxCodeAsync(ProblemState.Public, _authService.GetAccountId());
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
            var (problem, parameters, testcases) = MapCreateProblemDtoToEntity(problemDto);

            await ValidateAsync(problem, res);

            if (!res.Success)
            {
                return;
            }

            // Lưu thật
            if (problemDto.State == ProblemState.Public || problemDto.State == ProblemState.Private)
            {
                var parames = BuildSubmissionParams(problemDto, problemDto.SolutionLanguage?.JudgeId);

                var resCreateBatch = await _ceService.CreateBatchAsync(parames);

                if (resCreateBatch.Success)
                {
                    var submissionResponses = resCreateBatch.Data as List<SubmissionResponse>;
                    var tokens = submissionResponses?.Select(r => r.token).ToList() ?? new List<string>();

                    var resGetBatch = await _ceService.GetBatchAsync(tokens);

                    res.Data = resGetBatch;

                    if (resGetBatch.Data is List<SubmissionResponse> data && data.Count > 0)
                    {
                        var submissionData = new SubmissionData()
                        {
                            Submissions = data
                        };

                        foreach (var subRes in data)
                        {
                            var index = data.IndexOf(subRes);

                            if (index >= 0 && index < testcases.Count && testcases[index] != null)
                            {
                                subRes.testcase_id = testcases[index].TestcaseId;
                            }

                            switch (subRes.status_id)
                            {
                                case StatusJudge0.Accepted:
                                    break;
                                case StatusJudge0.Error:
                                case StatusJudge0.InQueue:
                                case StatusJudge0.Processing:
                                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemTryAgain"], submissionData);
                                    break;
                                case StatusJudge0.WrongAnswer:
                                    // Lỗi quá tài nguyên
                                    var time = Convert.ToDecimal(subRes.time);
                                    var memory = subRes.memory;
                                    var errors = new List<string>();

                                    if (problem.LimitTime != null && problem.LimitTime > 0 && time > problem.LimitTime)
                                    {
                                        var msg = string.Format(_resource["ProblemOverLimitTime"], time + "s");
                                        errors.Add(msg);
                                    }
                                    if (problem.LimitMemory != null && problem.LimitMemory > 0 && memory > problem.LimitMemory)
                                    {
                                        var msg = string.Format(_resource["ProblemOverLimitMemory"], memory + "kb");
                                        errors.Add(msg);
                                    }

                                    if (errors?.Count > 0)
                                    {
                                        var userMsg = string.Join(" ", errors);
                                        subRes.status_id = StatusJudge0.OverLimit;
                                        subRes.user_msg = userMsg;
                                    }

                                    res.OnError(ErrorCode.ProblemTestcaseCreate, _resource["ProblemTestcaseCreateError"], submissionData);
                                    break;
                                case StatusJudge0.TimeLimitExceeded:
                                    res.OnError(ErrorCode.ProblemTimeLimitExceeded, _resource["ProblemTimeLimitExceeded"], submissionData);
                                    break;
                                default:
                                    res.OnError(ErrorCode.ProblemSourceCode, _resource["ProblemSourceCodeError"], submissionData);
                                    break;
                            }
                        };

                        if (res.Success)
                        {
                            submissionData.CalculateAverage();
                            res.Data = submissionData;
                        }
                    }
                    else
                    {
                        res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], resGetBatch);
                    }
                }
                else
                {
                    res.OnError(ErrorCode.ProblemCreate, _resource["ProblemCreateError"], resCreateBatch);
                }
            }

            // Nếu thành công thì lưu vào db
            if (res.Success && false)
            {
                // Nếu mà lưu ở 2 chế độ công khai và riêng tư
                if (problemDto.IsPrivateState == true && problemDto.IsPublicState == true)
                {
                    problem.State = ProblemState.Private;

                    var (problemPublic, paramPublic, testcasePublic) = MapCreateProblemDtoToEntity(problemDto);
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

                // Không thì lưu đúng chế độ
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    var newCode = await _repository.GetMaxCodeAsync(ProblemState.Public, _authService.GetAccountId());
                    newCode++;
                    problem.ProblemCode = newCode; await _repository.InsertAsync(problem);
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

        // Submit
        public async Task SubmitAsync(ProblemDto problemDto, ServerResponse res)
        {

        }

        // Build submission params 
        private List<SubmissionParam> BuildSubmissionParams(ProblemDto problemDto, int? judgeId)
        {
            var parames = new List<SubmissionParam>();

            var testcases = problemDto.Testcases;
            var parameters = problemDto.Parameters;

            var limitTime = problemDto.LimitTime;
            var limitMemory = problemDto.LimitMemory;

            var languageId = judgeId;
            var sourceCode = BuildFullSourceCode(problemDto.Solution, parameters, problemDto.OutputType, (LanguageId)(languageId ?? 0));

            if (testcases != null && parameters != null)
            {
                for (int i = 0; i < testcases.Count; i++)
                {
                    var stdin = testcases[i].SerializeInputs();
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

        // Clone 
        public (Problem problem, List<Parameter> parameters, List<Testcase> testcases) MapCreateProblemDtoToEntity(
            ProblemDto problemDto, bool? isClone = false)
        {
            var clone = isClone ?? false;

            var problem = MapCreateDtoToEntity(problemDto);
            problem.AccountId = _authService.GetAccountId();

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

        // Build full source code
        public static string BuildFullSourceCode(string? sourceCode, List<ParameterDto>? param, DataType? outputType, LanguageId languageId)
        {
            var userCode = sourceCode ?? string.Empty;
            var full = userCode;
            var outType = outputType ?? DataType.String;
            var arg = RenderArgument(param, languageId);
            var output = RenderStdout(outType);

            switch (languageId)
            {
                case LanguageId.C:
                    full = ReplaceCode(SourceCode.C, arg, output, userCode);
                    break;
                case LanguageId.Csharp:
                    full = ReplaceCode(SourceCode.CSharp, arg, output, userCode);
                    break;
                case LanguageId.Cpp:
                    full = ReplaceCode(SourceCode.Cpp, arg, output, userCode);
                    break;
                case LanguageId.Js:
                    full = ReplaceCode(SourceCode.Javascript, arg, output, userCode);
                    break;
                case LanguageId.Php:
                    full = ReplaceCode(SourceCode.Php, arg, output, userCode);
                    break;
                case LanguageId.Java:
                    full = ReplaceCode(SourceCode.Java, arg, output, userCode);
                    break;
                case LanguageId.Python:
                    full = ReplaceCode(SourceCode.Python, arg, output, userCode);
                    break;
                default:
                    break;
            };

            return full;
        }

        // Ốp arg vs userCode vào
        public static string ReplaceCode(string sysCode, string args, string stdout, string userCode)
        {
            var result = sysCode.Replace("{args}", args).Replace("{stdout}", stdout).Replace("{userCode}", userCode);
            return result;
        }

        // Build argument
        public static string RenderArgument(List<ParameterDto>? param, LanguageId languageId)
        {
            var result = string.Empty;

            if (param?.Count > 0)
            {
                var args = new List<string>();

                for (int i = 0; i < param.Count; i++)
                {
                    var p = param[i];
                    var exp = Explicit(p.DataType);
                    if (languageId == LanguageId.Php)
                    {
                        args.Add(string.Format(exp, $"$a[{i}]"));
                    }
                    else
                    {
                        args.Add(string.Format(exp, $"a[{i}]"));
                    }
                }

                result = string.Join(", ", args);
            }

            return result;
        }

        // Build explicit
        public static string Explicit(DataType dataType)
        {
            var res = dataType switch
            {
                DataType.String => "{0}",
                DataType.Strings => "stringToStrings({0})",
                DataType.Int => "stringToInt({0})",
                DataType.Ints => "stringToInts({0})",
                DataType.Double => "stringToDouble({0})",
                DataType.Doubles => "stringToDoubles({0})",
                DataType.Bool => "stringToBool({0})",
                DataType.Bools => "stringToBools({0})",
                _ => "{0}"
            };
            return res;
        }

        public static string RenderStdout(DataType dataType)
        {
            var res = dataType switch
            {
                DataType.String => "printString",
                DataType.Strings => "printStrings",
                DataType.Int => "printInt",
                DataType.Ints => "printStrings",
                DataType.Double => "printDouble",
                DataType.Doubles => "printDoubles",
                DataType.Bool => "printBool",
                DataType.Bools => "printBool",
                _ => ""
            };
            return res;
        }
        #endregion
    }
}