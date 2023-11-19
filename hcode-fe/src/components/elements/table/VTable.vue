<template>
    <div class="table-container">
        <div class="table-container__top d-flex-column flex-1">
            <!-- table toolbar -->
            <div class="table-toolbar">
                <div class="table-toolbar__left">
                    <slot name="toolbarLeft"></slot>
                </div>
                <div class="table-toolbar__right">
                    <slot name="toolbarRight"></slot>
                    <v-search-box
                        v-model="keySearch"
                        ref="searchBox"
                    ></v-search-box>
                </div>
            </div>
            <!-- table content -->
            <div
                class="table-content"
                @scroll="onScroll()"
                ref="tableContent"
            >
                <table :class="{ 'table': true, '.disabled': isLoading }">
                    <thead
                        class="table__head"
                        ref="tableHead"
                        v-if="!isHideHead"
                    >
                        <slot name="thead"></slot>
                        <v-th
                            v-for="(col, index) in columns"
                            :key="index"
                            :textAlign="col.textAlign"
                            :widthCell="col.widthCell"
                            :name="col.name"
                            :title="col.title"
                            :fullTitle="col?.fullTitle"
                            :sortType="col.sortType"
                            :filterConfig="col.filterConfig"
                            :resizable="col.resizable == true ? true : resizable"
                        >
                            <!-- v-model:sortModel="sortModels[index]"
                            v-model:filterModel="filterModels[index]"
                        > -->
                            {{ col.title }}
                        </v-th>
                    </thead>
                    <tbody class="table__body">
                        <slot name="tbody"></slot>
                        <!-- table body -->
                        <v-tr
                            v-if="!this.$slots.tbody"
                            v-for="(item, index) in items"
                            :key="item[itemId] ?? index"
                            :index="index"
                            :id="item[itemId]"
                            ref="tr"
                            @emitUpdate="showMaterialForm(item)"
                            @emitDelete="handleDeleteOnRow(item[itemId])"
                            @emitDuplicate="duplicateMaterial(item)"
                            @emitCreate="showEmptyMaterialForm"
                            @emitReload="onReloadData"
                            @emitExport="exportToExcel"
                            @emitFocusNext="focusNextRow"
                            @emitFocusPrevious="focusPreviousRow"
                            @emitFocusById="focusById"
                            @emitSelectMany="selectMany"
                        >
                            <!-- :isSelected="isSelected(item[itemId])" -->
                            <template #content>
                                <v-td
                                    v-for="(col, index) in columns"
                                    :key="index"
                                    :textAlign="col.textAlign"
                                    :content="item[col.name]"
                                >
                                </v-td>
                                <!-- Ngừng theo dõi-->
                                <!-- <v-td textAlign="center">
                                    <v-checkbox
                                        :checked="item.IsUnfollow"
                                        :isReadOnly="true"
                                    ></v-checkbox>
                                </v-td> -->
                            </template>
                        </v-tr>
                    </tbody>
                </table>
                <v-loading v-if="isLoading"></v-loading>
                <v-no-content v-if="(!isLoading && totalRecord == 0)"></v-no-content>
            </div>
        </div>
        <!-- table footer -->
        <div class="table__footer">
            <slot name="tfooter"></slot>
        </div>
    </div>
</template>
<script>
export default {
    name: 'VTable',
    props: {
        columns: {
            /*         
                {
                    key: "MaterialCode",
                    title: Tiêu đề,
                    textAlign: 'left',
                    widthCell: 160,
                    sortType: this.$enums.sortType.blur,
                    filterConfig: {
                        filterType: this.$enums.filterType.text,
                    }
                },
             */
            type: Array,
            default: []
        },
        /**
         * List of items
         */
        items: {
            type: Array
        },
        /**
         * List of selected items
         */
        selectedItems: {
            type: Array
        },
        /**
         * Tên trường Id
         */
        itemId: {
            type: String,
            default: "id"
        },
        /**
         * Total record in database
         */
        totalRecord: {
            type: Number
        },
        /**
         * Loading state
         */
        isLoading: {
            type: Boolean
        },
        /**
         * Không show head
         */
        isHideHead: {
            type: Boolean,
            default: false
        },
        /**
         * Resizable or not
         */
        resizable: {
            type: Boolean,
            default: true,
        }
    },
    expose: [
        'scrollTop',
    ],
    beforeUnmount() {
        this.$refs.tableContent.scrollTop = 0;
    },
    methods: {
        /**
         * Handle scroll table content
         *
         * Author: nlnhat (26/06/2023)
         */
        onScroll() {
            this.showHideTheadShadow();
            this.$emit('emitScroll');
        },
        /**
         * Scroll to postion
         * 
         * Author: nlnhat (26/06/2023)
         * @param {number} position 
         */
        scrollTop(position) {
            this.$refs.tableContent.scrollTop = position;
        },
        /**
         * Remove thead shadow when scroll at top
         *
         * Author: nlnhat (26/06/2023)
         */
        showHideTheadShadow() {
            try {
                if (this.$refs.tableContent.scrollTop === 0) {
                    this.$refs.tableHead.style.boxShadow = "none";
                } else {
                    this.$refs.tableHead.style.boxShadow = "0px 2px 10px rgba(23, 27, 42, 0.2)";
                }
            } catch { }
        }
    }
}
</script>
<style scoped>
@import './table.css';
</style>