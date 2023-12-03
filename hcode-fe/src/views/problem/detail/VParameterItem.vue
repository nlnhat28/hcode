<template>
    <div class="parameter-item">
        <div class="parameter__order flex-center">
            1
        </div>
        <div class="parameter__data-type flex-center dark">
            <v-combobox
                v-model="selectedDataType"
                optionLabel="name"
                :options="$cv.enumToSelects($enums.dataType)"
            ></v-combobox>
        </div>
        <div class="parameter__name flex-center dark">
            <v-input-text></v-input-text>
        </div>
        <div class="parameter__function flex-center">
            <v-icon
                icon="far fa-circle-xmark"
                color="error"
                applyPointer
            />
        </div>
    </div>
</template>
<script>
export default {
    name: 'VParameterItem',
    data() {
        return {
            selectedDataType: this.$cv.enumToSelects(this.$enums.dataType)[0],
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
    column-gap: 16px;
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

.parameter__data-type,
.parameter__name {
    flex: 1;
    height: 100%;
}</style>