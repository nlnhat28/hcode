
using AutoMapper;
using HCode.Domain;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HCode.Application
{
    // Dịch vụ thực thi code
    public class CEService : CoreService, ICEService
    {
        #region Fields
        /// <summary>
        /// CE Config
        /// </summary>
        private readonly CEConfig _ceConfig;
        /// <summary>
        /// Encode base64 không
        /// </summary>
        private readonly bool _isEncode;
        /// <summary>
        /// Uri base encode. Vd: base64_encoded=true
        /// </summary>
        private readonly string _base64Encoded;
        #endregion

        #region Constructors
        public CEService(IOptions<CEConfig> ceConfig, IStringLocalizer<Resource> resource, IMapper mapper)
            : base(resource, mapper)
        {
            _ceConfig = ceConfig.Value;

            _isEncode = _ceConfig.IsBase64Encode ?? true;
            _base64Encoded = $"base64_encoded={_isEncode.ToString().ToLower()}";
        }
        #endregion

        #region Methods
        // Gửi request đến server
        public async Task<BaseResponse> SendAsync(string uri, HttpMethod method, string? content = null)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(uri),
                    Headers =
                    {
                        { "X-RapidAPI-Key", _ceConfig.ApiKey },
                        { "X-RapidAPI-Host", _ceConfig.ApiHost },
                    }
                };

                if (content != null)
                {
                    request.Content = new StringContent(content)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    };
                }

                using var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                var res = new BaseResponse();

                if (response.IsSuccessStatusCode)
                {
                    res.OnSuccess(data: body);
                    return res;
                }

                res.OnError(data: body);
                return res;
            }
            catch (Exception exception)
            {
                return new BaseResponse(exception);
            }

        }

        // Tạo 1 submission
        public async Task<BaseResponse> CreateSubmissionAsync(SubmissionParam param)
        {
            var method = HttpMethod.Post;
            var fields = AppHelper.GetFieldsToString<SubmissionResponse>() ?? "*";
            var uri = $"{_ceConfig.BaseUrl}/submissions?{_base64Encoded}&fields={fields}&wait=true";

            if (_isEncode)
            {
                param.Encode();
            }

            var content = JsonSerializer.Serialize(param);

            var response = await SendAsync(uri, method, content);

            if (response.Success)
            {
                var jsonData = (string)(response.Data ?? string.Empty);
                var data = JsonSerializer.Deserialize<SubmissionResponse>(jsonData);

                if (_isEncode)
                {
                    data?.Decode();
                }

                data?.GenStatusName();

                response.Data = data;
            }

            return response;
        }

        // Tạo nhiều submission
        public async Task<BaseResponse> CreateBatchAsync(List<SubmissionParam> param)
        {
            var method = HttpMethod.Post;
            var uri = $"{_ceConfig.BaseUrl}/submissions/batch?{_base64Encoded}";

            if (_isEncode)
            {
                param.ForEach(item => item.Encode());
            }

            var obj = new
            {
                submissions = param
            };

            var content = JsonSerializer.Serialize(obj);

            var response = await SendAsync(uri, method, content);

            if (response.Success)
            {
                var jsonData = (string)(response.Data ?? string.Empty);
                var data = JsonSerializer.Deserialize<List<SubmissionResponse>>(jsonData);
                response.Data = data;
            }

            return response;
        }

        // Lấy nhiều submission
        public async Task<BaseResponse> GetBatchAsync(List<string> tokens)
        {
            var method = HttpMethod.Get;
            var fields = AppHelper.GetFieldsToString<SubmissionResponse>() ?? "*";
            var tokensString = string.Join(",", tokens);
            var uri = $"{_ceConfig.BaseUrl}/submissions/batch?tokens={tokensString}&{_base64Encoded}&fields={fields}";

            var response = await SendAsync(uri, method);

            if (response.Success)
            {
                var jsonData = (string)(response.Data ?? string.Empty);
                var submissions = JsonSerializer.Deserialize<Submissions>(jsonData) ?? new Submissions();
                var data = submissions.submissions;

                if (_isEncode)
                {
                    data?.ForEach(item =>
                    {
                        item.Decode();
                        item.GenStatusName();
                    });
                }
                else
                {
                    data?.ForEach(item => item.GenStatusName());
                }

                response.Data = data;
            }

            return response;
        }

        // Thực thi bài toán
        public async Task ExecuteAsync(ProblemDto problemDto, List<Testcase> testcases, ServerResponse res)
        {
            var parames = BuildSubmissionParams(problemDto, problemDto.SolutionLanguage?.JudgeId);

            var resCreateBatch = await CreateBatchAsync(parames);

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

                    var _res = await GetBatchAsync(processingTokens);

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
                                var time = Convert.ToDouble(subRes.time, CultureInfo.InvariantCulture);
                                var memory = subRes.memory;
                                var errors = new List<string>();

                                if (problemDto.LimitTime != null && problemDto.LimitTime > 0 && time > problemDto.LimitTime)
                                {
                                    var msg = string.Format(_resource["ProblemOverLimitTime"], time + "s");
                                    errors.Add(msg);
                                }
                                if (problemDto.LimitMemory != null && problemDto.LimitMemory > 0 && memory > problemDto.LimitMemory)
                                {
                                    var msg = string.Format(_resource["ProblemOverLimitMemory"], memory + "kb");
                                    errors.Add(msg);
                                }

                                if (errors?.Count > 0)
                                {
                                    var userMsg = string.Join(" ", errors);
                                    subRes.status_id = StatusJudge0.OverLimit;
                                    subRes.status_name = StatusJudge0.OverLimit.ToString();
                                    subRes.user_msg = userMsg;
                                }

                                res.OnError(ErrorCode.ProblemTestcase, _resource["ProblemTestcaseError"], submissionData);
                                break;
                            case StatusJudge0.TimeLimitExceeded:
                                res.OnError(ErrorCode.ProblemTimeLimitExceeded, _resource["ProblemTimeLimitExceeded"], submissionData);
                                break;
                            default:
                                res.OnError(ErrorCode.ProblemSourceCode, _resource["ProblemSourceCodeError"], submissionData);
                                break;
                        }
                    };

                    submissionData.CalculateResult();
                    res.Data = submissionData;
                }
                else
                {
                    res.OnError(ErrorCode.ProblemExecute, _resource["ProblemExecuteError"], resCreateBatch);
                }
            }
            else
            {
                res.OnError(ErrorCode.ProblemExecute, _resource["ProblemExecuteError"], resCreateBatch);
            }
        }

        /// <summary>
        /// Build submission params 
        /// </summary>
        /// <param name="problemDto"></param>
        /// <param name="judgeId"></param>
        /// <returns></returns>
        private static List<SubmissionParam> BuildSubmissionParams(ProblemDto problemDto, int? judgeId)
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

        /// <summary>
        /// Build full source code
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="param"></param>
        /// <param name="outputType"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        private static string BuildFullSourceCode(string? sourceCode, List<ParameterDto>? param, DataType? outputType, LanguageId languageId)
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

        /// <summary>
        /// Ốp arg vs userCode vào
        /// </summary>
        /// <param name="sysCode"></param>
        /// <param name="args"></param>
        /// <param name="stdout"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private static string ReplaceCode(string sysCode, string args, string stdout, string userCode)
        {
            var result = sysCode.Replace("{args}", args).Replace("{stdout}", stdout).Replace("{userCode}", userCode);
            return result;
        }

        /// <summary>
        /// Build argument
        /// </summary>
        /// <param name="param"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        private static string RenderArgument(List<ParameterDto>? param, LanguageId languageId)
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

        /// <summary>
        /// Build explicit
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>        
        private static string Explicit(DataType dataType)
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

        /// <summary>
        /// Build stdout
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static string RenderStdout(DataType dataType)
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