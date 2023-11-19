<template>
    <div class="filter">
        <!-- <div class="filter__logic">
            <m-compare-select
                :compares="logicTypeSelects"
                :isActiveFilter="filterModel != null"
                v-model:compareModel="logicType"
                @emitSelected="handleSelectedLogic()"
                @emitRemoveFilter=onClickRemove()
                ref="logic"
            >
            </m-compare-select>
        </div>
        <div
            class="filter__compare"
            v-if="isShowCompareSelects"
        >
            <m-compare-select
                v-model:compareModel="compareType"
                :compares="compareTypeSelects"
                :isActiveFilter="filterModel != null"
                @emitSelected="handleSelectedCompare()"
                @emitRemoveFilter=onClickRemove()
                ref="compare"
            >
            </m-compare-select>
        </div> -->
        <div class="filter__value">
            <!-- <m-combobox
                v-if="isShowSelectsId"
                v-model:id="filterValueKey"
                v-model:name="filterValueName"
                :theSelects="selects"
                :isReadOnly="true"
                :canSearch="false"
                :canUnselect="true"
                @emitEnter="handleUpdateFilterModel()"
                ref="combobox"
            >
            </m-combobox>
            <m-combobox
                v-if="isShowSelectsName"
                v-model:id="filterValueKey"
                v-model:name="filterValueName"
                :theSelects="selects"
                :isReadOnly="false"
                :canSearch="true"
                :canUnselect="true"
                @emitEnter="handleUpdateFilterModel()"
                ref="combobox"
            >
            </m-combobox> -->
            <v-input-text
                v-if="isShowTextField"
                ref="textfield"
                v-model="filterValueKey"
                :maxLength=255
                :isReadOnly="isReadOnlyInput"
                @emitBlur="clearErrorMessage()"
                @emitEnter="handleUpdateFilterModel()"
            >
            </v-input-text>
            <v-input-text
                v-if="isShowNumberField"
                ref="textfield"
                v-model="filterValueKey"
                :format="formatDecimalInput"
                :maxLength=255
                :label="this.$resources['vn'].filterValue"
                :isReadOnly="isReadOnlyInput"
                :canSetSelection="true"
                @emitBlur="clearErrorMessage()"
                @emitEnter="handleUpdateFilterModel()"
            >
            </v-input-text>
            <m-date-picker
                v-if="isShowDatePicker"
                ref="datepicker"
                v-model:dateModel="filterValueKey"
                :label="capitalizeFirstChar(title)"
                @emitEnterInput="onClickApply()"
            >
            </m-date-picker>
            <!-- <v-input-text
                v-if="isShowMonthField"
                type="number"
                ref="textfield"
                v-model="filterValueKey"
                :validate="validateMonthField"
                :maxLength=255
            >
            </v-input-text>
            <v-input-text
                v-if="isShowYearField"
                type="number"
                ref="textfield"
                v-model="filterValueKey"
                :validate="validateYearField"
                :maxLength=255
            >
            </v-input-text> -->
        </div>
    </div>
</template>
<script>
// import { isNullOrEmpty, capitalizeFirstChar } from "@/js/utils/string.js";
// import { validateMonth, validateYear } from "@/js/form/validate.js";
// import { formatDecimalInput } from "@/js/utils/format.js";
// import { reformatDecimal } from "@/js/utils/clean-format.js";
// import resources from '@/constants/resources.js'
import enums from '@/enums/enums.js'
import { debounce } from 'lodash';

const logicType = enums.logicType;
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
         * Filter model output
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
                // {
                //     id: compareType.contain,
                //     name: '*',
                //     note: resources['vn'].contain,
                // },
                // {
                //     id: compareType.notContain,
                //     name: '!',
                //     note: resources['vn'].notContain,
                // },
                // {
                //     id: compareType.equal,
                //     name: '=',
                //     note: resources['vn'].equal,
                // },
                // {
                //     id: compareType.startWith,
                //     name: '+',
                //     note: resources['vn'].startWith,
                // },
                // {
                //     id: compareType.endWith,
                //     name: '-',
                //     note: resources['vn'].endWith,
                // },
            ],
            /**
             * List of compare type when filter data type is date
             */
            dateCompareSelects: [
                // {
                //     id: compareType.equal,
                //     name: resources['vn'].equal
                // },
                // {
                //     id: compareType.less,
                //     name: resources['vn'].before
                // },
                // {
                //     id: compareType.more,
                //     name: resources['vn'].after
                // },
                // {
                //     id: compareType.month,
                //     name: resources['vn'].monthEqual
                // },
                // {
                //     id: compareType.year,
                //     name: resources['vn'].yearEqual
                // },
                // {
                //     id: compareType.empty,
                //     name: resources['vn'].empty
                // }
            ],
            /**
             * List of compare type when filter data type is number
             */
            numberCompareSelects: [
                // {
                //     id: compareType.equal,
                //     name: '=',
                //     note: resources['vn'].equal,
                // },
                // {
                //     id: compareType.notEqual,
                //     name: '<>',
                //     note: resources['vn'].notEqual,
                // },
                // {
                //     id: compareType.less,
                //     name: '<',
                //     note: resources['vn'].less,
                // },
                // {
                //     id: compareType.more,
                //     name: '>',
                //     note: resources['vn'].more,
                // },
                // {
                //     id: compareType.lessEqual,
                //     name: '<=',
                //     note: resources['vn'].lessEqual,
                // },
                // {
                //     id: compareType.moreEqual,
                //     name: '>=',
                //     note: resources['vn'].moreEqual,
                // },
            ],
            /**
             * Logic type select
             */
            logicTypeSelects: [
                // {
                //     id: logicType.and,
                //     name: '&',
                //     note: resources['vn'].and,
                // },
                // {
                //     id: logicType.or,
                //     name: '||',
                //     note: resources['vn'].or,
                // },
            ],
            /**
             * List of compare type when filter data type is date
             */
            selectCompareSelects: [
                // {
                //     id: compareType.equal,
                //     name: '=',
                //     note: resources['vn'].equal
                // }
            ],
            /**
             * Logic type of filter
             */
            logicType: {
                id: null,
                name: null,
                note: null,
            },
            /**
             * Compare type of filter
             */
            compareType: {
                id: null,
                name: null,
                note: null,
            },
            /**
             * Filter value contain condition filter (key, name)
             */
            filterValue: { key: '', name: '' },
            /**
             * Select list in case filter type is choice
             */
            selects: [],
            /**
             * Key of filter value
             */
            filterValueKey: null,
            /**
             * Name of filter value
             */
            filterValueName: null,
            /**
             * Number field config
             */
            numberField: {
                label: null,
                placeholder: null,
                validate: null,
            },
            /**
             * Trạng thái filter
             */
            appliedFilter: false,
        }
    },
    created() {
        this.logicType = { ...this.logicTypeSelects[0] };

        const compares = this.compareTypeSelects;
        this.compareType = { ...compares[0] };

        this.mapSelects();
        if (this.filterModel) {
            if (this.filterConfig.filterType != filterType.selectName)
                this.filterValueKey = this.filterModel.value.key;
            this.filterValueName = this.filterModel.value.name;
        }
    },
    expose: ['focus', 'focusOnApply'],
    watch: {
        /**
         * What select from config to update select data
         */
        "filterConfig.selects": function () {
            this.mapSelects();
        },
        /**
         * Handle when filter value changes
         */
        // filterValueKey() {
        //     this.onChangeFilterValue();
        // },
    },
    computed: {
        /**
         * Load compare type to combobox
         * 
         * Author: nlnhat (25/08/2023)
         */
        compareTypeSelects() {
            switch (this.filterConfig.filterType) {
                case filterType.text:
                    return this.textCompareSelects;
                case filterType.date:
                    return this.dateCompareSelects;
                case filterType.number:
                    return this.numberCompareSelects;
                case filterType.selectId:
                    return this.selectCompareSelects;
                case filterType.selectName:
                    return this.selectCompareSelects;
                default:
                    return this.textCompareSelects;
            }
        },
        /**
         * Show combobox select id or not 
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowSelectsId() {
            return (this.filterConfig.filterType == filterType.selectId)
        },
        /**
         * Show combobox select name or not
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowSelectsName() {
            return (this.filterConfig.filterType == filterType.selectName)
        },
        /**
         * Show textfield or not
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowTextField() {
            return (this.filterConfig.filterType == filterType.text)
        },
        /**
         * Show number field or not
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowNumberField() {
            return (this.filterConfig.filterType == filterType.number)
        },
        /**
         * Show datepicker or not 
         * 
         * Author: nlnhat (25/08/2023)
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
         * Author: nlnhat (25/08/2023)
         */
        isShowMonthField() {
            return (this.compareType.id == compareType.month)
        },
        /**
         * Show year field or not
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowYearField() {
            return (this.compareType.id == compareType.year)
        },
        /**
         * Show or not compare types select
         * 
         * Author: nlnhat (25/08/2023)
         */
        isShowCompareSelects() {
            return (this.filterConfig.filterType == filterType.text
                || this.filterConfig.filterType == filterType.date
                || this.filterConfig.filterType == filterType.number)
        },
        /**
         * Return filter data from compare type and filter value
         *
         * Author: nlnhat (25/08/2023)
         */
        filterDataComputed() {
            // Xử lý lọc từ combobox
            if (this.filterConfig.filterType == filterType.selectName)
                this.filterValue.key = this.filterValueName;
            else
                this.filterValue.key = this.filterValueKey;
            this.filterValue.name = this.filterValueName;

            // Xử lý giá trị lọc là số
            if (this.filterConfig.filterType == filterType.number)
                this.filterValue.key = this.reformatDecimal(this.filterValue.key)?.toString();
            else
                this.filterValue.key = this.filterValue.key?.toString();

            return {
                title: this.title,
                column: this.name,
                logicType: this.logicType.id,
                logicName: this.logicType.name.toLowerCase(),
                compareType: this.compareType.id,
                compareName: this.compareType.name.toLowerCase(),
                filterType: this.filterConfig.filterType,
                value: (this.compareType.id == compareType.empty) ? { key: '', name: '' } : { ...this.filterValue },
            }
        },
        /**
         * Set readonly input
         */
        isReadOnlyInput() {
            return (this.compareType.id == compareType.empty)
        }
    },
    methods: {
        /**
         * Handle when filter value changes
         * 
         * Author: nlnhat (25/08/2023)
         */
        onChangeFilterValue: debounce(function () {
            this.handleUpdateFilterModel();
        }, 500),
        /**
         * Update filter model
         * 
         * Author: nlnhat (25/08/2023)
         */
        handleUpdateFilterModel() {
            if (this.filterValueKey == null || this.filterValueKey === '') {
                this.onClickRemove();
            }
            else {
                this.onClickApply();
            }
        },
        /**
         * Handle when click apply filter
         * 
         * Author: nlnhat (25/08/2023)
         */
        onClickApply() {
            const data = this.filterDataComputed;
            this.$emit('emitUpdateFilterModel', data);
            this.appliedFilter = true;
        },
        /**
         * Handle when click remove filter
         * 
         * Author: nlnhat (25/08/2023)
         */
        onClickRemove() {
            if (this.filterModel != null)
                this.$emit('emitUpdateFilterModel', null);
            this.appliedFilter = false;
        },
        /**
         * Handle after selected logic type
         * 
         * Author: nlnhat (29/08/2023)
         */
        handleSelectedLogic() {
            this.handleUpdateFilterModel();
            this.focusOnCompare();
        },
        /**
         * Handle after selected compare type
         * 
         * Author: nlnhat (29/08/2023)
         */
        handleSelectedCompare() {
            this.handleUpdateFilterModel();
            this.focus();
        },
        /**
         * Focus on compare select
         * 
         * Author: nlnhat (25/08/2023)
         */
        focusOnCompare() {
            this.$nextTick(() => {
                if (this.$refs.compare != null)
                    this.$refs.compare.focus();
                else
                    this.focus();
            })
        },
        /**
         * Focus on button apply
         * 
         * Author: nlnhat (25/08/2023)
         */
        focusOnApply() {
            this.$nextTick(() => {
                this.$refs.apply.focus();
            })
        },
        /**
         * Focus input
         * 
         * Author: nlnhat (25/08/2023)
         */
        focus() {
            if (this.$refs.textfield != null && !this.isReadOnlyInput) {
                this.$refs.textfield.focus();
                this.$refs.textfield.select();
            }
            else if (this.$refs.datepicker != null && !this.isReadOnlyInput) {
                this.$refs.datepicker.focus();
                this.$refs.datepicker.select();
            }
            else if (this.$refs.combobox != null && !this.isReadOnlyInput) {
                this.$refs.combobox.focus();
            }
        },
        /**
         * Clear error message input
         * 
         * Author: nlnhat (25/08/2023)
         */
        clearErrorMessage() {
            if (this.$refs.textfield != null)
                this.$refs.textfield.clearErrorMessage();
            if (this.$refs.combobox != null)
                this.$refs.combobox.clearErrorMessage();
        },
        /**
         * Clear input
         * 
         * Author: nlnhat (25/08/2023)
         */
        clearInput() {
            this.filterValue = [];
        },
        /**
         * Validate filter value cannot empty
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateCannotEmpty(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (isNullOrEmpty(value)) {
                return `${label} ${this.$resources['vn'].cannotEmpty}`
            }
            return null;
        },
        /**
         * Validate month filter
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateMonthField(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (isNullOrEmpty(value)) {
                return `${label} ${this.$resources['vn'].cannotEmpty}`
            }
            return this.validateMonth(label, value);
        },
        /**
         * Validate year filter
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} label Label of filter
         * @param {*} value Filter value to check
         */
        validateYearField(label, value) {
            if (this.compareType.id == compareType.empty)
                return null;
            if (isNullOrEmpty(value)) {
                return `${label} ${this.$resources['vn'].cannotEmpty}`
            }
            return this.validateYear(label, value);
        },
        /**
         * Check validate filter value
         * 
         * Author: nlnhat (25/08/2023)
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
            if (this.isShowTextField || this.isShowNumberField || this.isShowMonthField || this.isShowYearField
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
         * Author: nlnhat (25/08/2023)
         */
        mapSelects() {
            if (this.filterConfig.selects) {
                this.selects = this.filterConfig.selects;
                // this.filterValueKey = this.selects[0].id;
                // this.filterValueName = this.selects[0].name;
            }
        },
        /**
         * Handle short key on filter
         * 
         * Author: nlnhat (29/08/2023)
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
        // formatDecimalInput,
        // reformatDecimal,
    }
}
</script>
<style>
@import "./filter.css";
</style>