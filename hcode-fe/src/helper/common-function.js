import _, { values } from "lodash";
import emitter from "tiny-emitter/instance";
import enums from "@/enums/enums.js";
import { t } from "@/i18n/i18n.js";
import { v4 as uuidv4 } from "uuid";

const commonFuction = {
    /**
     * Check giá trị null hoặc undefined
     */
    isNullValue(value) {
        // try catch đề phòng reference null object thì return true luôn
        try {
            let result = value == null;
            return result;
        } catch (error) {
            console.error(error);
            return true;
        }
    },
    /**
     * Check string null hoặc undefined hoặc ""
     */
    isEmptyString(string) {
        if (!this.isNullValue(string)) {
            return string === "";
        }
        return true;
    },
    /**
     * Sao chép vào clipboard
     */
    copyToClipboard(value) {
        navigator.clipboard.writeText(value);
        return true;
    },
    /**
     * Sleep
     *
     * @param {number} second
     */
    async sleep(second) {
        if (!second) {
            second = 5;
        }
        return new Promise((resolve) => setTimeout(resolve, second * 1000));
    },
    /**
     * Success response or not
     *
     * @param {number} response
     */
    isSuccess(response) {
        return response && response.Success;
    },
    /**
     * Clone deep
     *
     * @param {*} obj Object clone
     */
    cloneDeep(obj) {
        return _.cloneDeep(obj);
    },
    /**
     * Tạo document title
     */
    documentTitle(title) {
        if (this.isEmptyString(title)) {
            return window.config.app.title;
        }
        return `${title} - ${window.config.app.title}`;
    },
    /**
     * Tạo mảng n phần tử rỗng
     */
    genArrayEmpty(n) {
        let amount = 1;
        if (n && Number.isInteger(n) && n > 0) {
            amount = n;
        }

        const arr = Array.from({ length: amount }, () => ({}));
        return arr;
    },
    /**
     * Check 1 mảng không có phần tử
     * @param {*} array
     * @returns
     */
    isEmptyArray(array) {
        if (array && Array.isArray(array)) {
            return array.length == 0;
        }
        return true;
    },
    /**
     * Check object không có key nào
     */
    isEmptyObject(obj) {
        if (obj) {
            return Object.keys(obj).length == 0;
        }
        return true;
    },
    /**
     * uuid
     * @returns
     */
    uuid: {
        new() {
            const newId = uuidv4();
            return newId;
        },
        isEmpty(id) {
            return id == "00000000-0000-0000-0000-000000000000";
        },
        empty: "00000000-0000-0000-0000-000000000000",
    },
    /**
     * Capitalize first char
     *
     * Author: nlnhat (28/07/2023)
     *
     * @param {string} text String to format
     * @returns {string} String format: test vƯƯalue => Test value
     */
    upperFirstChar(text) {
        if (text && typeof text == "string") {
            return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
        }
        return text;
    },
    /**
     * Bỏ những item null hoặc bằng ''
     * @param {*} array
     */
    removeNullOrEmpty(array) {
        if (array) {
            const result = array.filter((item) => item != null && item !== "");
            return result;
        }
        return array;
    },
    /**
     * Lấy tên key của enum
     *
     * @param {*} value Giá trị enum
     * @param {*} enumObj Enum object
     * @param {*} defaultValue Giá trị default nếu không có
     */
    getKeyNameEnum(value, enumObj, defaultValue) {
        if (enumObj) {
            for (const key in enumObj) {
                if (enumObj[key] == value) {
                    return key;
                } else if (
                    enumObj[key] != null &&
                    enumObj[key].value == value
                ) {
                    return key;
                }
            }
        }
        return defaultValue;
    },
    /**
     * Lấy số lượng
     *
     * @param {array} array
     * @param {boolean} nullToZero Null thì return 0
     */
    getCount(array, nullToZero = true) {
        if (array) {
            return array.length;
        }

        if (nullToZero) {
            return 0;
        }
        return null;
    },
    /**
     * Tính tổng theo trên trường
     *
     * @param {array} array
     * @param {string} fieldTotal Trường lấy tổng
     * @param {number} from Giá trị bắt đầu
     * @param {boolean} nullToZero Null thì return 0
     */
    calcTotal(array, fieldTotal, from = 0, nullToZero) {
        if (array) {
            const sum = array.reduce((accumulator, currentObject) => {
                return accumulator + (currentObject[fieldTotal] ?? 0);
            }, from);

            return sum;
        }

        if (nullToZero) {
            return 0;
        }
        return null;
    },
    /**
     * Tính số phút giữa 2 khoảng thời gian
     *
     * @param {*} startTime
     * @param {*} endTime
     */
    getMinuteTwoDateTime(startTime, endTime, nullToZero = false) {
        if (startTime && endTime) {
            let milliseconds = endTime - startTime;
            let minutes = milliseconds / (1000 * 60);

            return minutes;
        }

        if (nullToZero) {
            return 0;
        }
        return null;
    },
    /**
     * Checkvalidate
     * @param {*} ref
     */
    checkValidateRef(ref) {
        if (ref && typeof ref.checkValidate == "function") {
            let msg = ref.checkValidate();
            return msg;
        }
        return null;
    },
    /**
     * Tạo route abc/xyz
     * @returns
     */
    combineRoute() {
        if (arguments) {
            let args = Object.values(arguments);
            let path = args
                .filter((a) => a != null && a !== "")
                .map((a) => {
                    return a.toString().replace(/\/+$/, "");
                });

            let route = path?.join("/");
            return route;
        }
        return "";
    },
    /**
     * Gán select vào filter column
     * @param {*} selects
     * @param {*} columns
     * @param {*} name
     */
    addSelectsToColumn(selects, columns, name) {
        if (columns && Array.isArray(columns)) {
            columns.forEach((c) => {
                if (c.name == name && c.filterConfig) {
                    c.filterConfig.selects = selects;
                }
            });
        }
    },
    /**
     * Đếm ngược thời gian
     *
     * @param {*} fixedTime Khoảng cố định (giây)
     * @param {*} fixedTimeUnit Đơn vị thời gian cố định
     * @param {*} toTime Thời điểm kết thúc
     * @param {*} fromTime Thời điểm bắt đầu
     * @param {*} showValues Những giá trị hiển thị
     * @param {*} format long/short
     * @param {*} isFull Hiện các giá trị ngay cả khi bằng 0
     */
    countDown(
        fixedTime,
        fromTime,
        toTime,
        fixedTimeUnit,
        showValues,
        format,
        isFull
    ) {
        try {
            if (fixedTime == null && toTime == null) {
                throw new Error("Must have fixedTime or toTime");
            }
            const shows = Array.isArray(showValues) && showValues?.length > 0
                ? showValues
                : ["d", "h", "m", "s"];

            let duration = 0; // khoảng thời gian để đếm (millisecond)

            let d = 0;
            let h = 0;
            let m = 0;
            let s = 0;

            // Nếu mà có khoảng thời gian
            if (fixedTime != null) {
                const unit = enums.timeUnit;
                switch (fixedTimeUnit) {
                    case unit.millisecond:
                        duration = fixedTime;
                        break;
                    case unit.second:
                        duration = fixedTime * 1000;
                        break;
                    case unit.minute:
                        duration = fixedTime * 60_000;
                        break;
                    case unit.hour:
                        duration = fixedTime * 3_600_000;
                        break;
                    default: // giây
                        duration = fixedTime * 1000;
                        break;
                }
            }
            // Dùng thời điểm khác
            else {
                let from = fromTime != null ? new Date(fromTime) : new Date();
                let to = new Date(toTime);
                duration = to - from;
            }

            duration = Math.max(duration, 0);

            // Các giá trị thô
            d = Math.max(Math.floor(duration / 86_400_000), 0);
            h = Math.max(Math.floor((duration % 86_400_000) / 3_600_000), 0);
            m = Math.max(Math.floor((duration % 3_600_000) / 60_000), 0);
            s = Math.max(Math.floor((duration % 60_000) / 1000), 0);

            // Xử lý theo các giá trị show

            // Nếu hiện d mà không hiện h mà vẫn còn lẻ thì +1 vào d, vd: 1 ngày 2 giờ 3 phút 4 giây => 2 ngày
            // Nếu không hiện d thì cộng dồn vào h, vd: 1 ngày 2 giờ => 26 giờ
            // Tương tự cho h, m, s

            // d
            if (shows.includes("d") && !shows.includes("h") && h + m + s > 0) {
                d++;
            }

            // h
            if (shows.includes("h") && !shows.includes("m") && m + s > 0) {
                h++;
            }
            if (shows.includes("h") && !shows.includes("d") && d > 0) {
                h += d * 24;
            }

            // m
            if (shows.includes("m") && !shows.includes("s") && s > 0) {
                m++;
            }
            if (shows.includes("m") && !shows.includes("h") && d + h > 0) {
                shows = shows.filter((i) => i != "d"); // Bỏ cả d đi
                m += d * 24 + h * 60;
            }

            // s
            if (shows.includes("s") && !shows.includes("m") && d + h + m > 0) {
                shows = shows.filter((i) => i != "d" && i != "h"); // Bỏ cả d, h đi
                m += d * 24 + h * 60 + m * 60;
            }

            let texts = [];
            let seperator = format == "long" ? " " : ":";
            if (shows.includes("d")) {
                if (d > 0 || (d == 0 && isFull)) {
                    texts.push(`${String(d).padStart(2, "0")} ${t("com.day")}`);
                }
            }
            if (shows.includes("h")) {
                if (h > 0 || (h == 0 && (isFull || d > 0))) {
                    texts.push(
                        `${String(h).padStart(2, "0")} ${t("com.hour")}`
                    );
                }
            }
            if (shows.includes("m")) {
                if (m > 0 || (m == 0 && (isFull || d + h > 0))) {
                    texts.push(
                        `${String(m).padStart(2, "0")} ${t("com.minute")}`
                    );
                }
            }
            if (shows.includes("s")) {
                if (s > 0 || (s == 0 && (isFull || d + h + m > 0))) {
                    texts.push(
                        `${String(s).padStart(2, "0")} ${t("com.second")}`
                    );
                }
            }
            let text = texts.join(seperator);
            return {
                day: d,
                hour: h,
                minute: m,
                second: s,
                text: text,
                duration,
            };
           
        } catch (error) {
            console.error(error);
            return null;
        }
    },
};
export default commonFuction;
