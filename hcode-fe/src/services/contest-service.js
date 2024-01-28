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
    /**
     * Rời bài thi
     *
     * Author: nlnhat (17/08/2023)
     * @return
     */
    async leave(contestAccountId) {
        const response = await this.baseRequest.get(
            `${this.baseUrl}/Leave/${contestAccountId}`
        );
        return response;
    }
    /**
     * Kết thúc bài thi
     *
     * Author: nlnhat (17/08/2023)
     * @return
     */
    async finish(contestAccountId) {
        const response = await this.baseRequest.get(
            `${this.baseUrl}/Finish/${contestAccountId}`
        );
        return response;
    }
    /**
     * Lấy thông tin bài thi cho màn submit
     *
     * Author: nlnhat (17/08/2023)
     * @return
     */
    async getForSubmit(contestId) {
        const response = await this.baseRequest.get(
            `${this.baseUrl}/ForSubmit/${contestId}`
        );
        return response;
    }
}
const contestService = new ContestService();
export default contestService;
