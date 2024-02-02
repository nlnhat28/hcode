import { createI18n } from "vue-i18n";
import vi from "./vi/vi.js"
/**
 * Khai báo ngôn ngữ
 */
const messages = {
    vi,
}
const locales = ["vi"];
/**
 * Tạo instance
 */
const i18n = createI18n({
    legacy: false,
    locale: locales[0],
    fallbackLocale: locales[0],
    messages,
});
export const t = i18n.global.t;

export default i18n;