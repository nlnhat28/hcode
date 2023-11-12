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
     * Cập nhật đã xác thực
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} accountId Id tài khoản
     * @return
     */
    async updatePassword(accountId) {
        const response = await this.baseRequest.put(
            `${this.baseUrl}/Password/${accountId}`
        );
        return response;
    }
}
const accountService = new AccountService();
export default accountService;
