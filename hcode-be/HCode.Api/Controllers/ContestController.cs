﻿using HCode.Application;
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
       
        #endregion
    }
}