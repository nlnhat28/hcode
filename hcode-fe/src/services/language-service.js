import BaseService from "./base-service.js";
/**
 * Service auth
 * Author: nlnhat (17/08/2023)
 */
class LanguageService extends BaseService {
    constructor() {
        super("Language");
    }
}
const languageService = new LanguageService();
export default languageService;
