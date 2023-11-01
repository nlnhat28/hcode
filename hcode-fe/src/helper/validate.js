import cf from "@/helper/common-function.js";
import regex from "@/helper/regex.js";
import { t } from "@/i18n/i18n.js";
/**
 * Các hàm validate thông dụng
 */
const validate = {
    validateEmail: (label, value) => {
        if (cf.isNullString(value) || regex.regexEmail(value)) {
            return null;
        };
        return t("msg.invalidEmail");
    },
};
export default validate;
