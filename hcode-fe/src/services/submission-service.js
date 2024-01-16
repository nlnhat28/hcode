import BaseService from "./base-service.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class SubmissionService extends BaseService {
    constructor() {
        super("Submission");
    }
}
const submissionService = new SubmissionService();
export default submissionService;
