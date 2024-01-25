import BaseService from "./base-service.js";
/**
 * Service contest
 * Author: nlnhat (17/08/2023)
 */
class ContestService extends BaseService {
    constructor() {
        super("Contest");
    }
    /**
     * Tạo quan hệ bài toán, người dùng
     *
     * Author: nlnhat (17/08/2023)
     * @return
     */
    async join(data) {
        const response = await this.baseRequest.post(
            this.baseUrl + "/Join",
            data
        );
        return response;
    }
}
const contestService = new ContestService();
export default contestService;
