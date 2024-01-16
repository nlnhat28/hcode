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
    public class SubmissionController : BaseController<SubmissionDto, Submission>
    {
        #region Fields
        /// <summary>
        /// Service auth
        /// </summary>
        /// Created by: nlnhat (17/08/2023)
        private readonly ISubmissionService _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        /// <param name="service">Service auth</param>
        /// Created by: nlnhat (17/08/2023)
        public SubmissionController(ISubmissionService service, IWebHostEnvironment webHostEnvironment) : base(service, webHostEnvironment)
        {
            _service = service;
        }
        #endregion

        #region Methods
       
        #endregion
    }
}