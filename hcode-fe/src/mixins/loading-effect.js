/**
 * Làm hiệu ứng loading
 */
export default {
    data() {
        return {
            isLoading: false,
        };
    },
    methods: {
        async loadingEffect(func) {
            try {
                this.isLoading = true;
                await func();
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
    },
};
