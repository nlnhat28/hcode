<template>
    <th v-if="!isHide"
        :style="`text-align: ${textAlign}; width: ${width}px; min-width: ${width}px;`"
        ref="th"
    >
        <div
            :class="[
                'th__sort',
                { 'cursor-pointer': sortConfig.sortType != $enums.sortType.disabled },
                { 'flex-start': textAlign == 'left' },
                { 'justify-center': textAlign == 'center' },
                { 'flex-start': textAlign == 'right' },
            ]"
            @click="onClickSortType()"
        >
            {{ title }}
            <v-icon
                v-if="showIconSort"
                class="th__sort-icon"
                applyPointer
                :color="sort == $enums.sortType.asc ? 'success' : 'error'"
                :size="30"
                :icon="sortIcon"
            >
            </v-icon>
        </div>
        <div
            class='th__filter'
            v-if="filterConfig"
        >
            <!-- v-click-outside="hideFilter" -->
            <v-icon
                :icon="'far fa-filter'"
                :class="[
                    'th__filter-icon',
                    { 'th__filter--active': filterData },
                    { 'th__filter--show': isShowFilter }
                ]"
                :color="filterData ? 'info' : isShowFilter ? 'light' : 'dark'"
                applyPointer
                :size="30"
                @click="toggleDisplayFilter"
            >
            </v-icon>
            <div class="p-relative">
                <v-filter
                    :filterConfig="filterConfig"
                    :filterModel="filterData"
                    :name="name"
                    :title="fullTitle ?? title"
                    v-show="isShowFilter"
                    @emitUpdateFilterModel="updateFilterModel"
                    @emitHideFilter="hideFilter"
                    ref="filter"
                >
                </v-filter>
            </div>
        </div>
        <span
            class="th__resize"
            v-if="resizable"
            @mousedown="startResize"
        ></span>
    </th>
</template>
<script>
import enums from '@/enums/enums.js'
const sortType = enums.sortType;

export default {
    name: 'VTh',
    props: {
        /**
         * Width of cell
         */
        widthCell: {
            type: [String, Number],
            default: 160,
        },
        /**
         * Text align of cell
         */
        textAlign: {
            type: String,
            default: 'left',
            validator: (value) => {
                return [
                    'left',
                    'center',
                    'right',
                ].includes(value);
            },
        },
        /**
         * Name of column
         */
        name: {
            type: String
        },
        /**
         * Title of column
         */
        title: {
            type: String
        },
        /**
         * Tooltip string
         */
        fullTitle: {
            type: String,
            default: null
        },
        /**
         * Kiểu sắp xếp
         */
        // sortType: {
        //     type: Number,
        //     default: sortType.disabled,
        //     validator: (value) => {
        //         return [
        //             sortType.disabled,
        //             sortType.asc,
        //             sortType.desc,
        //             sortType.blur,
        //         ].includes(value);
        //     },
        // },
        /**
         * Config để sort {sortType, field}
         */
        sortConfig: {
            type: Object,
            default: {}
        },
        /**
         * (Prop) Sort object
         */
        sortModel: {
            type: Object
        },
        /**
         * Filter config
         */
        filterConfig: {
            type: Object,
            default: null,
        },
        /**
         * Filter model output
         */
        filterModel: {
            type: Object
        },
        /**
         * Resizable or not
         */
        resizable: {
            type: Boolean,
            default: true,
        },
        /**
         * Ẩn
         */
        isHide: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * Width of cell
             */
            width: parseInt(this.widthCell),
            /**
             * Is resizing or not
             */
            isResizing: false,
            /**
             * Position X before resize
             */
            startX: 0,
            /**
             * Sort state
             */
            sort: this.sortConfig.sortType,
            /**
             * Sort icon
             */
            sortIcon: 'fa fa-arrow-up',
            /**
             * Filter data
             */
            filterData: null,
            /**
             * Show filter or not
             */
            isShowFilter: false,
        }
    },
    watch: {
        /**
         * Watch sort data
         */
        sort() {
            this.sortIcon = this.sortIconComputed;
            this.$emit('update:sortModel', this.sortModelComputed)
        },
        /**
         * Watch fitlter item to update filter data
         */
        filterModel: {
            handler() {
                this.filterData = this.filterModel;
            },
            deep: true
        }
    },
    computed: {
        showIconSort() {
            return this.sort == sortType.asc || this.sort == sortType.desc
        },
        /**
         * Đổi icon sort
         * 
         * Author: nlnhat (20/07/2023)
         */
        sortIconComputed() {
            switch (this.sort) {
                case this.$enums.sortType.asc:
                    return 'fa fa-arrow-up'
                case this.$enums.sortType.desc:
                    return 'fa fa-arrow-down'
                // default:
                //     return 'fa fa-arrow-up';
            }
        },
        /**
         * Trả về thuộc tính sắp xếp
         * 
         * Author: nlnhat (20/07/2023)
         */
        sortModelComputed() {
            if (this.sort == this.$enums.sortType.asc || this.sort == this.$enums.sortType.desc)
                return {
                    sortType: this.sort,
                    columnName: this.sortConfig.field,
                };
            else return null;
        },
        /**
         * Show filter textfield or not
         * 
         * Author: nlnhat (20/07/2023)
         */
        isShowTextField() {
            return (
                this.filterConfig.filterType == this.$enums.filterType.text ||
                this.filterConfig.filterType == this.$enums.filterType.date)
        },
    },
    created() {
        if (this.sortConfig && !this.sortConfig.field) {
            this.sortConfig.field = this.name;
        }
    },
    methods: {
        /**
         * Start resize when mouse down
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        startResize(event) {
            this.isResizing = true;
            this.startX = event.clientX;
            document.addEventListener('mousemove', this.resize);
            document.addEventListener('mouseup', this.endResize);
        },
        /**
         * Resize when mouse move
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        resize(event) {
            if (this.isResizing) {
                const deltaX = event.clientX - this.startX;
                this.width = Math.max(100, this.width + deltaX);
                this.startX = event.clientX;
            }
        },
        /**
         * End resize when mouse up
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        endResize() {
            this.isResizing = false;
            document.removeEventListener('mousemove', this.resize);
            document.removeEventListener('mouseup', this.endResize);
        },
        /**
         * Return width of th 
         */
        getWidth() {
            return this.$refs.th.offsetWidth;
        },
        /**
         * Change sort type
         * 
         * Author: nlnhat (20/07/2023)
         */
        onClickSortType() {
            if (this.sortConfig.sortType != sortType.disabled) {
                switch (this.sort) {
                    case this.$enums.sortType.blur:
                        this.sort = this.$enums.sortType.asc
                        break;
                    case this.$enums.sortType.asc:
                        this.sort = this.$enums.sortType.desc
                        break;
                    case this.$enums.sortType.desc:
                        this.sort = this.$enums.sortType.blur
                        break;
                    default:
                        break;
                }
            }
        },
        /**
         * Toggle show or hide filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        toggleDisplayFilter() {
            this.isShowFilter = !this.isShowFilter;
            if (this.isShowFilter) {
                this.$nextTick(() => {
                    this.$refs.filter.focus();
                })
            }
        },
        /**
         * Show filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        showFilter() {
            this.isShowFilter = true;
        },
        /**
         * Hide filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        hideFilter() {
            this.isShowFilter = false;
        },
        /**
         * Hide filter
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} filterData Filter data from filter component
         */
        updateFilterModel(filterData) {
            this.$emit('update:filterModel', filterData);
        }
    },
}
</script>
<style scoped>
@import './table.css';

.icon-container {
    display: inline-flex;
}
</style>