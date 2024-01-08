<template>
    <div class="mask mask--darker">
        <div class="loader-dialog">
            <div class="loader-dialog__icon">
                <component :is="iconComponent"></component>
            </div>
            <div class="loader-dialog__text">
                {{ text }}
            </div>
        </div>
    </div>
</template>
<script>
import { t } from "@/i18n/i18n.js"
export default {
    name: "VMaskDialog",
    props: {
        /**
         * Text hiển thị
         */
        text: {
            type: [String, Number],
            default: t("msg.pleaseWait"),
        },
        /**
         * Kiểu icon
         */
        type: {
            type: String,
            default: 'loader',
            validator: (value) => {
                return [
                    'loader',
                    'spinner',
                    'lock'
                ].includes(
                    value
                );
            },
        }
    },
    computed: {
        /**
         * Icon component
         */
        iconComponent() {
            switch (this.type) {
                case 'spinner':
                    return 'v-spinner'
                case 'lock':
                    return 'v-loader-lock'
                default:
                case 'loader':
                    return 'v-loader'
            }
        }
    }

}
</script>
<style scoped>
@import './mask.css';
@import './mask-dialog.css';
</style>