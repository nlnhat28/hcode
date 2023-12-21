<template>
    <div class="parameter-item">
        <div class="parameter__order flex-center">
            {{ index + 1 }}
        </div>
        <div class="flex align-center justify-between col-gap-12 flex-1">
            <div class="parameter__data-type flex-center dark">
                <v-combobox
                    v-model="selectedDataType"
                    optionLabel="name"
                    :placeholder="$t('problem.dataType')"
                    :options="$cv.enumToSelects($enums.dataType)"
                ></v-combobox>
            </div>
            <div class="parameter__name flex-center dark">
                <v-input-text
                    ref="refParameterName"
                    v-model="instance.ParameterName"
                    isRequired
                    hasClear
                    :validate="validateParameterName"
                    :label="$t('problem.parameterName')"
                ></v-input-text>
            </div>
        </div>
        <div class="parameter__function flex-center">
            <v-icon
                icon="far fa-circle-xmark"
                color="danger"
                applyPointer
                @click="clickDelete"
            />
        </div>
    </div>
</template>
<script>
import { validate } from "@/mixins/mixins";

export default {
    name: 'TestcaseItem',
    mixins: [validate],
    props: {
        /**
         * Index
         */
        index: {
            type: Number,
        },
        /**
         * Thông tin parameter
         */
        parameter: {
            type: Object,
            default: {}
        },
        /**
         * Danh sách parameter khác
         */
        parameters: {
            type: Array,
            default: []
        }
    },
    data() {
        return {
            selectedDataType: this.$cv.enumToSelects(this.$enums.dataType)[0],
            instance: {},
        }
    },
    emits: ['onDelete'],
    created() {
        this.instance = this.parameter;
    },
    mounted() {
        this.refs = [this.$refs['refParameterName']];
    },
    watch: {
        parameter() {
            this.instance = this.parameter;
        },
        parameters: {
            handler() {
                if (!this.$cf.isEmptyString(this.instance.ParameterName)) {
                    this.checkValidate();
                }
            },
            deep: true,
        }
    },
    computed: {
        /**
         * Các tên parameter
         */
        parameterNames() {
            if (this.parameters) {
                const names = this.parameters.map(param => param.ParameterName);
                return names;
            }
            return [];
        }
    },
    methods: {
        /**
         * Click xoá
         */
        clickDelete() {
            this.$emit('onDelete', this.parameter);
        },
        /**
         * Hàm validate refParameterName
         */
        validateParameterName(label, value) {
            if (!this.$cf.isEmptyString(this.instance.ParameterName)
                && this.parameterNames.filter(item => item == value).length > 1) {
                const errorMessage = this.$t('problem.usedParameterName', { name: value });
                return errorMessage;
            };
            return null;
        },
        /**
         * Tạo tên tham số tự động
         */
        generateParameterName() {
            if (this.$cf.isEmptyString(this.instance.ParameterName)) {

            }
        }
    }
}
</script>
<style scoped>
.parameter-item {
    width: 100%;
    height: 60px;
    min-height: 60px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: var(--tpr-300);
    border-radius: 8px;
    transition: background-color 0.2s;
}

.parameter-item:focus-within {
    background-color: var(--tpr-200);
}

.parameter-item:hover {
    background-color: var(--tpr-100);
}

.parameter__order,
.parameter__function {
    width: 56px;
    height: 100%;
}

.parameter__order {
    font-weight: 700;
}

.parameter__data-type,
.parameter__name {
    flex: 1;
    height: 100%;
}
</style>