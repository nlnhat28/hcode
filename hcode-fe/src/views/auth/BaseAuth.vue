<script>
import BaseForm from "@/components/base/BaseForm.vue";
import { authService, accountService } from "@/services/services.js";
import { useAuthStore, useAccountStore } from "@/stores/stores.js";
import { mapStores } from 'pinia';
import authEnum from "@/enums/auth-enum.js";

export default {
    name: "BaseAuth",
    extends: BaseForm,
    data() {
        return {
            instanceService: authService,
            authService: authService,
            accountService: accountService,
            authEnum: authEnum,
        }
    },
    computed: {
        /**
         * Store
         */
        ...mapStores(useAuthStore),
        ...mapStores(useAccountStore),
    },
}
</script>