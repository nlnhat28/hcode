<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { authService } from "@/services/services.js";
import { useAuthStore } from "@/stores/stores.js";
import { mapStores } from 'pinia';
import authEnum from "@/enums/auth-enum.js";

export default {
    name: "Auth",
    extends: BaseForm,
    data() {
        return {
            instanceService: authService,
        }
    },
    computed: {
        /**
         * Store
         */
        ...mapStores(useAuthStore),
    },
}
</script>