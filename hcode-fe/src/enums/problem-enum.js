import { t } from "@/i18n/i18n.js";

const problemEnum = {
    /**
     * Trạng thái người dùng với bài toán
     */
    problemAccountState: {
        none: {
            value: 0,
            label: t("problem.problemAccountState.none"),
        },
        /**
         * Đã xem
         */
        seen: {
            value: 1,
            label: t("problem.problemAccountState.seen"),
        },
        /**
         * Đang làm
         */
        doing: {
            value: 2,
            label: t("problem.problemAccountState.doing"),
        },
        /**
         * Submit sai
         */
        wrong: {
            value: 3,
            label: t("problem.problemAccountState.wrong"),
        },
        /**
         * Đã giải
         */
        solved: {
            value: 4,
            label: t("problem.problemAccountState.solved"),
        },
        /**
         * Nháp
         */
        draft: {
            value: 5,
            label: t("problem.problemAccountState.draft"),
        },
    },
};

export default problemEnum;
