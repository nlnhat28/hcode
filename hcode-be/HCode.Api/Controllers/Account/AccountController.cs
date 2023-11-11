﻿using HCode.Application;
using HCode.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HCode.Api
{
    /// <summary>
    /// Controller account
    /// </summary>
    /// Created by: nlnhat (13/07/2023)
    public class AccountController : BaseController<AccountDto, Account>
    {
        #region Fields
        /// <summary>
        /// Service account
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly IAccountService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service account</param>
        /// Created by: nlnhat (17/08/2023)
        public AccountController(IAccountService service, IWebHostEnvironment webHostEnvironment) : base(service, webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Cập nhật đã xác thực
        /// </summary>
        /// <param name="id">Account Id</param>
        /// Created by: nlnhat (17/08/2023)
        [HttpPut("UpdateVerified/{id}")]
        public async Task<IActionResult> UpdateVerifyAsync(Guid id)
        {
            var res = new ServerResponse();
            await _service.UpdateVerifyAsync(id, res);
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}