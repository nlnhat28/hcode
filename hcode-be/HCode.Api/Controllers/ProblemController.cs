using HCode.Application;
using HCode.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCode.Api
{
    /// <summary>
    /// Controller Auth
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    //[Authorize]
    public class ProblemController : BaseController<ProblemDto, Problem>
    {
        #region Fields
        /// <summary>
        /// Service auth
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IProblemService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service auth</param>
        /// Created by: nlnhat (17/08/2023)
        public ProblemController(IProblemService service, IWebHostEnvironment webHostEnvironment) : base(service, webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách bài toán cho bài thi
        /// </summary>
        /// <param name="authDto"></param>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("ForContest")]
        public async Task<IActionResult> GetForContestAsync()
        {
            var res = new ServerResponse();
            await _service.GetForContestAsync(res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Submit
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync(ProblemDto problemDto)
        {
            var res = new ServerResponse();
            await _service.SubmitAsync(problemDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Submit
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("ProblemAccount")]
        public async Task<IActionResult> AuditProblemAccountAsync(ProblemAccount problemAccount)
        {
            var res = new ServerResponse();
            await _service.AuditProblemAccountAsync(problemAccount, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Get For result
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("ViewResult/{id}/{accountId}")]
        public async Task<IActionResult> ViewResultAsync(Guid id, Guid accountId)
        {
            var res = new ServerResponse();
            await _service.ViewResultAsync(id, accountId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}