<template>
    <div class="parameter-item">
        <div class="parameter__order flex-center">
            {{ instance.ParameterOrder }}
        </div>
        <div class="flex align-center justify-between col-gap-12 flex-1 pr-12">
            <div class="parameter__data-type flex-center dark">
                <v-input-text
                    ref="refOutputType"
                    isReadOnly
                    :value="selectedDataType?.name"
                    :applyPlaceholder="false"
                >
                </v-input-text>
            </div>
            <div class="parameter__name flex-center dark">
                <v-input-text
                    ref="refParameterName"
                    v-model="instance.ParameterName"
                    isReadOnly
                    :applyPlaceholder="false"
                ></v-input-text>
            </div>
        </div>
    </div>
</template>
<script>
import { validate } from "@/mixins/mixins";
import problemEnum from "@/enums/problem-enum";

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
    },
    data() {
        return {
            dataTypes: [],
            selectedDataType: null,
            instance: {},
        }
    },
    created() {
        this.assignInstance();
        this.dataTypes = this.$cv.enumToSelects(problemEnum.dataType);
        this.selectedDataType = this.$cv.enumKeyToSelected(this.instance.DataType, this.dataTypes, 0);
    },
    watch: {
        parameter() {
            this.assignInstance();
        },
        index() {
            this.instance.ParameterOrder = this.index + 1;
        },
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
         * 
         */

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
    padding-right: 12px
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