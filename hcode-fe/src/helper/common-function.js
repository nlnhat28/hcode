const commonFuction = {
    /**
     * Check giá trị null hoặc undefined
     */
    isNullValue: (value) => {
        return !value || value == null;
    },
    /**
     * Check string null hoặc undefined
     */
    isNullString: (string) => {
        return !string || string == null || string === "";
    },
    /**
     * Sao chép vào clipboard
     */
    copyToClipboard: (value) => {
        navigator.clipboard.writeText(value);
        return true;
    },
};
export default commonFuction;