import BaseService from "./baseService.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class AuthService extends BaseService {
    constructor() {
        super("Auth");
    }
};
const authService = new AuthService();
export default authService;