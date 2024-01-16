import BaseService from "./base-service.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class ContestAccountService extends BaseService {
    constructor() {
        super("ContestAccount");
    }
}
const contestAccountService = new ContestAccountService();
export default contestAccountService;
