import _, { values } from "lodash";
import emitter from "tiny-emitter/instance";
import enums from "@/enums/enums.js";
import { t } from "@/i18n/i18n.js";
import { v4 as uuidv4 } from "uuid";

const commonFuction = {
    /**
     * Check giá trị null hoặc undefined
     */
    isNullValue(value) {
        return value == undefined || value == null;
    },
    /**
     * Check string null hoặc undefined hoặc ""
     */
    isEmptyString(string) {
        if (!this.isNullValue(string)) {
            return string === "";
        }
        return true;
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
     * Success response or not
     *
     * @param {number} response
     */
    isSuccess(response) {
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
        if (this.isEmptyString(title)) {
            return window.config.app.title;
        }
        return `${title} - ${window.config.app.title}`;
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
    },
    /**
     * Check 1 mảng không có phần tử
     * @param {*} array
     * @returns
     */
    isEmptyArray(array) {
        if (array) {
            return array.length == 0;
        }
        return true;
    },
    /**
     * Check object không có key nào
     */
    isEmptyObject(obj) {
        if (obj) {
            return Object.keys(obj).length == 0;
        }
        return true;
    },
    /**
     * uuid
     * @returns
     */
    uuid: {
        new() {
            const newId = uuidv4();
            return newId;
        },
        isEmpty(id) {
            return id == "00000000-0000-0000-0000-000000000000";
        },
        empty: "00000000-0000-0000-0000-000000000000",
    },
    
};
export default commonFuction;
