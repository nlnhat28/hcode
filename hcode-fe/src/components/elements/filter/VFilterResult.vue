<template>
    <div class="filter-result">
        <b>{{ capitalizeFirstChar(filterData.title) }}</b>
        <div class="d-flex">
            {{ filterData.compareName }}
        </div>
        <div
            class="d-flex"
            v-if="filterValueComputed != null && filterValueComputed != ''"
        >
            "
            <pre class="filter-result__value">{{ filterValueComputed }}</pre>"
        </div>
        <div
            class="icon-container"
            @click="onClickRemoveFilter()"
            v-tooltip="this.$resources['vn'].removeFilter"
        >
            <m-icon-x class="filter-result__remove"></m-icon-x>
        </div>
    </div>
</template>
<script>
import { capitalizeFirstChar } from '@/js/utils/string.js'
import { formatDate } from '@/js/utils/format.js'
export default {
    name: 'MISAFilterResult',
    props: {
        /**
         * Filter item object
         */
        filterItem: {
            type: Object,
        },
        /**
         * Index of filter item
         */
        index: {
            type: Number,
        },
        /**
         * Date format
         */
        dateFormat: {
            type: String,
            default: 'dd/MM/yyyy',
            validator: (value) => {
                return [
                    'dd/MM/yyyy',
                    'yyyy/MM/dd'
                ].includes(value);
            },
        },
    },
    data() {
        return {
            filterData: null,
        }
    },
    created() {
        this.filterData = { ...this.filterItem };
    },
    watch: {
        /**
         * Update filter data when filter item changes
         */
        filterItem: {
            handler() {
                this.filterData = { ...this.filterItem };
            },
            deep: true
        }
    },
    computed: {
        /**
         * Return filter value string
         * 
         * Author: nlnhat (26/07/2023)
         */
        filterValueComputed() {
            const valueNames = [];
            if (this.filterData.values) {
                const values = [...this.filterData.values]
                for (const value of values) {
                    let displayValue = null
                    if (value.name == null || value.name == '')
                        displayValue = value.key;
                    else {
                        displayValue = value.name;
                    }
                    // Format if value is date
                    if (this.filterData.filterType == this.$enums.filterType.date
                        && this.filterData.compareType != this.$enums.compareType.month
                        && this.filterData.compareType != this.$enums.compareType.year)
                        displayValue = this.formatDate(displayValue, this.dateFormat);
                    valueNames.push(displayValue);
                }
            }
            return valueNames.join(',');
        }
    },
    methods: {
        /**
         * Handle click remove filter
         * 
         * Author: nlnhat (26/07/2023)
         */
        onClickRemoveFilter() {
            this.$emit('emitRemoveFilterItem', this.index);
        },
        /**
         * Imported methods
         */
        capitalizeFirstChar,
        formatDate,
    }
}
</script>
<style scoped>.filter-result__remove {
    scale: 0.64;
}</style>