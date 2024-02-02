import authEnum from "@/enums/auth-enum.js"
const role = authEnum.role;

const auth = {
    getToken() {
        try {
            return this.getAccount()?.Token;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Lấy thông tin account
     */
    logout() {
        try {
            return localStorage.removeItem("__persisted__account");;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Lấy thông tin account
     */
    getAccount() {
        try {
            return JSON.parse(localStorage.getItem("__persisted__account"))
                ?.account;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Lấy id tài khoản
     * @returns
     */
    getAccountId() {
        try {
            return this.getAccount()?.AccountId;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Kiểm tra đúng accountId không
     * @returns
     */
    isAccountId(accountId) {
        try {
            return (
                this.getAccountId() != null && this.getAccountId() == accountId
            );
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Đã đăng nhập
     * @returns
     */
    isAuthenticated() {
        try {
            return this.getAccount()?.Token;
        } catch (error) {
            console.error(error);
            return false;
        }
    },
    /**
     * Lấy role
     */
    getRole() {
        try {
            return this.getAccount()?.Role;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Kiểm tra role
     */
    isRole(roleCode) {
        try {
            return this.getRole() != null && this.getRole() == roleCode;
        } catch (error) {
            console.error(error);
            return false;
        }
    },
    /**
     * Là admin
     * @returns
     */
    isAdmin() {
        try {
            return this.isRole(role.admin);
        } catch (error) {
            console.error(error);
            return false;
        }
    },
    /**
     * Là censor
     * @returns
     */
    isCensor() {
        try {
            return this.isRole(role.censor);
        } catch (error) {
            console.error(error);
            return false;
        }
    },
    /**
     * Là user
     * @returns
     */
    isUser() {
        try {
            return this.isRole(role.user);
        } catch (error) {
            console.error(error);
            return false;
        }
    },
};

export default auth;