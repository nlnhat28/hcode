﻿using HCode.Domain;

namespace HCode.Infrastructure
{
    /// <summary>
    /// Repository role
    /// </summary>
    /// Created by: nlnhat (17/08/2023)
    public class RoleRepository : BaseCodeRepository<Role>, IRoleRepository
    {
        #region Constructors
        /// <summary>
        /// Hàm tạo repository nguyên vật liệu
        /// </summary>
        /// <param name="unitOfWork">Đối tượng unit of work được inject</param>
        /// Created by: nlnhat (17/08/2023)
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        
        #endregion
    }
}

