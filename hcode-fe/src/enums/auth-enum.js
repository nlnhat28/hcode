const authEnum = {
    formType: {
        login: 1,
        createAccount: 2,
    },
    verifyMode: {
        signup: 1,
        changePassword: 2,
        changeEmail: 3,
    },
    /** 
     * Vai trò 
     */
    role: {
        admin: 1,
        cencor: 2,
        user: 3,
    }
};

export default authEnum;
