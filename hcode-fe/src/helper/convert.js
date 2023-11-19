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
            case enums.difficulty.easy:
                return enums.color.cyan;
            case enums.difficulty.medium:
                return enums.color.orange;
            case enums.difficulty.hard:
                return enums.color.red;
            default:
                return "#fff";
        }
    },
    /**
     * Trạng thái account với problem sang icon
     *
     * @param {*} problemAccountStatus
     * @returns
     */
    problemAccountStatusToIcon(problemAccountStatus) {
        switch (problemAccountStatus) {
            case problemEnum.problemAccountStatus.seen:
                return "far fa-eye";
            case problemEnum.problemAccountStatus.doing:
                return "far fa-code";
            case problemEnum.problemAccountStatus.wrong:
                return "far fa-circle-xmark";
            case problemEnum.problemAccountStatus.solved:
                return "far fa-circle-check";
            default:
                return null;
        }
    },
    /**
     * Trạng thái account với problem sang màu
     *
     * @param {*} problemAccountStatus
     * @returns
     */
    problemAccountStatusToColor(problemAccountStatus) {
        switch (problemAccountStatus) {
            case problemEnum.problemAccountStatus.doing:
                return "light";
            case problemEnum.problemAccountStatus.wrong:
                return "error";
            case problemEnum.problemAccountStatus.solved:
                return "success";
            default:
                return "light";
        }
    },
    /**
     * Trạng thái account với problem sang text
     *
     * @param {*} problemAccountStatus
     * @returns
     */
    problemAccountStatusToText(problemAccountStatus) {
        switch (problemAccountStatus) {
            case problemEnum.problemAccountStatus.seen:
                return t("problem.problemAccountStatus.seen");
            case problemEnum.problemAccountStatus.doing:
                return t("problem.problemAccountStatus.doing");
            case problemEnum.problemAccountStatus.wrong:
                return t("problem.problemAccountStatus.wrong");
            case problemEnum.problemAccountStatus.solved:
                return t("problem.problemAccountStatus.solved");
            default:
                return null;
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
        return number.toString();
    },
};
export default convert;
