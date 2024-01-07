<template>
    <div
        class="filter bg-dark"
        @keydown="onKeyDown"
    >
        <div class="filter__header">
            {{ $t('com.filter') }} {{ title.toLowerCase() }}
        </div>
        <div class="filter__body">
            <v-combobox
                v-if="isShowCompareSelects"
                :options="compareTypeOptions"
                optionLabel="name"
                v-model="compareType"
            >
            </v-combobox>
            <v-combobox
                v-if="isShowSelectsId"
                ref="combobox"
                v-model="selectedValue"
                optionLabel="name"
                :options="selects"
            >
            </v-combobox>
            <v-combobox
                v-if="isShowSelectsName"
                ref="combobox"
                v-model="selectedValue"
                optionLabel="name"
                :options="selects"
            >
            </v-combobox>
            <v-input-text
                v-if="isShowTextField"
                ref="textfield"
                v-model="filterValueKey"
                :placeholder="`${$t('com.typeFilterValue')}`"
                :validate="validateCannotNull"
                :maxLength=255
                :label="$t('com.filterValue')"
                hasClear
            >
            </v-input-text>
            <!-- <m-date-picker
                    v-if="isShowDatePicker"
                    ref="datepicker"
                    v-model:dateModel="filterValueKey"
                    :validate="validateCannotNull"
                    :label="capitalizeFirstChar(title)"
                    @emitEnterInput="onClickApply()"
                >
                </m-date-picker> -->
            <v-input-text
                v-if="isShowMonthField"
                type="number"
                ref="textfield"
                v-model="filterValueKey"
                :placeholder="$t('com.typeMonth')"
                :validate="validateMonthField"
                :maxLength=255
                :label="$t('com.month')"
            >
            </v-input-text>
            <v-input-text
                v-if="isShowYearField"
                type="number"
                ref="textfield"
                v-model="filterValueKey"
                :placeholder="$t('com.typeYear')"
                :validate="validateYearField"
                :maxLength=255
                :label="$t('com.year')"
            >
            </v-input-text>
        </div>
        <div class="filter__footer">
            <v-button
                ref="apply"
                :label="$t('com.apply')"
                @click="onClickApply"
            >
            </v-button>
            <v-button
                outlined
                :label="$t('com.removeFilter')"
                @click="onClickRemove"
            >
            </v-button>
        </div>
    </div>
</template>
<script>
// import { isNullOrEmpty, capitalizeFirstChar } from "@/js/utils/string.js";
// import { validateMonth, validateYear } from "@/js/form/validate.js";
import enums from '@/enums/enums.js'

const compareType = enums.compareType;
const filterType = enums.filterType;

export default {
    name: 'VFilter',
    props: {
        /**
         * Display name
         */
        title: {
            type: String,
        },
        /**
         * Column name
         */
        name: {
            type: String,
        },
        /**
         * Filter config contains type of filter, select list
         */
        filterConfig: {
            type: Object
        },
        /**
         * Filter item output
         */
        filterModel: {
            type: Object
        },
    },
    data() {
        return {
            /**
             * List of compare type when filter data type is text
             */
            textCompareSelects: [
                {
                    id: compareType.contain,
                    name: this.$t('com.contain')
                },
                {
                    id: compareType.equal,
                    name: this.$t('com.equal')
                },
                {
                    id: compareType.startWith,
                    name: this.$t('com.startWith')
                },
                {
                    id: compareType.endWith,
                    name: this.$t('com.endWith')
                },
                {
                    id: compareType.empty,
                    name: this.$t('com.empty')
                },
            ],
            /**
             * List of compare type when filter data type is date
             */
            dateCompareSelects: [
                {
                    id: compareType.equal,
                    name: this.$t('com.equal')
                },
                {
                    id: compareType.less,
                    name: this.$t('com.before')
                },
                {
                    id: compareType.more,
                    name: this.$t('com.after')
                },
                {
                    id: compareType.month,
                    name: this.$t('com.monthEqual')
                },
                {
                    id: compareType.year,
                    name: this.$t('com.yearEqual')
                },
                {
                    id: compareType.empty,
                    name: this.$t('com.empty')
                }
            ],
            /**
             * List of compare type when filter data type is date
             */
            selectOneCompareSelects: [
                {
                    id: compareType.contain,
                    name: this.$t('com.equal')
                }
            ],
            /**
             * Compare type of filter
             */
            compareType: {
                id: null,
                name: null,
            },
            /**
             * Filter value contain condition filter (key, name)
             */
            filterValues: [{ key: '', name: '' }],
            /**
             * Select list in case filter type is choice
             */
            selects: [],
            /**
             * Key of filter value
             */
            filterValueKey: '',
            /**
             * Name of filter value
             */
            filterValueName: '',
            /**
             * 
             */
            selectedValue: {},
            /**
             * Number field config
             */
            numberField: {
                label: null,
                placeholder: null,
                validate: null,
            }
        }
    },
    created() {
        const compares = this.compareTypeOptions;
        this.compareType = { ...compares[0] };
        this.mapSelects();
    },
    mounted() {

    },
    expose: ['focus', 'focusOnApply'],
    computed: {
        /**
         * Load compare type to combobox
         * 
         * Author: nlnhat (25/07/2023)
         */
        compareTypeOptions() {
            switch (this.filterConfig.filterType) {
                case filterType.text:
                    return this.textCompareSelects;
                case filterType.date:
                    return this.dateCompareSelects;
                case filterType.selectOne:
                    return this.selectOneCompareSelects;
                default:
                    return this.dateCompareSelects;;
            }
        },
        /**
         * Show combobox select id or not 
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowSelectsId() {
            return (this.filterConfig.filterType == filterType.selectKey
                && this.compareType.id != compareType.empty)
        },
        /**
         * Show combobox select name or not
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowSelectsName() {
            return (this.filterConfig.filterType == filterType.selectName
                && this.compareType.id != compareType.empty)
        },
        /**
         * Show textfield or not
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowTextField() {
            return (this.filterConfig.filterType == filterType.text
                && this.compareType.id != compareType.empty)
        },
        /**
         * Show datepicker or not 
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowDatePicker() {
            return (this.filterConfig.filterType == filterType.date
                && this.compareType.id != compareType.empty
                && this.compareType.id != compareType.month
                && this.compareType.id != compareType.year)
        },
        /**
         * Show month field or not
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowMonthField() {
            return (this.compareType.id == compareType.month)
        },
        /**
         * Show year field or not
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowYearField() {
            return (this.compareType.id == compareType.year)
        },
        /**
         * Show or not compare types select
         * 
         * Author: nlnhat (25/07/2023)
         */
        isShowCompareSelects() {
            return (this.filterConfig.filterType == filterType.text
                || this.filterConfig.filterType == filterType.date)
        },
        /**
         * Return filter data from compare type and filter value
         *
         * Author: nlnhat (25/07/2023)
         */
        filterDataComputed() {
            // Key là giá trị lọc
            if (this.filterConfig.filterType == filterType.selectName)
                this.filterValueKey = this.filterValueName;

            this.filterValues[0].key = this.filterValueKey;
            this.filterValues[0].name = this.filterValueName;


            return {
                title: this.title,
                column: this.name,
                compareType: this.compareType.id,
                compareName: this.compareType.name.toLowerCase(),
                filterType: this.filterConfig.filterType,
                values: (this.compareType.id == compareType.empty) ? [] : [...this.filterValues],
            }
        }
    },
    watch: {
        /**
         * What compare type
         */
        compareType: {
            handler() {
                this.$nextTick(() => {
                    this.focus();
                })
            },
            deep: true
        },
        /**
         * What select from config to update select data
         */
        "filterConfig.selects": function () {
            this.mapSelects();
        },
        /**
         * Selected value để gán filter 
         */
        selectedValue: {
            handler() {
                if (this.selectedValue) {
                    this.filterValueKey = this.selectedValue.key;
                    this.filterValueName = this.selectedValue.name;
                }
            },
            deep: true
        }
    },
    methods: {
        /**
         * Handle when click apply filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        onClickApply() {
            const errorMessage = this.checkValidate();
            if (errorMessage == null) {
                const data = this.filterDataComputed;
                this.$emit('emitUpdateFilterModel', data);
                this.hideFilter();
            }
        },
        /**
         * Handle when click remove filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        onClickRemove() {
            if (this.filterModel != null)
                this.$emit('emitUpdateFilterModel', null);
            this.hideFilter();
        },
        /**
         * Hide this filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        hideFilter() {
            this.$emit('emitHideFilter');
        },
        /**
         * Focus input
         * 
         * Author: nlnhat (25/07/2023)
         */
        focus() {
            if (this.$refs.textfield != null) {
                this.$refs.textfield.focus();
                this.$refs.textfield.select();
            }
            else if (this.$refs.datepicker != null) {
                this.$refs.datepicker.focus();
                this.$refs.datepicker.select();
            }
            // else if (this.$refs.combobox != null) {
            //     this.$refs.combobox.focus();
            // }
            else this.focusOnApply();
        },
        /**
         * Clear input
         * 
         * Author: nlnhat (25/07/2023)
         */
        clearInput() {
            this.filterValues = [];
        },
        /**
         * Validate filter value cannot empty
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateCannotNull(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (this.$cf.isEmptyString(value)) {
                return `${label} ${this.$t('msg.cannotNull')}`
            }
            return null;
        },
        /**
         * Validate month filter
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateMonthField(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (isNullOrEmpty(value)) {
                return `${label} ${$t('msg.cannotNull')}`
            }
            return this.validateMonth(label, value);
        },
        /**
         * Validate year filter
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateYearField(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (isNullOrEmpty(value)) {
                return `${label} ${$t('msg.cannotNull')}`
            }
            return this.validateYear(label, value);
        },
        /**
         * Check validate filter value
         * 
         * Author: nlnhat (25/07/2023)
         */
        checkValidate() {
            let errorMessage = null;

            // Validate date
            if (this.isShowDatePicker && this.$refs.datepicker != null) {
                errorMessage = this.$refs.datepicker.checkValidate();
                if (errorMessage != null) {
                    this.$refs.datepicker.focus()
                    return errorMessage;
                }
                return null
            }

            // Validate textfield
            if (this.isShowTextField || this.isShowMonthField || this.isShowYearField
                && this.$refs.textfield != null) {
                errorMessage = this.$refs.textfield.checkValidate();
                if (errorMessage != null) {
                    this.$refs.textfield.focus()
                    return errorMessage;
                }
                return null;
            }

            return null;
        },
        /**
         * Map select from props to data
         * 
         * Author: nlnhat (25/07/2023)
         */
        mapSelects() {
            if (this.filterConfig.selects) {
                this.selects = this.filterConfig.selects;
                this.selectedValue = this.selects[0];
            }
        },
        /**
         * Focus on button apply
         * 
         * Author: nlnhat (25/07/2023)
         */
        focusOnApply() {
            this.$nextTick(() => {
                if (this.$refs.apply) {
                    // this.$refs.apply.focus();
                }
            })
        },
        /**
         * Handle short key on filter
         * 
         * Author: nlnhat (29/07/2023)
         * @param {*} event Keydown event on filter
         */
        onKeyDown(event) {
            // Enter: Apply
            if (event.keyCode == this.$enums.keyCode.enter) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickApply();
            }
        },
        /**
         * Imported methods
         */
        // capitalizeFirstChar,
        // validateMonth,
        // validateYear,
    }
}
</script>
<style scoped>
@import './filter.css';

.textfield {
    line-height: 35px;
}
</style>