import bcrypt from "bcryptjs";

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
        if (this.isNullValue(value)) {
            return null;
        }
        if (this.isNullValue(salt)) {
            salt = "";
        }
        var hashed = bcrypt.hashSync(value, salt);
        return hashed;
    },
    /**
     * Compare value has
     * @param {*} password
     * @param {*} salt
     */
    compareHash(value1, value2, salt) {
        if (this.isNullValue(value1) || this.isNullValue(value2)) {
            return false;
        }
        if (this.isNullValue(salt)) {
            salt = "";
        }
        var hashed = bcrypt.hashSync(value, salt);
        return hashed;
    },
};
export default commonFuction;
