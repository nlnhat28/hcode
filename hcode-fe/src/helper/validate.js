import cf from "@/helper/common-function.js";
import regex from "@/helper/regex.js";
import { t } from "@/i18n/i18n.js";
/**
 * Các hàm validate thông dụng
 */
const validate = {
    /**
     * Validate email
     * @param {*} label 
     * @param {*} value 
     * @returns 
     */
    email(label, value) {
        if (cf.isNullString(value) || regex.email(value)) {
            return null;
        }
        else if (!value.includes("@")) {
            return null;
        }
        return t("msg.invalidEmail");
    },
    /**
     * Validate mật khẩu
     * @param {*} label 
     * @param {*} value 
     * @returns 
     */
    password(label, value) {
        if (cf.isNullString(value) || regex.password(value)) {
            return null;
        }
        return t("msg.weakPassword");
    },
};
export default validate;
