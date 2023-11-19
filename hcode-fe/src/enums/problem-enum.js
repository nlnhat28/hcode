const problemEnum = {
    /**
     * Trạng thái người dùng với bài toán
     */
    problemAccountStatus: {
        none: 0,
        /**
         * Đã xem
         */
        seen: 1,
        /**
         * Đang làm
         */
        doing: 2,
        /**
         * Submit sai
         */
        wrong: 3,
        /**
         * Đã giải
         */
        solved: 4
    },
};

export default problemEnum;
