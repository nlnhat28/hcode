import rfm from "./reformat";
import cf from "./common-function";

const format = {
    /**
     * Format số thập phân
     *
     * @param {*} value
     * @returns
     */
    formatDecimal(value, fraction, locale) {
        if (isNaN(value) || value == null) {
            return value;
        }

        let _fraction = 2;
        let _locale = "vi-VN";

        if (fraction) {
            _fraction = fraction;
        }

        if (locale) {
            _locale = locale;
        }

        const formattedNumber = value.toLocaleString(_locale, {
            style: "decimal",
            minimumFractionDigits: _fraction,
            maximumFractionDigits: _fraction,
        });
        return formattedNumber;
    },
    /**
     * Format number seperated by separator
     *
     * Author: nlnhat (02/07/2023)
     *
     * @param {*} number
     * @param {*} separator
     * @returns {string} Number format, example: xxx-xxx-xxx
     */
    formatNumberBySeparator(number, separator) {
        try {
            let newNumber = rfm.cleanNotDigitChar(number);
            newNumber = rfm.cleanFormatIntNumber(newNumber);
            if (cf.isNullValue(newNumber)) return null;
            if (!isNaN(newNumber))
                return new Intl.NumberFormat("en")
                    .format(newNumber)
                    .replace(/[,]/g, separator);
        } catch (error) {
            console.error(error);
            return number;
        }
    },
    /**
     * Format number seperated by dot: xxx.xxx.xxx
     *
     * Author: nlnhat (02/07/2023)
     *
     * @param {*} number
     * @returns {string} Number format: xxx.xxx.xxx
     */
    formatNumberByDot(number) {
        const result = format.formatNumberBySeparator(number, ".");
        return result;
    },
    /**
     * Format number seperated by space: xxx xxx xxx
     *
     * Author: nlnhat (02/07/2023)
     *
     * @param {*} number
     * @returns {string} Number format: xxx xxx xxx
     */
    formatNumberBySpace(number) {
        const result = format.formatNumberBySeparator(number, " ");
        return result;
    },
    /**
     * Format số nguyên
     */
    formatInteger(value) {
        try {
            let newNumber = rfm.cleanNotDigitChar(value);
            if (cf.isEmptyString(newNumber)) return null;
            if (!isNaN(newNumber))
                return new Intl.NumberFormat("en")
                    .format(newNumber)
                    .replace(/[,]/g, " ");
        } catch (error) {
            console.error(error);
            return value;
        }
    },
};
export default format;
