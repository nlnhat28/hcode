import enums from "@/enums/enums.js";

/**
 * Làm hiệu ứng loading
 */
export default {
    methods: {
        async handleError(response, action) {
            if (response && !response.Success) {
                
                let userMsg = response.UserMsg;
                let errorItems = [];
                let refFocus = null;
                let callback = action;

                const Data = response.Data;
                const ErrorKey = response.ErrorKey;

                if (Data) {
                    switch (ErrorKey) {
                        // Nếu error key là form item thì focus vào form item bị lỗi
                        case enums.errorKey.formItem:
                            if (!Array.isArray(Data)) {
                                errorItems.push(Data);
                            } else {
                                errorItems = Data;
                            }

                            errorItems.forEach((item) => {
                                const ref = item.RefName;
                                const msg = item.ErrorMessage;
                                this.setErrorMessage(ref, msg);

                                if (refFocus == null) {
                                    refFocus = ref;
                                }
                                if (this.$cf.isEmptyString(userMsg)) {
                                    userMsg = msg;
                                }
                                callback = () => {
                                    this.focusErrorItem(refFocus);
                                };
                            });
                            break;
                        default:
                            break;
                    }

                    if (this.$cf.isEmptyString(userMsg)) {
                        userMsg = this.$t("msg.clientError");
                    }

                    this.$dl.error(userMsg, callback);
                }
            }
        },
        /**
         * Gán error message cho item (ref) lỗi
         *
         * @param {string} refName
         * @param {string} errorMessage
         */
        setErrorMessage(refName, errorMessage) {
            if (this.$refs[refName]) {
                this.$refs[refName].errorMessage = errorMessage;
            }
        },
        /**
         * Focus item (ref) lỗi
         *
         * @param {string} refName
         */
        focusErrorItem(refName) {
            if (this.$refs[refName]) {
                this.$refs[refName].focus();
            }
        },
        /**
         * Response success
         */
        isSuccess(res) {
            return res && res.Success;
        }
    },
};
