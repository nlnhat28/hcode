import BaseService from "./base-service.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class AccountService extends BaseService {
    constructor() {
        super("Account");
    }
    /**
     * Cập nhật đã xác thực
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} accountId Id tài khoản
     * @return
     */
    async updateVerified(accountId) {
        const response = await this.baseRequest.put(
            `${this.baseUrl}/Verified/${accountId}`
        );
        return response;
    }
    /**
     * Cập nhật mật khẩu
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Data mật khẩu mới
     * @return
     */
    async changePassword(data) {
        const response = await this.baseRequest.put(
            this.baseUrl + "/Password",
            data
        );
        return response;
    }
}
const accountService = new AccountService();
export default accountService;
