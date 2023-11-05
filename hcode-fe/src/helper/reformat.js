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
            };
        } catch (error) {
            console.error(error);
        }
        return value;
    },
};
export default reformat;
