const reformat = {
    /**
     * Loại bỏ hết space
     * @param {*} value
     * @returns
     */
    removeSpace(value) {
        try {
            if (value) {
                return value.toString().replace(/\s/g, "");
            }
        } catch (error) {
            console.error(error);
            return value;
        }
    },
    /**
     * Clean character not digit char
     * @param {*} value Value to clean
     * @returns Number
     */
    cleanNotDigitChar(value) {
        try {
            if (value) return value.replace(/\D/g, "");
        } catch (error) {
            console.log(error);
        }
        return value;
    },
    /**
     * Clean format int number [Tạm bỏ]
     * @param {*} value Number to clean
     * @returns Number only
     */
    cleanFormatIntNumber(value) {
        try {
            if (value) return value.replace(/[\s.,]/g, "");
        } catch (error) {
            console.log(error);
        }
        return value;
    },
    /**
     * Clean format integer / Dùng khi format bằng hàm formatInteger
     * @param {*} value Number to clean
     * @returns Number only
     */
    cleanFormatInteger(value) {
        try {
            if (value) {
                return value.toString().replace(/\s/g, "");
            }
        } catch (error) {
            console.error(error);
            return value;
        }
    },
    /**
     * Reformat date
     *
     * Author: nlnhat (02/07/2023)
     * @param {*} date Value to reformat
     * @returns Value with T00:00:00
     */
    reformatDate(date) {
        try {
            if (date != null) {
                return `${date}T00:00:00`;
            }
        } catch (error) {
            console.error(error);
        }
        return date;
    },
};
export default reformat;
