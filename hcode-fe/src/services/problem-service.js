import BaseService from "./base-service.js";
/**
 * Service problem
 * Author: nlnhat (17/08/2023)
 */
class ProblemService extends BaseService {
    constructor() {
        super("Problem");
    }
    /**
     * Submit problem
     *
     * Author: nlnhat (17/08/2023)
     * @param {*} data Thông tin
     * @return
     */
    async submit(data) {
        const response = await this.baseRequest.post(
            this.baseUrl + "/Submit",
            data
        );
        return response;
    }
}
const problemService = new ProblemService();
export default problemService;
