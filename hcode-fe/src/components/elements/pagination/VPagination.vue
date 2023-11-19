<template>
    <div class="pagination-wrapper">
        <div class="count-wrapper">
            {{ $t("com.total") }}:
            <b class="pagination__highlight"> {{ totalRecord }}</b>
            <!-- <div v-show="this.percentage > 0 && this.percentage < 100">
            {{ this.$resources['vn'].percentage }}:
            <b>{{ percentageComputed }}</b>
        </div> -->
        </div>
        <div
            class="pagination"
            v-if="totalRecord"
        >
            <div class="pagination__property">
                {{ $t("com.recordsPerPage") }}:
                <v-combobox
                    ref="refPageSize"
                    v-model="pageSize"
                    optionLabel="name"
                    :options="optionsPageSize"
                ></v-combobox>
            </div>
            <div
                class="pagination__property"
                v-if="fromRecord < toRecord"
            >
                {{ $t('com.record') }}:
                <span>
                    <b class="">{{ fromRecord }}</b>
                </span>
                <span v-if="toRecord > 1">-</span>
                <span v-if="toRecord > 1">
                    <b class="">{{ toRecord }}</b>
                </span>
            </div>
            <div class="pagination__switch">
                <div
                    class="d-flex"
                    :class="{ 'disabled': page.pageNumber <= 1 }"
                >
                    <v-icon
                        class="pagination__angle"
                        icon="fa fa-angle-left"
                        color="light"
                        applyPointer
                        @click="onPrevious()"
                        v-if="page.pageNumber > 1"
                    >
                    </v-icon>
                </div>
                <div
                    :class="{ 'pagination__numbers': true, 'disabled': !canAccessRandom }"
                    v-if="isShowPageNumbers"
                >
                    <v-page-number
                        @emitPageActive="pageActive(pageNumbers[0].number)"
                        :pageNumber="pageNumbers[0]"
                    >
                    </v-page-number>
                    <div
                        class="pagination__view"
                        v-if="isShowPreviousView"
                        @click="onPreviousView()"
                    >
                        ...
                    </div>
                    <v-page-number
                        v-for="(pageNumber) in pageNumbers"
                        :key="pageNumber.number"
                        v-show="isShowed(pageNumber)"
                        @emitPageActive="pageActive(pageNumber.number)"
                        :pageNumber="pageNumber"
                    >
                    </v-page-number>
                    <div
                        class="pagination__view"
                        v-if="isShowNextView"
                        @click="onNextView()"
                    >
                        ...
                    </div>
                    <v-page-number
                        v-if="pageNumbers.length > 1"
                        @emitPageActive="pageActive(pageNumbers[pageNumbers.length - 1].number)"
                        :pageNumber="pageNumbers[pageNumbers.length - 1]"
                    >
                    </v-page-number>
                </div>
                <div
                    class="d-flex"
                    :class="{ 'disabled': pageNumbers.length <= page.pageNumber }"
                >
                    <v-icon
                        class="pagination__angle"
                        icon="fa fa-angle-right"
                        color="light"
                        applyPointer
                        @click="onNext()"
                        v-if="pageNumbers.length > page.pageNumber"
                    >
                    </v-icon>
                </div>
            </div>
            <!-- page-number--active -->
        </div>
    </div>
</template>
<script>

export default {
    name: 'VPagination',
    props: {
        /**
         * (Props) Total record
         */
        totalRecord: {
            type: Number,
            default: 0,
        },
        /**
         * (Props) All record
         */
        allRecord: {
            type: Number,
            default: 0,
        },
        /**
         * (Props) Page object (includes pageNumber and pageSize 
         */
        pageModel: {
            type: Object,
            default: {
                pageNumber: 1,
                pageSize: 20,
            }
        },
        /**
         * Can access to any page number or access each page one by one
         */
        canAccessRandom: {
            type: Boolean,
            default: true,
        },
        /**
         * Show page numbers or not
         */
        isShowPageNumbers: {
            type: Boolean,
            default: true,
        },
        /**
         * Hide arrow when disabled
         */
        hideArrow: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * (Data) Page properties (pageNumber, pageSize)
             */
            page: this.pageModel,
            /**
             * Page numbers
             */
            pageNumbers: [],
            /**
             * Selected pagesize
             */
            pageSize: {},
            /**
             * Selects for records/page 
             */
            optionsPageSize: [
                {
                    id: 10,
                    name: 10
                },
                {
                    id: 20,
                    name: 20
                },
                {
                    id: 30,
                    name: 30
                },
                {
                    id: 50,
                    name: 50
                },
                {
                    id: 100,
                    name: 100
                }
            ],
            /**
             * Current page view
             */
            pageView: 1,
            /**
             * Percentage totalRecord/allRecord
             */
            percentage: 0,
        }
    },
    created() {
        this.pageNumbers = this.pageNumbersComputed;
        this.pageView = this.page.pageNumber;
        this.pageSize = this.optionsPageSize[0];
    },
    mounted() {
        // Add shortkey event
        window.addEventListener('keydown', this.handleShortKey);
    },
    beforeUnmount() {
        // Remove shortkey event
        window.removeEventListener('keydown', this.handleShortKey);
    },
    computed: {
        /**
         * Return length of total pages
         * 
         * Author: nlnhat (22/06/2023)
         */
        totalPage() {
            const length = Math.ceil(this.totalRecord / this.page.pageSize)
            return length
        },
        /**
         * Return page numbers
         * 
         * Author: nlnhat (22/06/2023)
         */
        pageNumbersComputed() {
            let newPageNumbers = []
            for (var i = 1; i <= this.totalPage; i++) {
                const pageNumber = {
                    number: i,
                    isActive: (i == this.page.pageNumber),
                };
                newPageNumbers.push(pageNumber)
            }
            return newPageNumbers
        },
        /**
         * Return index of first record in a page
         * 
         * Author: nlnhat (22/06/2023)
         */
        fromRecord() {
            return (this.page.pageSize * (this.page.pageNumber - 1) + 1)
        },
        /**
         * Return index of last record in a page
         * 
         * Author: nlnhat (22/06/2023)
         */
        toRecord() {
            return Math.min(this.fromRecord + this.page.pageSize - 1, this.totalRecord)
        },
        /**
         * Show or hide icon previous view
         * 
         * Author: nlnhat (22/06/2023)
         */
        isShowPreviousView() {
            return (this.pageView - 1 > 1 && this.pageNumbers.length > 4)
        },
        /**
         * Show or hide icon next view
         * 
         * Author: nlnhat (22/06/2023)
         */
        isShowNextView() {
            return (this.pageView + 2 < this.pageNumbers.length && this.pageNumbers.length > 4)
        },
        /**
         * Percentage totalRecord/allRecord
         * 
         * Author: nlnhat (22/06/2023)
         */
        percentageComputed() {
            this.percentage = (this.totalRecord / this.allRecord) * 100;
            this.percentage = this.percentage % 1 == 0 ? this.percentage.toFixed(0) : this.percentage.toFixed(1);
            return `${this.percentage}%`
        },
    },
    emits: ['emitUpdatePage'],
    watch: {
        /**
         * Handler when page model changes
         * 
         * Author: nlnhat (22/06/2023)
         */
        pageModel: {
            handler() {
                this.page = this.pageModel
            },
            deep: true
        },
        /**
         * Handle when page number changes
         * 
         * Author: nlnhat (22/06/2023)
         * @param {*} newNumber New page number
         * @param {*} oldNumber Old page number
         */
        "page.pageNumber": function (newNumber, oldNumber) {
            this.pageNumbers.forEach(pageNumber => {
                if (pageNumber.number == oldNumber) {
                    pageNumber.isActive = false
                }
                if (pageNumber.number == newNumber) {
                    pageNumber.isActive = true
                    this.pageView = pageNumber.number;
                }
            });
        },
        /**
         * Handle when page size changes
         * 
         * Author: nlnhat (22/06/2023)
         */
        "page.pageSize": function () {
            this.pageNumbers = this.pageNumbersComputed;
            this.page.pageNumber = 1
            this.updatePage();
        },
        /**
         * Handle when total record changes
         * 
         * Author: nlnhat (22/06/2023)
         */
        totalRecord() {
            this.pageNumbers = this.pageNumbersComputed;
        },
    },
    methods: {
        /**
         * Kích hoạt page number khi click vào
         * 
         * Author: nlnhat (22/06/2023)
         * @param {*} number Page number được kích hoạt
         */
        pageActive(number) {
            this.page.pageNumber = number;
            this.updatePage();
        },
        /**
         * Jump to previous page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onPrevious() {
            this.page.pageNumber = Math.max(1, --this.page.pageNumber);
            this.updatePage();
        },
        /**
         * Jump to next page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onNext() {
            this.page.pageNumber = Math.min(this.pageNumbers.length, ++this.page.pageNumber);
            this.updatePage();
        },
        /**
         * Jump to previous page view
         * 
         * Author: nlnhat (22/06/2023)
         */
        onPreviousView() {
            this.pageView = Math.max(1, this.pageView - 3)
        },
        /**
         * Jump to next page view
         * 
         * Author: nlnhat (22/06/2023)
         */
        onNextView() {
            this.pageView = Math.min(this.pageNumbers.length, this.pageView + 3)
        },
        /**
         * Show page number or not
         * 
         * Author: nlnhat (22/06/2023)
         */
        isShowed(pageNumber) {
            const number = pageNumber.number;
            const pageView = this.pageView;
            const totalPage = this.pageNumbers.length;

            if (number == 1 || number == this.pageNumbers.length) return false;
            if (pageView <= 2) return (number <= 3);
            if (pageView == totalPage - 3) return (number >= pageView - 1 && number <= pageView + 1);
            if (pageView >= totalPage - 2) return (number >= totalPage - 2);
            return (number >= pageView && number <= pageView + 2)
        },
        /**
         * Handle shortcut keys
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;
            // Left: Trang trước
            if (event.ctrlKey && event.keyCode == code.left) {
                event.preventDefault();
                event.stopPropagation();
                this.onPrevious();
            }
            // Right: Trang tiếp
            else if (event.ctrlKey && event.keyCode == code.right) {
                event.preventDefault();
                event.stopPropagation();
                this.onNext();
            }
            // Ctrl + up : Trang đầu
            else if (event.ctrlKey && event.keyCode == code.up) {
                event.preventDefault();
                event.stopPropagation();
                this.page.pageNumber = 1;
                this.updatePage();
            }
            // Ctrl + up: Trang cuối
            else if (event.ctrlKey && event.keyCode == code.down) {
                event.preventDefault();
                event.stopPropagation();
                this.page.pageNumber = this.pageNumbers.length;
                this.updatePage();
            }
        },
        /**
         * Update page property
         */
        updatePage() {
            this.$emit('emitUpdatePage', this.page)
        }
    }
}
</script>
<style>
@import './pagination.css';
</style>