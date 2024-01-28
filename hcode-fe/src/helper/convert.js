import enums from "@/enums/enums.js"
import problemEnum from "@/enums/problem-enum.js"
import contestEnum from "@/enums/contest-enum.js"
import { t } from "@/i18n/i18n.js";
import problem from "../i18n/vi/i18n-problem";

const convert = {
    /**
     * Lấy màu từ độ khó
     *
     * @param {*} difficulty
     * @returns
     */
    difficultyToColor(difficulty) {
        try {
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
        } catch (error) {
            console.error(error);
            return "#fff";
        }
    },
    /**
     * Lấy màu theo trạng thái thi
     *
     * @param {*} contestAccountState
     * @returns
     */
    contestAccountStateToColor(contestAccountState) {
        try {
            let state = contestEnum.contestAccountState;
            switch (contestAccountState) {
                case state.pending.value:
                    return enums.color.text;
                case state.doing.value:
                    return enums.color.cyan;
                case state.finish.value:
                    return enums.color.green;
                default:
                    return "#fff";
            }
        } catch (error) {
            console.error(error);
            return "#fff";
        }
    },
    /**
     * Lấy màu theo trạng thái contest
     *
     * @param {*} contestState
     * @returns
     */
    statusJudge0ToColor(status) {
        try {
            const statusEnum = problemEnum.statusJudge0;
            switch (status) {
                case statusEnum.Accepted:
                    return enums.color.green;
                case statusEnum.OverLimit:
                    return enums.color.orange;
                default:
                    return enums.color.red;
            }
        } catch (error) {
            console.error(error);
            return '#fff'
        }
    },
    /**
     * Lấy màu theo trạng thái contest
     *
     * @param {*} contestState
     * @returns
     */
    contestStateToColor(contestState) {
        try {
            let state = contestEnum.contestState;
            switch (contestState) {
                case state.pending.value:
                    return enums.color.text;
                case state.goingOn.value:
                    return enums.color.green;
                case state.finish.value:
                    return enums.color.red;
                default:
                    return "#fff";
            }
        } catch (error) {
            console.error(error);
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
        try {
            switch (problemAccountState) {
                case problemEnum.problemAccountState.seen.value:
                    return "far fa-eye";
                case problemEnum.problemAccountState.doing.value:
                    return "far fa-code";
                case problemEnum.problemAccountState.wrong.value:
                    return "far fa-circle-xmark";
                case problemEnum.problemAccountState.accepted.value:
                    return "far fa-circle-check";
                // case problemEnum.problemAccountState.draft.value:
                //     return "far fa-file";
                default:
                    return null;
            }
        } catch (error) {
            console.error(error);
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
        try {
            switch (problemAccountState) {
                case problemEnum.problemAccountState.doing.value:
                    return "light";
                case problemEnum.problemAccountState.wrong.value:
                    return "error";
                case problemEnum.problemAccountState.accepted.value:
                    return "success";
                default:
                    return "light";
            }
        } catch (error) {
            console.error(error);
            return 'light';
        }
    },
    /**
     * Đưa về dạng số gọn
     * 12515 -> 12.5K
     * @param {*} number
     * @returns
     */
    numberToSuffix(number) {
        try {
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
        } catch (error) {
            console.error(error);
            return number?.toString();
        }
    },
    /**
     * Enum thành text
     *
     * @param {*} value
     * @param {*} enumObj
     */
    enumToResource(value, enumObj) {
        try {
            for (const key in enumObj) {
                if (enumObj[key].value == value) {
                    return enumObj[key].label;
                }
            }
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Chuyển enum sang filter selects {value, label} => [{key, name}]
     *
     * @param {*} enumObj
     */
    enumToSelects(enumObj) {
        try {
            if (enumObj) {
                const enumArray = Object.entries(enumObj).map(([key, value]) => ({
                    key: value.value ?? value,
                    name: value.label ?? key,
                }));
                return enumArray;
            };
            return [];
        } catch (error) {
            console.error(error);
            return [];
        }
    },
    /**
     * Chuyển selected object sang enum key
     *
     * @param {*} selected
     */
    selectedToEnumKey(selected, valueIfFail) {
        try {
            if (selected) {
                const value = selected.key;
                return value;
            }
            if (valueIfFail) {
                return valueIfFail;
            }
            return null;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Chuyển enum key sang selected object
     *
     * @param {*} enumKey
     * @param {array} selects
     */
    enumKeyToSelected(enumKey, selects, indexIfNull) {
        try {
            if (selects) {
                if (enumKey != null) {
                    const values = selects.filter((item) => item.key == enumKey);

                    if (values) {
                        return values[0];
                    }
                    return null;
                } else if (
                    indexIfNull != null &&
                    indexIfNull >= 0 &&
                    indexIfNull < selects.length
                ) {
                    const value = selects[indexIfNull];
                    return value;
                }
                return null;
            }
            return null;
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    /**
     * Convert sang json string
     * @param {*} any
     * @param {*} valueIfFail
     * @returns
     */
    toJson(any, valueIfFail) {
        try {
            let result = JSON.stringify(any);
            return result;
        } catch (error) {
            console.error(error);
            return valueIfFail;
        }
    },
    /**
     * Chuyển tỷ lệ sang phần trăm
     */
    rateToPercent(value, fixed = 2, defaultValue) {
        try {
            if (value != null) {
                let cent = value * 100;
                let result = `${cent.toFixed(fixed)}%`;
                return result;
            }

            if (defaultValue) {
                return defaultValue;
            }
            return null;
        } catch (error) {
            console.error(error);
            return null;
        }
    }
};
export default convert;
