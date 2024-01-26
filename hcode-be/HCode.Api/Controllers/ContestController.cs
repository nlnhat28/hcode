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
        public async Task<IActionResult> JoinAsync(ContestAccountDto contestAccountDto)
        {
            var res = new ServerResponse();
            await _service.JoinAsync(contestAccountDto, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Leave
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Leave/{id}")]
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
        [HttpGet("Start/{id}")]
        public async Task<IActionResult> StartAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.StartAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        /// <summary>
        /// Finish
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        [HttpGet("Finish/{id}")]
        public async Task<IActionResult> FinishAsync(Guid contestAccoutId)
        {
            var res = new ServerResponse();
            await _service.FinishAsync(contestAccoutId, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}