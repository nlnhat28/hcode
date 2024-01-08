<template>
    <div
        class="box-dropdown"
        v-click-outside="hideMenu"
    >
        <div
            :class="{ 'box-dropdown__box': true, 'box-dropdown__box--active': isActiveFilter }"
            v-tooltip="selected.note"
            tabindex="0"
            ref="box"
            @click="toggleMenu"
        >
            {{ selected.name }}
        </div>
        <div
            class="p-relative"
            v-if="isShowMenu"
        >
            <table class="select-table table--noborder">
                <tbody>
                    <tr
                        v-for="(compare) in compares"
                        :key="compare.id"
                        ref="tr"
                        @click="onClickSelect(compare)"
                    >
                        <m-td textAlign="center">
                            <m-icon-container
                                icon="cukcuk-dot--green"
                                v-if="compare.id == selected.id"
                            ></m-icon-container>
                        </m-td>
                        <m-td
                            textAlign="center"
                            :content="compare.name"
                        ></m-td>
                        <m-td
                            textAlign="left"
                            :content="compare.note"
                        ></m-td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISACompareSelect',
    props: {
        /**
         * Danh sách lựa chọn
         */
        compares: {
            type: Array,
        },
        /**
         * Compare object
         */
        compareModel: {
            type: Object,
        },
        /**
         * Filter is active
         */
        isActiveFilter: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * Hiện menu hay không
             */
            isShowMenu: false,
            /**
             * Selected object
             */
            selected: { ...this.compareModel }
        }
    },
    expose: ['focus'],
    methods: {
        /**
         * Hiện menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        showMenu() {
            this.isShowMenu = true;
        },
        /**
         * Ẩn menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        hideMenu() {
            this.isShowMenu = false;
        },
        /**
         * Ẩn/Hiện menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        toggleMenu() {
            this.isShowMenu = !this.isShowMenu;
        },
        /**
         * Handle event click on a select
         * 
         * Author: nlnhat (19/08/2023)
         * @param {*} select Select on clicked
         */
        onClickSelect(select) {
            try {
                this.selected = {
                    id: select.id,
                    name: select.name,
                    note: select.note
                };
                this.$emit('update:compareModel', this.selected)
                this.onSelected();
                this.isShowMenu = false;
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle after selected
         * 
         * Author: nlnhat (01/07/2023)
         */
        onSelected() {
            this.$emit('emitSelected');
        },
        /**
         * Handle remove filter
         * 
         * Author: nlnhat (01/07/2023)
         */
        onClickRemove() {
            this.isShowMenu = false;
            this.$emit('emitRemoveFilter')
        },
        /**
         * Focus on box
         * 
         * Author: nlnhat (01/07/2023)
         */
        focus() {
            if (this.$refs.box != null) {
                this.$refs.box.focus();
            }
        },
    }
}
</script>
<style scoped>
table.select-table td {
    padding: 0 4px;
}

table.select-table td:first-child {
    padding-right: 0px;
}

table.select-table td:last-child {
    padding-right: 8px;
}
</style>