/**
 * Validate 1 component
 */
export default {
    data() {
        return {
            /**
             * Các refs để validate
             */
            refs: [],
            /**
             * Error message
             */
            errorMessage: null,
            /**
             * ref lỗi
             */
            refError: null,
        };
    },
    computed: {
        /**
         * Validate các ref
         *
         * Author: nlnhat (26/08/2023)
         */
        validateComputed() {
            try {
                let errorMessage = null;
                this.refs.forEach((ref) => {
                    const message = ref.checkValidate();
                    if (message) {
                        errorMessage = message;
                        this.refError = ref;
                        // Focus vào ref lỗi
                        try {
                            if (typeof ref.focusRefError == "function") {
                                ref.focusRefError();
                            } else if (typeof ref.focus == "function") {
                                ref.focus();
                            }
                        } catch {
                            console.log(error);
                        }
                    }
                });
                return errorMessage;
            } catch (error) {
                console.error(error);
                return null;
            }
        },
    },
    methods: {
        /**
         * Thực hiện validate
         *
         * Author: nlnhat (21/08/2023)
         */
        checkValidate() {
            try {
                return (this.errorMessage = this.validateComputed);
            } catch (error) {
                console.error(error);
                return null;
            }
        },
        /**
         * Focus vào ref lỗi
         */
        focusRefError() {
            if (this.refError) {
                if (typeof this.refError.focusRefError == "function") {
                    this.refError.focusRefError();
                } else if (typeof this.refError.focus == "function") {
                    this.refError.focus();
                }
            }
        },
    },
};
