import { t } from "@/i18n/i18n.js";

const enums = {
    /**
     * Chế độ sử dụng form
     */
    formMode: {
        create: 1,
        post: 1,
        update: 2,
        put: 2,
        duplicate: 3,
    },
    /**
     * Http status
     */
    httpStatus: {
        ok: 200,
        created: 201,
        noContent: 204,
        serverError: 500,
        clientError: 400,
    },
    /**
     * Key code
     */
    keyCode: {
        tab: 9,
        enter: 13,
        shift: 16,
        ctrl: 17,
        alt: 18,
        esc: 27,
        space: 32,
        left: 37,
        up: 38,
        right: 39,
        down: 40,
        insert: 45,
        delete: 46,
        digit1: 49,
        a: 65,
        b: 66,
        c: 67,
        d: 68,
        e: 69,
        f: 70,
        g: 71,
        h: 72,
        i: 73,
        j: 74,
        k: 75,
        l: 76,
        m: 77,
        n: 78,
        r: 82,
        s: 83,
        t: 84,
        u: 85,
        v: 86,
        y: 89,
        f1: 112,
        f2: 113,
        f3: 114,
        f8: 119,
    },
    /**
     * Kiểu dialog
     */
    dialogType: {
        error: 2,
        warn: 3,
        info: 4,
        confirm: 5,
    },
    /**
     * Màu
     */
    color: {
        yellow: "#f2d066",
        orange: "#ffad33",
        orange200: "#ffd699",
        blue: "#3395ff",
        red: "#ff3232",
        green: "#0ecc57",
        cyan: "#00f2fa",
    },
    /**
     * Type of sort
     */
    sortType: {
        disable: 0,
        asc: 1,
        desc: 2,
        blur: 3,
    },
    /**
     * Type of compare
     */
    compareType: {
        contain: 1,
        notContain: 2,
        equal: 3,
        less: 4,
        more: 5,
        range: 6,
        empty: 7,
        startWith: 8,
        endWith: 9,
        lessEqual: 10,
        moreEqual: 11,
        notEqual: 12,
    },
    /**
     * Type of logic
     */
    logicType: {
        and: 1,
        or: 2,
    },
    /**
     * Type of data filter
     */
    filterType: {
        disable: 0,
        text: 1,
        date: 2,
        selectKey: 3,
        selectName: 4,
        selectMany: 5,
        number: 6,
    },
    dataInput: {
        text: 1,
        integer: 2,
        decimal: 3,
    },
    /**
     * Apply state
     */
    applyState: {
        activated: 1,
        deactivated: 2,
    },
    /**
     * Type of error key in error data
     */
    errorKey: {
        formItem: 1,
    },
    /**
     * Độ khó
     */
    difficulty: {
        easy: {
            value: 1,
            label: t("enums.difficulty.easy"),
        },
        medium: {
            value: 2,
            label: t("enums.difficulty.medium"),
        },
        hard: {
            value: 3,
            label: t("enums.difficulty.hard"),
        },
    },
    /**
     * Đơn vị thời gian
     */
    timeUnit: {
        millisecond: {
            value: 3,
            label: "millisecond",
        },
        second: {
            value: 4,
            label: "second",
        },
    },
    /**
     * Đơn vị bộ nhớ
     */
    memoryUnit: {
        byte: {
            value: 1,
            label: "byte",
        },
        kilobyte: {
            value: 2,
            label: "kb",
        },
        megebyte: {
            value: 2,
            label: "mb",
        },
    },
};
export default enums;
