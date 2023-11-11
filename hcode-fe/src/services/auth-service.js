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
        const response = await this.baseRequest.post(
            this.baseUrl + "/Signup",
            data
        );
        return response;
    }
    /**
     * Đăng nhập
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Thông tin đăng nhập
     * @return
     */
    async login(data) {
        const response = await this.baseRequest.post(
            this.baseUrl + "/Login",
            data
        );
        return response;
    }
    /**
     * Xác thực
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Thông tin xác thực
     * @return
     */
    async verify(data) {
        const response = await this.baseRequest.post(
            this.baseUrl + "/Verify",
            data
        );
        return response;
    }
    /**
     * Gửi email xác thực
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Thông tin tạo mới
     * @return
     */
    async sendVerifyCode(data) {
        const response = await this.baseRequest.post(
            this.baseUrl + "/SendVerifyCode",
            data
        );
        return response;
    }
}
const authService = new AuthService();
export default authService;
