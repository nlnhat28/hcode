<script>
// import ItemForm from './item-form/ItemForm.vue';
// import ItemStat from './item-stat/ItemStat.vue';
// import ItemImport from './item-import/ItemImport.vue';
// import {
//   formatDate,
//   formatStringByDot,
//   formatStringBySpace,
//   formatDecimal
// } from "@/js/utils/format.js";
// import { sortByName } from '@/js/utils/array.js'
// import { download } from '@/js/utils/file.js'
// import { openUrl } from "@/js/utils/window.js";
// import { useUnitStore, useWarehouseStore } from '@/stores/stores.js';
// import { mapStores, mapState } from 'pinia';
import { loadingEffect, handleResponse } from "@/mixins/mixins.js"

export default {
    name: "BaseList",
    mixins: [loadingEffect, handleResponse],
    components: {
    },
    data() {
        return {
            /**
             * Columns
             */
            columns: [],
            /**
             * Service chính
             */
            itemService: {},
            /**
             * List of items to show on table
             */
            items: [],
            /**
             * List of selected items
             */
            selectedItems: [],
            /**
             * Item showed on form to update
             */
            itemUpdate: {},
            /**
             * Tên key id
             */
            itemIdKey: 'id',
            /**
             * Id of item to focus
             */
            focusedId: null,
            /**
             * Ids of item to focus
             */
            focusedIds: [],
            /**
             * Phân trang
             */
            pagingModel: {
                pageNumber: 1,
                pageSize: 10,
            },
            /**
             * Key search
             */
            keySearch: '',
            /**
             * Total record on table
             */
            totalRecord: 0,
            /**
             * All record in database
             */
            allRecord: 0,
            /**
             * Loading flag
             */
            isLoading: true,
            /**
             * Key of item form
             */
            formKey: 0,
            /**
             * Danh sách các sort model mới từ các cột
             */
            sortModels: [],
            /**
             * Danh sách cũ các sort model từ các cột 
             */
            oldSortModels: [],
            /**
             * Hàng đợi sort (Sắp xếp sort model theo thứ tự chọn)
             */
            queueSortModels: [],
            /**
             * Các filter model từ các cột
             */
            filterModels: [],
            /**
             * Các filter model không rỗng từ các cột
             */
            filterModelsClean: [],
            /**
             * Flag
             */
            isFilterSuccess: true,
            /**
             * Date format
             */
            dateFormat: 'dd/MM/yyyy',
            /**
             * Title
             */
            documentTitle: null,
            /**
             * Ref name muốn focus đầu tiên
             */
            refFocusFirst: 'refSearchBox',
        };
    },
    async created() {
        document.title = this.$cf.documentTitle(this.documentTitle);
        await this.initOnCreated();
        await this.loadDataOnCreated();
        // this.isLoading = false
    },
    async mounted() {
        this.firstFocus();
    },
    watch: {
        /**
         * Reload data if search changes
         * 
         * Author: nlnhat (05/08/2023)
         */
        async keySearch() {
            await this.reloadItems();
            this.focusFirstItem();
        },
        /**
         * Theo dõi thay đổi của mỗi sort item
         * 
         * Author: nlnhat (05/08/2023)
         */
        sortModels: {
            async handler() {
                // Tìm item bị thay đổi
                const newItem = this.sortModels.find(item => item != null && !this.oldSortModels.includes(item)) ?? null;
                const oldItem = this.oldSortModels.find(item => item != null && !this.sortModels.includes(item)) ?? null;

                // Cập nhật item trong hàng đợi
                if (oldItem) {
                    const oldIndex = this.queueSortModels.indexOf(oldItem);
                    if (oldIndex != -1) this.queueSortModels[oldIndex] = newItem
                    else this.queueSortModels.push(newItem);
                }
                else this.queueSortModels.push(newItem);

                this.queueSortModels = this.queueSortModels.filter(item => item != oldItem);

                // Gán vào danh sách cũ
                this.oldSortModels = [...this.sortModels];

                // Load lại dữ liệu
                await this.reloadItems()
            },
            deep: true
        },
        /**
         * Clean filter models when filters change
         * 
         * Author: nlnhat (26/08/2023)
         */
        filterModels: {
            handler() {
                this.filterModelsClean = this.filterModels.filter(item => item && item != null);
            },
            deep: true
        },
        /**
         * Reload data when filters change
         * 
         * Author: nlnhat (26/08/2023)
         */
        filterModelsClean: {
            async handler() {
                await this.reloadItems();
                this.focusFirstItem();
            },
            deep: true
        },
        /**
         * Watch number of items
         */
        items: {
            handler() {
                this.focusedId = this.items.length > 0 ? this.focusedId : null;
            },
            deep: false
        },
        "unitStore.unitSelects": function () {
            const head = this.table.heads.find(head => head.name == "UnitName")
            head.filterConfig.selects = this.unitStore.unitSelects;
        },
        "warehouseStore.warehouseSelects": function () {
            const head = this.table.heads.find(head => head.name == "WarehouseCode")
            head.filterConfig.selects = this.warehouseStore.warehouseSelects;
        },
    },
    computed: {
        /**
         * Check all records in page are selected or not
         * 
         * Author: nlnhat (05/08/2023)
         * @return True if are selected, otherwise false
         */
        isCheckedAllComputed() {
            if (!this.items || this.items.length === 0) {
                return false;
            }
            return this.items.every(item => this.selectedItems.includes(item.ItemId));
        },
        /**
         * Check some records (has check but not all) or not
         * 
         * Author: nlnhat (05/08/2023)
         * @return True if are selected, otherwise false
         */
        isCheckSomeComputed() {
            const isIncludes = this.selectedItems.every(id =>
                this.items.some(item => item.ItemId == id));

            return (this.items
                && this.selectedItems.length > 0
                && this.selectedItems.length < this.items.length
                && isIncludes)
        },
        /**
         * Filter
         * 
         * Author: nlnhat (19/08/2023)
         */
        filterComputed() {
            const filter = {
                keySearch: this.keySearch,
                pagingModel: this.pagingModel,
                sortModels: this.sortModelsComputed,
                filterModels: this.filterModelsComputed,
            };
            return filter;
        },
        /**
         * Data sort models truyền đến server
         * 
         * Author: nlnhat (19/08/2023)
         */
        sortModelsComputed() {
            const sortModelsDto = this.queueSortModels.filter(item => item && item != null);
            return sortModelsDto;
        },
        /**
         * Data filter models truyền đến server
         * 
         * Author: nlnhat (25/08/2023)
         * @return Câu truy vấn sort
         */
        filterModelsComputed() {
            const filterModelsDto = this.filterModelsClean.map(item => ({
                columnName: item.column,
                logicType: item.logicType,
                logicName: item.logicName,
                compareType: item.compareType,
                compareName: item.compareName,
                filterValue: item.values[0].key,
            }));
            return filterModelsDto;
        },
        /**
         * Filter count object
         */
        filterCountComputed() {
            return {
                totalRecord: this.totalRecord,
                allRecord: this.allRecord,
            }
        },
    },
    methods: {
        /**
         * Khởi tạo, gán data
         */
        async initOnCreated() {
        },
        /**
         * Load data
         */
        async loadDataOnCreated() {
            this.reloadItems();
        },
        /**
         * Handle checkbox check all records on table in a page
         * 
         * Author: nlnhat (05/08/2023)
         */
        onChangeCheckAll() {
            // Uncheck all records in a page
            if (this.isCheckedAllComputed) {
                this.selectedItems = this.selectedItems.filter(id => {
                    return !this.items.some(item => item.ItemId === id);
                });
            }
            // Check all records in a page
            else {
                this.items.forEach(item => {
                    if (!this.selectedItems.includes(item.ItemId)) {
                        this.selectedItems.push(item.ItemId)
                    }
                })
            }
        },
        /**
         * Handle on click un select allk
         * 
         * Author: nlnhat (05/08/2023)
         */
        onClickUnselectAll() {
            this.selectedItems = []
        },
        /**
         * Check id in selected list or not
         * 
         * Author: nlnhat (05/08/2023)
         * @return True if selected, otherwise false
         */
        isSelected(id) {
            return this.selectedItems.includes(id)
        },
        /**
         * Get all items
         * 
         * Author: nlnhat (26/06/2023)
         */
        async getItems() {
            await this.loadingEffect(async () => {
                const response = await this.itemService.getAll();
                if (response?.status == this.$enums.status.ok)
                    this.items = response.data;
            })
        },
        /**
         * Get filtered items
         * 
         * Author: nlnhat (26/06/2023)
         */
        async filterItems() {
            try {
                const response = await this.itemService.filter(this.filterComputed);
                if (this.$cf.isSuccess(response)) {
                    this.items = response.Data.Data;
                    this.totalRecord = response.Data.TotalRecord;
                    this.allRecord = response.Data.AllRecord;
                    this.pagingModel.pageNumber = response.Data.PagingModel.PageNumber;
                    this.isFilterSuccess = true;
                } else {
                    this.items = [];
                    this.totalRecord = 0;
                    this.isFilterSuccess = false;
                    this.handleError(response);
                }
            } catch (error) {
                console.error(error);
                this.items = [];
                this.totalRecord = 0;
                this.isFilterSuccess = false;
            }
        },
        /**
         * Reload all 
         * 
         * Author: nlnhat (26/06/2023)
         */
        async reloadAll() {
            await this.loadingEffect(async () => {
                await Promise.all([
                    this.filterItems(),
                    this.getWarehouses(),
                    this.getUnits()
                ]);
            })
        },
        /**
         * Reload item
         * 
         * Author: nlnhat (26/06/2023)
         */
        async reloadItems() {
            await this.loadingEffect(this.filterItems);
        },
        /**
         * On click reload data button
         * 
         * Author: nlnhat (29/06/2023)
         */
        async onReloadData() {
            await this.reloadAll();
            if (this.isFilterSuccess) {
                this.showToastReloadSuccess();
                return true;
            }
        },
        /**
         * Delete one item
         * 
         * Author: nlnhat (26/06/2023)
         * @param {object} item Item object to delete
         */
        async deleteItem(item) {
            try {
                const itemId = item.ItemId;
                const itemCode = item.ItemCode;
                const itemName = item.ItemName;

                const oldIndex = this.items.indexOf(item);

                const response = await this.itemService.delete(itemId);
                const refDelete = this.$refs.tr.find(tr => tr.id == itemId);

                if (response?.status == this.$enums.status.ok) {
                    if (refDelete) {
                        refDelete.vanish();
                    }
                    setTimeout(async () => {
                        // Làm mới danh sách
                        this.selectedItems = this.selectedItems.filter(id => id != itemId);
                        this.items = this.items.filter(id => id != itemId);
                        await this.filterItems();

                        // Focus vào dòng mới
                        const length = this.items.length;
                        if (length > 0) {
                            const indexFocus = oldIndex < length ? oldIndex : length - 1;
                            this.focusedId = this.items[indexFocus].ItemId;
                            this.focusFocusedId();
                        }
                    }, 300);

                    // Hiện thông báo
                    const message
                        = `${this.$resources['vn'].deleted} ${this.$resources['vn'].item} <${itemCode} - ${itemName}>`;
                    this.showToastDeleteSuccess(message);
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Delete selected items
         * 
         * Author: nlnhat (26/06/2023)
         */
        async deleteItems() {
            await this.loadingEffect(async () => {
                try {
                    const response = await this.itemService.deleteMany(this.focusedIds);

                    if (response?.status == this.$enums.status.ok) {
                        const deletedCount = response.data;

                        // Nếu xoá được > 0 thì thông báo thành công
                        if (deletedCount > 0) {
                            this.showToastDeleteSuccess(`${this.$resources['vn'].deleted} ${deletedCount} ${this.$resources['vn'].item}`);

                            // Nếu không xoá được hết thì báo lỗi
                            if (deletedCount < this.focusedIds.length) {
                                const errorDeleteCount = this.focusedIds.length - deletedCount;
                                this.showToastDeleteError(`${this.$resources['vn'].cannotDelete} ${errorDeleteCount} ${this.$resources['vn'].item}`);
                            }

                            await this.filterItems();

                            // Focus vào dòng mới
                            const length = this.items.length;
                            if (length > 0) {
                                this.focusedId = this.items[0].ItemId;
                                this.focusedIds = [this.focusedId];
                                this.focusFocusedId();
                            }
                        }
                    }
                } catch (error) {
                    console.error(error)
                }
            })
        },

        /**
         * On click delete
         * 
         * Author: nlnhat (29/06/2023)
         */
        onClickDelete() {
            if (this.focusedIds.length <= 1) {
                this.handleDeleteItem();
            }
            else {
                this.handleDeleteItems();
            }
        },
        /**
         * Handle delete one item
         * 
         * Author: nlnhat (29/06/2023)
         */
        handleDeleteItem() {
            try {
                const itemId = this.focusedId;
                const item = this.items.find(item => item.ItemId == itemId);
                if (item) {
                    const itemCode = item.ItemCode;
                    const itemName = item.ItemName;

                    this.deleteConfirmDialog.content =
                        `${this.$resources['vn'].deleteConfirm} ${this.$resources['vn'].item} <${itemCode} - ${itemName}> ${this.$resources['vn'].questionNo}`;

                    this.deleteConfirmDialog.onClickDelete = () => {
                        this.deleteConfirmDialog.isShowed = false;
                        this.deleteItem(item);
                    }
                    this.deleteConfirmDialog.onClickCancel = () => {
                        this.deleteConfirmDialog.isShowed = false;
                    }
                    this.deleteConfirmDialog.isShowed = true;
                    this.focusOnButton();
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle delete many items
         * 
         * Author: nlnhat (29/06/2023)
         */
        handleDeleteItems() {
            try {
                const oldLength = this.focusedIds.length;

                this.deleteConfirmDialog.content = `${this.$resources['vn'].deleteConfirm} ${oldLength} ${this.$resources['vn'].selectedItems}`;
                this.deleteConfirmDialog.onClickDelete = () => {
                    this.deleteConfirmDialog.isShowed = false;
                    this.deleteItems();
                }
                this.deleteConfirmDialog.onClickCancel = () => {
                    this.deleteConfirmDialog.isShowed = false;
                }
                this.deleteConfirmDialog.isShowed = true;
                this.focusOnButton();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Show empty item form to add new 
         * 
         * Author: nlnhat (26/06/2023)
         */
        showEmptyItemForm() {
            // this.focusedId = null;
            this.formMode = this.$enums.formMode.create;
            this.isShowedItemForm = true;
        },
        /**
         * Show item form
         * 
         * Author: nlnhat (26/06/2023)
         * 
         * @param {*} item Selected item
         */
        showItemForm() {
            this.formMode = this.$enums.formMode.update;
            this.isShowedItemForm = true;
        },
        /**
         * Duplicate item
         * 
         * Author: nlnhat (29/08/2023)
         */
        duplicateItem() {
            this.formMode = this.$enums.formMode.duplicate;
            this.isShowedItemForm = true;
        },
        /**
         * Hide item form
         * 
         * Author: nlnhat (26/06/2023)
         */
        hideItemForm() {
            this.isShowedItemForm = false;
            this.focusFocusedId();
        },
        /**
         * Show toast message after reload
         * 
         * Author: nlnhat (29/06/2023)
         */
        showToastReloadSuccess() {
            const content = this.$resources['vn'].reloadedData;
            this.$emitter.emit("showToastSuccess", content);
        },
        /**
         * Show toast message after delete success
         * 
         * Author: nlnhat (29/06/2023)
         * @param {string} content Message added
         */
        showToastDeleteSuccess(content) {
            this.$emitter.emit("showToastSuccess", content);
        },
        /**
         * Show toast message after delete error
         * 
         * Author: nlnhat (29/06/2023)
         * @param {string} content Message added
         */
        showToastDeleteError(content) {
            this.$emitter.emit("showToastError", content);
        },
        /**
         * Re render form
         *
         * Author: nlnhat (29/06/2023)
         */
        reRenderForm() {
            this.formMode = this.$enums.formMode.create;
            this.itemUpdate = {}
            this.formKey += 1;
        },
        /**
         * Handle event click on button Excel
         * 
         * Author: nlnhat (22/08/2023)
         */
        async onClickExcel() {
            await this.exportToExcel();
        },
        /**
         * Handle export to excel
         * 
         * Author: nlnhat (22/08/2023)
         */
        async exportToExcel() {
            try {
                const response = await this.itemService.export(this.filterComputed);
                if (response?.status == this.$enums.status.ok) {
                    const blob = new Blob(
                        [response.data],
                        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
                    );
                    const fileName = `${this.$resources['vn'].itemListFileName}_${formatDate(new Date(), 'ddMMyyyy')}.xlsx`;
                    await this.download(blob, fileName);
                    return true;
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle click feedback
         * 
         * Author: nlnhat (25/08/2023)
         */
        onClickFeedback() {
            this.openUrl(window.externalUrl.feedback);
        },
        /**
         * Handle shortcut keys
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            try {
                const code = this.$enums.keyCode;

                // Handle shortcut keys on delete confirm dialog
                if (this.deleteConfirmDialog.isShowed == true) {
                    this.handleShortKeyDeleteDialog(event);
                }
                // Insert: Thêm mới
                else if ((event.ctrlKey && event.keyCode == code.digit1) || (event.keyCode == code.insert)) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.showEmptyItemForm();
                }
                // Ctrl + F || Ctrl + F3: Focus ô tìm kiếm
                else if ((event.ctrlKey && event.keyCode == code.f) || event.keyCode == code.f3) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.focusOnSearchBox();
                }
                // Ctrl + Y: Reload
                else if (event.ctrlKey && event.keyCode == code.y) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.onReloadData();
                }
                // Ctrl + D || Delete: Delete
                else if (((event.ctrlKey && event.keyCode == code.d) || event.keyCode == code.delete)
                    && this.focusedId != null) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.onClickDeleteItem();
                }
                // Ctrl + S || Thống kệ
                else if (event.ctrlKey && event.keyCode == code.s) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.showItemStat();
                }
                // Ctrl + I || Nhập khẩu
                else if (event.ctrlKey && event.keyCode == code.i) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.showItemImport();
                }
                // Ctrl + E: Export to excel
                else if (event.ctrlKey && event.keyCode == code.e) {
                    event.preventDefault();
                    event.stopPropagation();
                    this.exportToExcel();
                }
                // Ctrl + A: Chọn tất
                // else if (event.ctrlKey && event.keyCode == code.a) {
                //   event.preventDefault();
                //   event.stopPropagation();
                //   this.onChangeCheckAll();
                // }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle shortcut keys on delete confirm dialog
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} event Keydown event
         */
        handleShortKeyDeleteDialog(event) {
            const code = this.$enums.keyCode;
            if (event.keyCode == code.enter) {
                event.stopPropagation();
                event.preventDefault();
                this.deleteConfirmDialog.onClickDelete();
            }
            else if (event.keyCode == code.n || event.keyCode == code.esc) {
                event.stopPropagation();
                event.preventDefault();
                this.deleteConfirmDialog.onClickCancel();
            }
        },
        /**
         * Prevent double click
         */
        preventDoubleClick(event) {
            event.stopPropagation();
        },
        /**
         * Focus vào ref lúc đầu
         */
        firstFocus() {
            this.$nextTick(() => {
                if (this.$refs[this.refFocusFirst]) {
                    this.$refs[this.refFocusFirst].focus();
                }
            })
        },
        /**
         * Focus on search box
         * 
         * Author: nlnhat (25/08/2023)
         */
        focusOnSearchBox() {
            this.$nextTick(() => {
                if (this.$refs['refSearchBox']) {
                    this.$refs['refSearchBox'].focus();
                }
            })
        },
        /**
         * Remove filter in a column
         * 
         * Author: nlnhat (25/08/2023)
         * @param {number} index Index of column 
         */
        removeFilterModel(index) {
            this.filterModels[index] = null;
        },
        /**
         * Remove all filters in columns
         * 
         * Author: nlnhat (25/08/2023)
         */
        removeAllFilters() {
            this.filterModels = this.filterModels.fill(null);
        },
        /**
         * Focus on button
         * 
         * Author: nlnhat (25/08/2023)
         */
        focusOnButton() {
            this.$nextTick(() => {
                this.$refs.buttonFocus.focus();
            })
        },
        /**
         * Focus on next row in table
         * 
         * Author: nlnhat (25/08/2023)
         * @param {number} index Index of row 
         */
        focusNextRow(index) {
            let newIndex = index + 1;
            if (newIndex >= this.items.length)
                newIndex = 0;
            this.focusRow(newIndex);
        },
        /**
         * Focus on previous row in table
         * 
         * Author: nlnhat (25/08/2023)
         * @param {number} index Index of row 
         */
        focusPreviousRow(index) {
            let newIndex = index - 1;
            if (newIndex < 0)
                newIndex = this.items.length - 1;
            this.focusRow(newIndex);
        },
        /**
         * Focus on a row by index
         * 
         * Author: nlnhat (08/08/2023)
         * @param {number} index Index of row 
         */
        focusRow(index) {
            const refFocus = this.$refs.tr.find(tr => tr.index == index);
            if (refFocus) {
                this.focusedIds = [refFocus.id];
                refFocus.focus();
            }
        },
        /**
         * Focus on a row by id
         * 
         * Author: nlnhat (08/08/2023)
         * @param {string} id Id of row 
         */
        focusById(id) {
            const refFocus = this.$refs.tr.find(tr => tr.id == id);
            if (refFocus)
                refFocus.focus();
        },
        /**
         * Focus on a row has id == focusedId
         * 
         * Author: nlnhat (08/08/2023)
         */
        focusFocusedId() {
            const refFocus = this.$refs.tr.find(tr => tr.id == this.focusedId);
            if (refFocus) {
                refFocus.focus();
            }
        },
        /**
         * Update focused id
         * 
         * Author: nlnhat (08/08/2023) 
         * @param {*} id Focused id
         */
        updateFocusedId(id) {
            this.focusedId = id;
        },
        /**
         * Update focused ids
         * 
         * Author: nlnhat (08/08/2023) 
         * @param {*} ids Focused ids
         */
        updateFocusedIds(ids) {
            this.focusedIds = ids ?? [];
        },
        /**
         * Select many from focused id to id
         * 
         * Author: nlnhat (08/08/2023) 
         * @param {*} id End id selected
         */
        selectMany(startId, endId) {
            const ids = this.items.map(item => item.ItemId);
            let startIndex = Math.max(ids.indexOf(startId), 0);
            let endIndex = ids.indexOf(endId);
            let newIds = [];

            if (startIndex < endIndex) {
                newIds = ids.slice(startIndex, endIndex + 1);
            }
            else {
                newIds = ids.slice(endIndex, startIndex + 1);
            }

            newIds = newIds.filter(id => !this.focusedIds.includes(id));
            this.focusedIds.push(...newIds);
        },
        /**
         * Handle delete on a row
         * 
         * Author: nlnhat (04/08/2023)
         * @param {*} itemId Id of item on this row
         */
        handleDeleteOnRow(itemId) {
            this.onClickDeleteItem(itemId);
        },
        /**
         * Update page property from pagination component
         * 
         * Author: nlnhat (25/08/2023)
         * @param {*} page Page object from pagination component
         */
        async updatePage(page) {
            this.pagingModel = page;
            await this.reloadItems();
            // this.focusFirstItem();
        },
        /**
         * Focus vào nguyên vật liệu đầu tiên khi sang trang khác hoặc tìm kiếm
         *
         * Author: nlnhat (14/09/2023)
         */
        focusFirstItem() {
            if (this.items?.length > 0 && this.focusedIds?.length <= 1) {
                const id = this.items[0].ItemId;
                this.focusedId = id;
                this.focusedIds = [id];

            }
        },
        /**
         * Show ItemStat
         *
         * Author: nlnhat (08/09/2023)
         */
        showItemStat() {
            this.isShowItemStat = true;
        },
        /**
         * Close ItemStat
         *
         * Author: nlnhat (08/09/2023)
         */
        closeItemStat() {
            this.isShowItemStat = false;
        },
        /**
         * Show ItemImport
         *
         * Author: nlnhat (08/09/2023)
         */
        showItemImport() {
            this.isShowItemImport = true;
        },
        /**
         * Close ItemImport
         *
         * Author: nlnhat (08/09/2023)
         */
        closeItemImport() {
            this.isShowItemImport = false;
        },
        /**
         * Click vào nút tạo mới
         */
        clickCreate() {
        }
    }
}
</script>