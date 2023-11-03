<script>
import { debounce } from 'lodash';

export default {
    name: 'VBaseInput',
    props: {
        /**
         * Focused or not
         */
        isFocused: {
            type: Boolean,
            default: false,
        },
        /**
         * Readonly or not
         */
        isReadOnly: {
            type: Boolean,
            default: false,
        },
        /**
         * Disabled or not
         */
        isDisabled: {
            type: Boolean,
            default: false,
        },
        /**
         * Model binding
         */
        modelValue: {
            type: [String, Number]
        },
        /**
         * Tên nhãn
         */
        label: {
            type: [String, Number],
        },
        /**
         * Độ dài tối đa
         */
        maxLength: {
            type: Number,
        },
        /**
         * Không được để trống
         */
        isRequired: {
            type: Boolean,
            default: false,
        },
        /**
         * Dùng placeholder
         */
        applyPlaceholder: {
            type: Boolean,
            default: true,
        },
        /**
         * Placeholder
         */
        placeholder: {
            type: String,
        },
        /**
         * Function to validate
         */
        validate: {
            type: Function,
        },
        /**
         * Function to format
         */
        format: {
            type: Function,
        },
        /**
         * Function warning
         */
        warn: {
            type: Function,
        },
        /**
         * Icon trái
         */
        icon: {
            type: [String]
        },
        /**
         * Hành động đi kèm 
            {
                icon,
                method,
                tooltip,
                hasLoading,
            }
         */
        action: {
            type: Object,
            default: null
        },
        /**
         * Căn text
         */
        textAlign: {
            type: String,
            validator: (value) => {
                return [
                    'left',
                    'center',
                    'right',
                ].includes(value)
            }
        },
        /**
         * Set lại con trỏ select về vị trí trước khi text thay đổi
         */
        canSetSelection: {
            type: Boolean,
            default: false,
        },
        /**
         * Debounce khi update value
         */
        hasDebounce: {
            type: Boolean,
            default: false,
        },
        /**
         * Tooltip input
         */
        tooltip: {
            type: [String, Number],
            default: null,
        },
        /**
         * Show icon mà k cần hover
         */
        isShowActionIcon: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * Giá trị text fields
             */
            innerValue: this.modelValue ?? null,
            /**
             * Thông báo lỗi nếu có
             */
            errorMessage: null,
            /**
             * Cảnh báo nếu có
             */
            warnMessage: null,
            /**
             * Trạng thái thực hiện hành động
             */
            isLoading: {
                type: Boolean,
                default: false
            },
            /**
             * Show icon after success action
             */
            isSuccess: false,
            /**
             * Show icon after error action
             */
            isError: false,
            /**
             * Action 
             */
            innerAction: this.action,
        }
    },
    created() {
        this.innerValue = this.formatComputed;
        this.isLoading = false;
        this.isSuccess = false;

        if (this.innerAction) {
            if (!this.innerAction.hasLoading) {
                this.innerAction.hasLoading = false;
            }
        }
    },
    mounted() {
        if (this.isFocused) this.focus();
    },
    beforeUnmount() {
        this.blur();
    },
    watch: {
        modelValue() {
            this.innerValue = this.modelValue
        },
        innerValue() {
            this.onChangeValue();
        },
    },
    computed: {
        /**
         * Validate value
         * 
         * Author: nlnhat (01/07/2023)
         * @return Error message if invalid, null if valid
         */
        validateComputed() {
            try {
                // Validate required
                if (this.isRequired && this.$cf.isNullString(this.innerValue))
                    return `${this.label} ${this.$t('msg.cannotNull')}`;

                // Validate length
                if (this.maxLength && this.innerValue?.length > this.maxLength)
                    return `${this.label} ${this.$t('msg.mustLessEqual', { length: this.maxLength })}`;

                // Custom validate
                if (this.validate) {
                    return this.validate(this.label, this.innerValue);
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Format value
         * 
         * Author: nlnhat (01/07/2023)
         * @return Formated value
         */
        formatComputed() {
            try {
                if (this.format)
                    return this.format(this.innerValue)
            } catch (error) {
                console.error(error);
            }
            return this.innerValue;
        },
        /**
         * Warn value
         * 
         * Author: nlnhat (01/07/2023)
         * @return Warn message if invalid, null if valid
         */
        warnComputed() {
            try {
                // Custom warn
                if (this.warn) {
                    return this.warn(this.label, this.innerValue);
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Placeholder
         */
        placeholderComputed() {
            if (this.applyPlaceholder) {
                if (this.$cf.isNullString(this.placeholder)) {
                    return this.label;
                };
                return this.placeholder;
            };
            return null;
        },
        /**
         * Tooltip
         */
        tooltipComputed() {
            if (this.errorMessage) {
                return this.errorMessage
            }
            return this.warnMessage;
        }
    },
    methods: {
        /**
         * Focus on input
         * 
         * Author: nlnhat (01/07/2023)
         */
        focus() {
            if (this.$refs['refEl']) {
                this.$refs['refEl'].focus();
            }
        },
        /**
         * Blur input
         * 
         * Author: nlnhat (01/07/2023)
         */
        blur() {
            if (this.$refs.input)
                this.$refs.input.blur();
        },
        /**
         * Select all text input
         * 
         * Author: nlnhat (30/07/2023)
         */
        select() {
            if (this.$refs.input)
                this.$refs.input.select();
        },
        /**
         * Handle input value change
         * 
         * Author: nlnhat (01/07/2023)
         */
        onChangeValue() {
            try {
                // Set lại con trỏ select về vị trí trước khi text thay đổi
                if (this.canSetSelection && this.$refs.input) {
                    var position = this.$refs.input.selectionStart;
                    this.innerValue = this.formatComputed;
                    this.$nextTick(() => {
                        this.$refs.input.setSelectionRange(position, position);
                    });
                }
                else this.innerValue = this.formatComputed;

                if (!this.isReadOnly) {
                    if (this.hasDebounce)
                        this.debounceUpdate();
                    else
                        this.$emit('update:modelValue', this.innerValue);
                }

                this.checkValidate();
                this.checkWarn();

            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Check validate value
         * 
         * Author: nlnhat (02/07/2023)
         */
        checkValidate() {
            try {
                return this.errorMessage = this.validateComputed;
            } catch (error) {
                console.error(error);
            }
            return null
        },
        /**
         * Check validate value
         * 
         * Author: nlnhat (02/07/2023)
         */
        checkWarn() {
            try {
                return this.warnMessage = this.warnComputed;
            } catch (error) {
                console.error(error);
            }
            return null
        },
        /**
         * Make loading effect
         * 
         * Author: nlnhat (05/07/2023)
         * @param {function} func Function executes in loading process
         */
        async makeLoadingEffect(func) {
            try {
                this.isLoading = true;
                if (await func()) {
                    this.isSuccess = true;
                    await new Promise((resolve) => setTimeout(resolve, 1500));
                    this.isSuccess = false;
                }
                else {
                    this.isError = true;
                    await new Promise((resolve) => setTimeout(resolve, 1500));
                    this.isError = false;
                }
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
        /**
         * Handle when click action
         * 
         * Author: nlnhat (23/07/2023)
         */
        onAction() {
            if (this.innerAction.hasLoading) {
                this.makeLoadingEffect(this.innerAction.method)
            }
            else {
                this.innerAction.method();
            }
        },
        /**
         * Set error message
         * 
         * Author: nlnhat (23/07/2023)
         * @param {*} message Message to set for error message
         */
        setErrorMessage(message) {
            this.errorMessage = message;
        },
        /**
         * Clear error message
         * 
         * Author: nlnhat (23/07/2023)
         */
        clearErrorMessage() {
            this.errorMessage = null;
        },
        /**
         * Handle key down on input
         * 
         * Author: nlnhat (29/07/2023)
         */
        onKeyDown(event) {
            switch (event.keyCode) {
                case this.$enums.keyCode.enter:
                    event.preventDefault();
                    event.stopPropagation();
                    this.$emit('emitEnter');
                    break;
                case this.$enums.keyCode.space:
                    event.stopPropagation();
                    this.$emit('emitSpace');
                    break;
                case this.$enums.keyCode.tab:
                    this.$emit('emitTab');
                    break;
                default:
                    break;
            }
        },
        /**
         * Debounce update
         * 
         * Author: nlnhat (12/07/2023)
         */
        debounceUpdate: debounce(function () {
            this.$emit('update:modelValue', this.innerValue)
        }, 1000),
    }
}
</script>