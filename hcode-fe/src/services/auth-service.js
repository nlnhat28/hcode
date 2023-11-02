import BaseService from "./base-service.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class AuthService extends BaseService {
    constructor() {
        super("Auth");
    }
    /**
     * Tạo mới tài khoản
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Thông tin tạo mới
     * @return 
     */
    async signup(data) {
        const response = await this.baseRequest.post(this.baseUrl + "/Signup", {
            data: {
                data,
            },
        });
        return response;
    }
};
const authService = new AuthService();
export default authService;