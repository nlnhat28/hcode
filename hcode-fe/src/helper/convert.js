import enums from "@/enums/enums.js"
import problemEnum from "@/enums/problem-enum.js"
import { t } from "@/i18n/i18n.js";

const convert = {
    /**
     * Lấy màu từ độ khó
     *
     * @param {*} difficulty
     * @returns
     */
    difficultyToColor(difficulty) {
        switch (difficulty) {
            case enums.difficulty.easy.value:
                return enums.color.cyan;
            case enums.difficulty.medium.value:
                return enums.color.orange;
            case enums.difficulty.hard.value:
                return enums.color.red;
            default:
                return "#fff";
        }
    },
    /**
     * Trạng thái account với problem sang icon
     *
     * @param {*} problemAccountState
     * @returns
     */
    problemAccountStateToIcon(problemAccountState) {
        switch (problemAccountState) {
            case problemEnum.problemAccountState.seen.value:
                return "far fa-eye";
            case problemEnum.problemAccountState.doing.value:
                return "far fa-code";
            case problemEnum.problemAccountState.wrong.value:
                return "far fa-circle-xmark";
            case problemEnum.problemAccountState.solved.value:
                return "far fa-circle-check";
            case problemEnum.problemAccountState.draft.value:
                return "far fa-file";
            default:
                return null;
        }
    },
    /**
     * Trạng thái account với problem sang màu
     *
     * @param {*} problemAccountState
     * @returns
     */
    problemAccountStateToColor(problemAccountState) {
        switch (problemAccountState) {
            case problemEnum.problemAccountState.doing.value:
                return "light";
            case problemEnum.problemAccountState.wrong.value:
                return "error";
            case problemEnum.problemAccountState.solved.value:
                return "success";
            default:
                return "light";
        }
    },
    /**
     * Đưa về dạng số gọn
     * 12515 -> 12.5K
     * @param {*} number
     * @returns
     */
    numberToSuffix(number) {
        if (number >= 1000) {
            const suffixes = ["", "K", "M", "B", "T"];
            const suffixNum = Math.floor(("" + number).length / 3);
            let shortValue = parseFloat(
                (suffixNum !== 0
                    ? number / Math.pow(1000, suffixNum)
                    : number
                ).toPrecision(3)
            );
            if (shortValue % 1 !== 0) {
                shortValue = shortValue.toFixed(1);
            }
            return shortValue + suffixes[suffixNum];
        }
        return number?.toString();
    },
    /**
     * Enum thành text
     *
     * @param {*} value
     * @param {*} enumObj
     */
    enumToResource(value, enumObj) {
        for (const key in enumObj) {
            if (enumObj[key].value == value) {
                return enumObj[key].label;
            }
        }
        return null;
    },
    /**
     * Chuyển enum sang filter selects {value, label} => [{key, name}]
     *
     * @param {*} enumObj
     */
    enumToFilterSelects(enumObj) {
        const enumArray = Object.entries(enumObj).map(
            ([key, value]) => ({
                key: value.value,
                name: value.label,
            })
        ); 
        return enumArray;
    },
};
export default convert;
