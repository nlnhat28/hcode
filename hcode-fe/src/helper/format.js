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
};
export default format;
