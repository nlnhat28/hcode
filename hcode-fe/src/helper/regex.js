const regex = {
    /**
     * Regex email
     * @param {*} value Email để check
     * @returns True nếu đúng format của email
     */
    email(value) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(value);
    },
    /**
     * Regex password
     * @param {*} value Password để check
     * @returns True nếu password >= 8 ký tự, gồm cả chữ hoa, chữ thường, số và ký tự đặc biệt
     */
    password(value) {
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@;$!%*?&^#<>|~_=\-,.()"'`/{}[\]])[A-Za-z\d@;$!%*?&^#<>|~_=\-,.()"'`/{}[\]]{8,}$/;
;
        return regex.test(value);
    },
};
export default regex;