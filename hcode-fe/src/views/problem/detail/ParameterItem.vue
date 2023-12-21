<template>
    <div class="parameter-item">
        <div class="parameter__order flex-center">
            {{ instance.ParameterOrder }}
        </div>
        <div class="flex align-center justify-between col-gap-12 flex-1">
            <div class="parameter__data-type flex-center dark">
                <v-combobox
                    v-model="selectedDataType"
                    optionLabel="name"
                    :placeholder="$t('problem.dataType')"
                    :options="dataTypes"
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
    name: 'ParameterItem',
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
            dataTypes: [],
            selectedDataType: null,
            instance: {},
        }
    },
    emits: ['onDelete'],
    created() {
        this.assignInstance();
        this.dataTypes = this.$cv.enumToSelects(this.$enums.dataType);
        this.selectedDataType = this.$cv.enumKeyToSelected(this.instance.DataType, this.dataTypes, 0);
    },
    mounted() {
        this.refs = [this.$refs['refParameterName']];
        this.generateParameterName();
    },
    watch: {
        parameter() {
            this.assignInstance();
        },
        parameters: {
            handler() {
                if (!this.$cf.isEmptyString(this.instance.ParameterName)) {
                    this.checkValidate();
                }
            },
            deep: true,
        },
        index() {
            this.instance.ParameterOrder = this.index + 1;
        },
        selectedDataType: {
            handler() {
                this.instance.DataType = this.$cv.selectedToEnumKey(this.selectedDataType);
            },
            deep: true,
        },
        "instance.ParameterName"() {
            this.generateParameterName();
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
         * Gán instance
         */
        assignInstance() {
            this.instance = this.parameter;
            this.instance.ParameterOrder = this.index + 1;
        },
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
                let baseName = '';
                let dataTypeEnum = this.$enums.dataType;

                switch (this.instance.DataType) {
                    case dataTypeEnum.string.value:
                    case dataTypeEnum.int.value:
                    case dataTypeEnum.decimal.value:
                    case dataTypeEnum.bool.value:
                        baseName = this.selectedDataType.name;
                        break;

                    case dataTypeEnum.strings.value:
                    case dataTypeEnum.ints.value:
                    case dataTypeEnum.decimals.value:
                        baseName = `list${this.$cf.upperFirstChar(this.selectedDataType.name).replace('[]','')}`;
                        break;
                };

                let i = 1;
                while (i < 10) {
                    let name = `${baseName}${i}`;

                    if (this.parameterNames && !this.parameterNames.includes(name)) {
                        this.instance.ParameterName = name;
                        break;
                    }
                    i++;
                }
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
    background-color: var(--dark-400);
    border-radius: 8px;
    transition: background-color 0.2s;
}

.parameter-item:focus-within {
    background-color: var(--dark-300);
}

.parameter-item:hover {
    background-color: var(--dark-200);
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