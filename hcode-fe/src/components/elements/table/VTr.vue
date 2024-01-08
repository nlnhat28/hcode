<template>
    <tr
        tabindex="0"
        :class="{ 'tr--selected': isSelected, 'tr--dark': index % 2 != 0 && applyDark, 'tr--focused': isFocusedComputed }"
        @keydown="onKeyDown"
        @dblclick="onUpdate"
        @click="onClick"
        @focus="onFocus"
        @contextmenu="onContextMenu"
        v-contextmenu-outside="hideContextMenu"
        ref="tr"
    >
        <slot name="content"></slot>
    </tr>
    <!-- <m-context-menu
        v-click-outside="hideContextMenu"
        v-if="isShowContextMenu"
        :actions="contextMenuActions"
        :position="positionContextMenu"
        @emitClickItem="hideContextMenu"
    >
    </m-context-menu> -->
</template>
<script>
export default {
    name: 'VTr',
    props: {
        /**
         * This record in row is selected or not
         */
        isSelected: {
            type: Boolean,
            default: false,
        },
        /**
         * Index of row
         */
        index: {
            type: Number,
        },
        /**
         * Id of row
         */
        id: {
            type: [String, Number]
        },
        /**
         * Focused id
         */
        focusedId: {
            type: String,
            default: null,
        },
        /**
         * Apply dark
         */
        applyDark: {
            type: Boolean,
            default: true,
        },
        /**
         * Apply focus
         */
        applyFocus: {
            type: Boolean,
            default: true,
        },
        /**
         * Focused ids
         */
        focusedIds: {
            type: Array,
            default: []
        },
    },
    data() {
        return {
            /**
             * Focused state
             */
            isFocused: false,
            /**
             * Action for context menu
             */
            contextMenuActions: [
            ],
            /**
             * Show or hide context menu
             */
            isShowContextMenu: false,
            /**
             * Style of context menu
             */
            positionContextMenu: {
                top: null,
                left: null,
            },
            /**
             * Old focused id
             */
            oldFocusedId: this.focusedId,
        }
    },
    mounted() {
        this.focusById();
    },
    emits: [
        'emitUpdate',
        'emitDelete',
        'emitDuplicate',
        'emitCreate',
        'emitExport',
        'emitReload',
        'emitFocusNext',
        'emitFocusPrevious',
        'emitFocusById',
        'emitSelectMany',
        'update:focusedId',
        'update:focusedIds',
    ],
    expose: [
        'focus',
        'index',
        'id',
        'vanish',
        'setFocus',
        'clearFocus'],
    watch: {
        // focusedId() {
        //     this.focusById();
        // }
    },
    computed: {
        /**
         * Focus hay không
         * 
         * Author: nlnhat (29/08/2023)
         */
        isFocusedComputed() {
            return (this.id == this.focusedId || this.isIncluded)
        },
        /**
         * Focused ids đang chứa id này chưa
         * 
         * @return True if includes
         */
        isIncluded() {
            return this.focusedIds.includes(this.id);
        }
    },
    methods: {
        /**
         * Handle short key
         * 
         * Author: nlnhat (29/07/2023)
         * @param {*} event Keydown event on tr
         */
        onKeyDown(event) {
            const code = this.$enums.keyCode;
            // Space || F2: Update
            if (event.keyCode == code.space || event.keyCode == code.f2) {
                event.preventDefault();
                event.stopPropagation();
                this.onUpdate();
            }
            // Ctrl + D || Delete: Delete
            else if ((event.ctrlKey && event.keyCode == code.d) || event.keyCode == code.delete) {
                event.preventDefault();
                event.stopPropagation();
                this.onDelete();
            }
            // Ctrl + V: Duplicate
            else if (event.ctrlKey && event.keyCode == code.v) {
                event.preventDefault();
                event.stopPropagation();
                this.onDuplicate();
            }
            // Arrow down: Focus on next row
            else if (event.keyCode == code.down) {
                event.preventDefault();
                event.stopPropagation();
                this.focusNext();
            }
            // Arrow up: Focus on previous row
            else if (event.keyCode == code.up) {
                event.preventDefault();
                event.stopPropagation();
                this.focusPrevious();
            }
        },
        /**
         * Update record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onUpdate() {
            this.$emit('emitUpdate');
        },
        /**
         * Delete record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onDelete() {
            this.$emit('emitDelete');
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onDuplicate() {
            this.$emit('emitDuplicate');
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusNext() {
            this.$emit('emitFocusNext', this.index);
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusPrevious() {
            this.$emit('emitFocusPrevious', this.index);
        },
        /**
         * Focus on tr
         * 
         * Author: nlnhat (29/07/2023)
         */
        focus() {
            this.$nextTick(() => {
                if (this.$refs.tr)
                    this.$refs.tr.focus();
            });
        },
        /**
         * Focus on tr if id equals focus id
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusById() {
            if (this.focusedId == this.id) {
                this.focus();
                // this.$emit('update:focusedId', null);
            }
        },
        /**
         * Tạo mới
         * 
         * Author: nlnhat (29/07/2023)
         */
        create() {
            this.$emit('emitCreate')
        },
        /**
         * Reload data
         * 
         * Author: nlnhat (29/07/2023)
         */
        reload() {
            this.$emit('emitReload')
        },
        /**
         * Export to excel
         * 
         * Author: nlnhat (29/07/2023)
         */
        export() {
            this.$emit('emitExport')
        },
        /**
         * Make vanish effect when delete 
         * 
         * Author: nlnhat (09/08/2023)
         */
        vanish() {
            this.$nextTick(() => {
                if (this.$refs.tr)
                    this.$refs.tr.style.transition = 'opacity 400ms';
                this.$refs.tr.style.opacity = '0';
            });
        },
        /**
         * Set class focus
         * 
         * Author: nlnhat (09/08/2023)
         */
        setFocus() {
            this.isFocused = true;
        },
        /**
         * Clear class focus
         * 
         * Author: nlnhat (09/08/2023)
         */
        clearFocus() {
            this.isFocused = false;
        },
        /**
         * Handle when right click on tr
         *
         * Author: nlnhat (09/08/2023)
         */
        onContextMenu(event) {
            event.preventDefault();
            this.positionContextMenu.left = event.clientX;
            this.positionContextMenu.top = event.clientY;
            this.showContextMenu();
        },
        /**
         * Show context menu
         *
         * Author: nlnhat (09/08/2023)
         */
        showContextMenu() {
            this.isShowContextMenu = true;
        },
        /**
         * Hide context menu
         *
         * Author: nlnhat (09/08/2023)
         */
        hideContextMenu() {
            this.isShowContextMenu = false;
        },
        /**
         * Handle when focus
         *
         * Author: nlnhat (28/08/2023)
         */
        onFocus() {
            this.oldFocusedId = this.focusedId;
            this.$emit('update:focusedId', this.id);
        },
        /**
         * Xử lý khi click
         *
         * Author: nlnhat (28/08/2023)
         */
        onClick(event) {
            document.getSelection().removeAllRanges();
            if (event.ctrlKey) {
                this.onCtrlClick();
            }
            else if (event.shiftKey) {
                this.onShiftClick();
            }
            else {
                this.$emit('update:focusedIds', [this.id]);
            };
        },
        /**
         * Xử lý click khi giữ ctrl
         *
         * Author: nlnhat (28/08/2023)
         */
        onCtrlClick() {
            // Nếu có trong danh sách thì bỏ
            if (this.isIncluded) {
                this.$emit('update:focusedIds', this.focusedIds.filter(id => id != this.id));
                if (this.focusedIds.length > 0) {
                    this.$emit('emitFocusById', this.focusedIds[0]);
                }
            }
            // Nếu không có trong danh sách thì thêm vào
            else {
                this.$emit('update:focusedIds', [...this.focusedIds, this.id]);
            }
        },
        /**
         * Xử lý click khi giữ shift
         *
         * Author: nlnhat (28/08/2023)
         */
        onShiftClick() {
            this.$emit('emitSelectMany', this.oldFocusedId, this.id)
        }
    }
}
</script>
<style scoped>
@import './table.css';
</style>