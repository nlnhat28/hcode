const regex = {
    /**
     * Regex email
     * @param {*} value Email để check 
     * @returns True nếu đúng format của email
     */
    regexEmail: (value) => {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(value);
    },
};
export default regex;