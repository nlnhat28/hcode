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
            if (!isNaN(newNumber)) {
                let intValue = new Intl.NumberFormat("en")
                    .format(newNumber)
                    .replace(/[,]/g, " ");
                intValue = parseInt(intValue);
                return intValue;
            }
        } catch (error) {
            console.error(error);
            return value;
        }
    },
    /**
     * Format decimal
     */
    formatDecimal(value) {
        try {
            let newNumber = rfm.cleanNotDigitChar(value);
            if (cf.isEmptyString(newNumber)) return null;
            if (!isNaN(newNumber)) {
                let intValue = new Intl.NumberFormat("en")
                    .format(newNumber)
                    .replace(/[,]/g, " ");
                intValue = parseInt(intValue);
                return intValue;
            }
        } catch (error) {
            console.error(error);
            return value;
        }
    },
    /**
     * Format data time
     *
     * @param {*} _dateTime Date time
     * @param {*} _pattern Cấu trúc
     * @param {*} _to Mục đích
     */
    formatDateTime(_dateTime, _pattern, _to) {
        if (_dateTime) {
            try {
                const pattern = _pattern || "dd/mm/yyyy";
                const dateObject = new Date(_dateTime);
                let date = String(dateObject.getDate()).padStart(2, "0");
                let month = String(dateObject.getMonth() + 1).padStart(2, "0");
                let year = dateObject.getFullYear();
                let hour = String(dateObject.getHours()).padStart(2, "0");
                let minute = String(dateObject.getMinutes()).padStart(2, "0");
                let second = String(dateObject.getSeconds()).padStart(2, "0");
                let result = _dateTime;

                switch (pattern) {
                    case "dd/mm/yyyy":
                        result = `${date}/${month}/${year}`;
                        break;
                    case "dd/mm/yyyy hh/mm/ss":
                        result = `${date}/${month}/${year} ${hour}:${minute}:${second}`;
                        break;
                    case "dd/mm/yyyy hh/mm":
                        result = `${date}/${month}/${year} ${hour}:${minute}`;
                        break;
                    default:
                        break;
                }

                return result;

            } catch (error) {
                console.error(error);
                return _dateTime;
            }
        }
        return null;
    },
    /**
     * Làm tròn số thập phân
     * @param {*} value Số thập phân
     * @param {*} fixed Số chữ số sau dấu phẩy
     */
    roundDecimal(value, fixed = 2, nullToZero) {
        if (value != null) {
            return value.toFixed(fixed);
        };

        if (nullToZero) {
            return 0;
        }
        return value;
    }
};
export default format;
