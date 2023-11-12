using AutoMapper;
using HCode.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
namespace HCode.Application
{
    /// <summary>
    /// Triển khai service account
    /// </summary> 
    /// Created by: nlnhat (15/07/2023)
    public class AccountService : BaseService<AccountDto, Account>, IAccountService
    {
        #region Fields
        /// <summary>
        /// Service gửi email
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// Repo account
        /// </summary>
        private new readonly IAccountRepository _repository;
        /// <summary>
        /// Repo vai trò
        /// </summary>
        private readonly IRoleRepository _roleRepository;
        /// <summary>
        /// Cache
        /// </summary>
        /// Created by: nlnhat (13/07/2023
        protected readonly IMemoryCache _cache;
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
        public AccountService(IAccountRepository repository, IRoleRepository roleRepository,
                           IStringLocalizer<Resource> resource, IMapper mapper, 
                           IUnitOfWork unitOfWork, IEmailService emailService, IMemoryCache cache)
                         : base(repository, resource, mapper, unitOfWork)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _cache = cache;
        }
        #endregion

        #region Methods
        public override Account MapCreateDtoToEntity(AccountDto accountDto)
        {
            accountDto.AccountId = Guid.NewGuid();
            accountDto.CreatedTime ??= DateTime.UtcNow;

            var account = _mapper.Map<Account>(accountDto);

            return account;
        }

        public override Account MapUpdateDtoToEntity(AccountDto accountDto)
        {
            accountDto.ModifiedTime = DateTime.UtcNow;

            var account = _mapper.Map<Account>(accountDto);

            return account;
        }

        public Task ValidateAsync(AuthDto entity)
        {
            throw new NotImplementedException();
        }

        // Cập nhật đã xác thực
        public async Task UpdateVerifyAsync(Guid accountId, ServerResponse res)
        {
            var accountIds = new List<Guid>
            {
                accountId
            };

            var updateRes = await _repository.UpdateVerifiedAsync(accountIds);

            res.Data = updateRes;
        }

        // Cập nhật mật khẩu xác thực
        public async Task ChangePasswordAsync(AccountDto accountDto, ServerResponse res)
        {
            var (newPassword, salt) = ApplicationHelper.HashPassword(accountDto.Password);

            var updateRes = await _repository.UpdatePasswordAsync(accountDto.AccountId, newPassword, salt);

            res.Data = updateRes;
        }
        #endregion
    }
}