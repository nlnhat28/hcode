import bcrypt from "bcryptjs";
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
     * Hàm băm
     * @param {*} password
     * @param {*} salt
     */
    hash(value, salt) {
        if (this.isNullValue(value) || this.isNullValue(salt)) {
            return value;
        }
        var hashed = bcrypt.hashSync(value, salt);
        return hashed;
    },
    /**
     * Gen salt
     * @param {*} round
     * @returns
     */
    genSalt() {
        const saltRounds = 10;
        const salt = bcrypt.genSaltSync(saltRounds);
        return salt;
    },
    /**
     * Sleep
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
     */
    onError(error, owner) {
        const data = error.data,
              userMsg = data.UserMsg,
              key = data.Data?.Key,
              errorKey = data.Data?.ErrorKey;

        let message = t("msg.clientError"),
            actionOnClose = null;

        if (userMsg && userMsg != "") {
            message = userMsg;
        }

        if 
        switch (errorKey) {
            // Nếu error key là form item thì focus vào form item bị lỗi
            case enums.errorKey.formItem:
                let ref = null;
                if (key && key != "") ref = key;

                emitter.emit("setMessageFormItem", ref, message);

                actionOnClose = () => {
                    emitter.emit("focusFormItem", ref);
                };
                break;

            default:
                break;
        }

        emitter.emit("dialogError", message, actionOnClose);
    }
    /**
     * Clone deep
     */
    cloneDeep(obj) {
        return _.cloneDeep(obj);
    }
};
export default commonFuction;
