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
        accepted: {
            value: 4,
            label: t("problem.problemAccountState.accepted"),
        },
    },
    problemState: {
        /**
         * Công khai
         */
        public: {
            value: 1,
            label: t("problem.problemState.public"),
        },
        /**
         * Công khai nhưng chưa kích hoạt
         */
        // deactivatedPublic: {
        //     value: 2,
        //     label: t("problem.problemState.deactivatedPublic"),
        // },
        /**
         * Riêng tư
         */
        private: {
            value: 3,
            label: t("problem.problemState.private"),
        },
        /**
         * Nháp
         */
        draft: {
            value: 4,
            label: t("problem.problemState.draft"),
        },
    },
    /**
     * Language id
     */
    language: {
        c: 49,
        csharp: 51,
        cpp: 53,
        js: 63,
        php: 68,
        java: 91,
        python: 92,
    },
    /**
     * Kiểu dữ liệu
     */
    dataType: {
        // Chuỗi
        string: {
            value: 1,
            label: "string",
        },
        strings: {
            value: 2,
            label: "string[]",
        },
        // Ký tự
        // char: {
        //     value: 3,
        //     label: "char",
        // },
        // chars: {
        //     value: 4,
        //     label: "char[]",
        // },

        // Số nguyên
        int: {
            value: 5,
            label: "int",
        },
        ints: {
            value: 6,
            label: "int[]",
        },
        // Số thực
        double: {
            value: 7,
            label: "double",
        },
        doubles: {
            value: 8,
            label: "double[]",
        },
        /**
         * Bool
         */
        bool: {
            value: 9,
            label: "bool",
        },
        bools: {
            value: 10,
            label: "bool[]",
        },
    },
    /**
     * Map kiểu dữ liệu theo ngôn ngữ
     * @param {*} dataType Enum data type
     * @param {*} judgeId Id ngôn ngữ
     * @returns
     */
    mapDataTypeByLanguage(dataType, judgeId) {
        switch (dataType) {
            // Kiểu string
            case this.dataType.string.value:
                switch (judgeId) {
                    case this.language.c:
                        return "char*";
                    case this.language.cpp:
                    case this.language.csharp:
                        return "string";
                    case this.language.java:
                        return "String";
                    default:
                        return "";
                }
            // Kiểu string[]
            case this.dataType.strings.value:
                switch (judgeId) {
                    case this.language.c:
                        return "char**";
                    case this.language.cpp:
                        return "vector<string>";
                    case this.language.csharp:
                        return "string[]";
                    case this.language.java:
                        return "String[]";
                    default:
                        return "";
                }
            // Kiểu int
            case this.dataType.int.value:
                switch (judgeId) {
                    case this.language.c:
                    case this.language.cpp:
                    case this.language.csharp:
                    case this.language.java:
                        return "int";
                    default:
                        return "";
                }
            // Kiểu int[]
            case this.dataType.ints.value:
                switch (judgeId) {
                    case this.language.cpp:
                        return "vector<int>";
                    case this.language.c:
                    case this.language.csharp:
                    case this.language.java:
                        return "int[]";
                    default:
                        return "";
                }
            // Kiểu double
            case this.dataType.double.value:
                switch (judgeId) {
                    case this.language.c:
                    case this.language.cpp:
                    case this.language.csharp:
                    case this.language.java:
                        return "double";
                    default:
                        return "";
                }
            // Kiểu double[]
            case this.dataType.doubles.value:
                switch (judgeId) {
                    case this.language.cpp:
                        return "vector<double>";
                    case this.language.c:
                    case this.language.csharp:
                    case this.language.java:
                        return "double[]";
                    default:
                        return "";
                }
            // Kiểu bool
            case this.dataType.bool.value:
                switch (judgeId) {
                    case this.language.c:
                    case this.language.cpp:
                    case this.language.csharp:
                        return "bool";
                    case this.language.java:
                        return "boolean";
                    default:
                        return "";
                }
            // Kiểu bool[]
            case this.dataType.bools.value:
                switch (judgeId) {
                    case this.language.cpp:
                        return "vector<bool>";
                    case this.language.c:
                    case this.language.csharp:
                        return "bool[]";
                    case this.language.java:
                        return "boolean[]";
                    default:
                        return "";
                }
            default:
                return "";
        }
    },
    statusJudge0: {
        Accepted: 3,
        WrongAnswer: 4,
        OverLimit: 50,
        Error: 0,
        InQueue: 1,
        Processing: 2,
        TimeLimitExceeded: 5,
        CompilationError: 6,
        RuntimeErrorSIGSEGV: 7,
        RuntimeErrorSIGXFSZ: 8,
        RuntimeErrorSIGFPE: 9,
        RuntimeErrorSIGABRT: 10,
        RuntimeErrorNZEC: 11,
        RuntimeErrorOther: 12,
        InternalError: 13,
        ExecFormatError: 14,
    },
};

export default problemEnum;
