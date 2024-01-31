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
    public class ContestController : BaseController<ContestDto, Contest>
    {
        #region Fields
        /// <summary>
        /// Service auth
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IContestService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service auth</param>
        /// Created by: nlnhat (17/08/2023)
        public ContestController(IContestService service, IWebHostEnvironment webHostEnvironment) : base(service, webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Join
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Join")]
        public async Task<IActionResult> JoinAsync(ContestDto contestDto)
        {
            var res = new ServerResponse();
            await _service.JoinAsync(contestDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Leave
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Leave/{contestAccoutId}")]
        public async Task<IActionResult> LeaveAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.LeaveAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Start
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Start/{contestAccoutId}")]
        public async Task<IActionResult> StartAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.StartAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Continue
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Continue/{contestAccoutId}")]
        public async Task<IActionResult> ContinueAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.ContinueAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Finish
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Finish/{contestAccoutId}")]
        public async Task<IActionResult> FinishAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.FinishAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Get For Submit
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("ForSubmit/{id}")]
        public async Task<IActionResult> GetForSubmitAsync(Guid id)
        {
            var res = new ServerResponse();
            await _service.GetForSubmitAsync(id, res);
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
        /// <summary>
        /// Submit
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitAsync(ContestSubmitDto submitDto)
        {
            var res = new ServerResponse();
            await _service.SubmitAsync(submitDto.ContestProblemId, submitDto.ProblemDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}