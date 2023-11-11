import _ from "lodash";
import emitter from "tiny-emitter/instance";
import enums from "@/enums/enums.js";
import { t } from "@/i18n/i18n.js";

const commonFuction = {
    /**
     * Check giá trị null hoặc undefined
     */
    isNullValue(value) {
        return !value || value == null;
    },
    /**
     * Check string null hoặc undefined
     */
    isNullString(string) {
        return !string || string == null || string === "";
    },
    /**
     * Sao chép vào clipboard
     */
    copyToClipboard(value) {
        navigator.clipboard.writeText(value);
        return true;
    },
    /**
     * Sleep
     *
     * @param {number} second
     */
    async sleep(second) {
        if (!second) {
            second = 5;
        }
        return new Promise((resolve) => setTimeout(resolve, second * 1000));
    },
    /**
     * Success response
     *
     * @param {number} response
     */
    onSuccess(response) {
        return response && response.Success;
    },
    /**
     * Clone deep
     * 
     * @param {*} obj Object clone
     */
    cloneDeep(obj) {
        return _.cloneDeep(obj);
    },
};
export default commonFuction;
