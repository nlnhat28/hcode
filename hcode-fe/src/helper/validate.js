import cf from "@/helper/common-function.js";
import regex from "@/helper/regex.js";
/**
 * Các hàm validate thông dụng
 */
const validate = {
    validateEmail: (label, value) => {
        if (cf.isNullString(value) || regex.regexEmail(value)) {
            return null;
        };
        return resources["vn"].invalidEmail;
    },
};
export default validate;
