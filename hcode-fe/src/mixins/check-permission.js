/**
 * Kiểm tra permission
 */
export default {
    methods: {
        /**
         * Kiểm tra xem có quyền k
         * @param {*} permission 
         */
        hasPermission(permission) {

        },
        checkPermission(permission) {
            if (true) {
                return true
            }
            else {
                this.$dl.error(this.$t('msg.cannotDoFunction'));
                return false;
            }
        },
        /**
         * Kiểm tra đăng nhập chưa và thông báo
         * @returns 
         */
        checkAuthenticated() {
            if (this.$auth.isAuthenticated()) {
                return true
            }
            else {
                let buttons = [
                    {
                        label: this.$t("auth.login"),
                        click: this.goToLogin,
                        outlined: true
                    },
                    {
                        label: this.$t("com.later"),
                    }
                ]
                this.$dl.warn(this.$t('msg.loginPlease'), buttons, this.$t('com.error'));
                return false;
            }
        },
        /**
         * Ra đăng nhập
         */
        goToLogin() {
            this.$router.push(this.$path.login);
        }
    },
};
