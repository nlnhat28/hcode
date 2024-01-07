
using AutoMapper;
using HCode.Domain;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit;
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
        #endregion
    }
}