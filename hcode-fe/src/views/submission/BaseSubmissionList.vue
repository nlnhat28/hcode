<script>
import BaseList from "@/components/base/BaseList.vue";
import { submissionService } from "@/services/services.js";
import problemEnum from "@/enums/problem-enum.js";
import contestEnum from "@/enums/contest-enum.js";
import { useLanguageStore } from "@/stores/stores";
import { mapStores, mapState } from 'pinia';

export default {
    name: "BaseSubmissionList",
    extends: BaseList,
    emits: [
        'selected',
    ],
    props: {
        parentId: {
            type: [String],
            default: null,
        }
    },
    data() {
        return {
            // documentTitle: this.$t("problem.problemList"),
            hasBuildDocumentTitle: false,
            itemIdKey: "SubmissionId",
            problemEnum: problemEnum,
            contestEnum: contestEnum,
            /**
             * Các cột
             */
            columns: [],
            languages: [],
        }
    },
    computed: {
        ...mapStores(useLanguageStore),
    },
    methods: {
        /**
         * Override
         * 
         */
        initOnCreated() {
            this.itemService = submissionService;
        },
        /**
         * Lấy dữ liệu
         * @override
         */
        async customLoadDataOnCreated() {
            try {
                await this.getLanguages();
                let selectsLanguage = this.languages.map(l => ({
                    key: l.LanguageId,
                    name: l.LanguageName,
                }));
                this.$cf.addSelectsToColumn(selectsLanguage, this.columns, 'LanguageId');
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Lấy danh sách language
         */
        async getLanguages() {
            if (this.$cf.isEmptyArray(this.languageStore.languages)) {
                try {
                    const res = await languageService.getAll();
                    if (this.$res.isSuccess(res)) {
                        this.languages = this.$cf.cloneDeep(res.Data);
                        this.languageStore.setLanguages(res.Data);
                    }
                }
                catch (error) {
                    console.error(error);
                }
            }
            else {
                this.languages = this.$cf.cloneDeep(this.languageStore.languages);
            }
        },
        /**
         * Chọn item
         */
        onSelect(item) {
            this.$emit('selected', item);
        }
    }
}
</script>