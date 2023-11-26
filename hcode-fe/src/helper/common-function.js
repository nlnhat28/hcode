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
    /**
     * Tạo document title
     */
    documentTitle(title) {
        if (this.isNullString(title)) {
            return window.config.app.title;
        }
        return `${title} - ${window.config.app.title}`
    },
    /**
     * Tạo mảng n phần tử rỗng
     */
    genArrayEmpty(n) {
        let amount = 1;
        if (n && Number.isInteger(n) && n > 0) {
            amount = n;
        }

        const arr = Array.from({ length: amount }, () => ({}));
        return arr;
    }
};
export default commonFuction;
