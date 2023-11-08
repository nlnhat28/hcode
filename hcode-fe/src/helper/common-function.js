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
};
export default commonFuction;
