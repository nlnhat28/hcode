<template>
    <v-dialog
        class="v-dialog"
        v-model:visible="isVisible"
        modal
        :header="headerComputed"
        :draggable="false"
        @hide="close"
        style="width: 600px;"
    >
        <template #closeIcon>
            <v-icon icon="far fa-circle-xmark" />
        </template>
        <div class="v-dialog__content">
            <div class="w-full flex-column row-gap-20 p-20">
                <v-code-mirror
                    v-model="instance.Solution"
                    :language="instance.SolutionLanguage"
                ></v-code-mirror>
            </div>
        </div>
        <template #footer>
            <v-mask-loading v-if="isLoading" />
            <v-button-container justifyContent="space-between">
                <!-- Các nút bên trái -->
                <div>
                </div>
                <!-- Các nút bên phải -->
                <div class="flex col-gap-8">
                    <!-- Đóng -->
                    <v-button
                        v-if="!0"
                        outlined
                        :label="$t('com.cancel')"
                        @click="close"
                    ></v-button>
                </div>
            </v-button-container>
        </template>
    </v-dialog>
</template>

<script>
import BaseFormDialog from '@/components/base/BaseFormDialog.vue';

export default {
    name: "SubmissionDialog",
    extends: BaseFormDialog,
    data() {
        return {
        }
    },
    computed: {
        headerComputed() {
            return '';
        },
    },
    mounted() {
    },
    methods: {
    }
};
</script>
<style scoped></style>