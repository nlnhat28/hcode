<script>
import { validate } from "@/mixins/mixins";

export default {
    name: 'BaseItem',
    mixins: [validate],
    props: {
        /**
         * Index
         */
        index: {
            type: Number,
        },
        /**
         * Thông tin item (props)
         */
        item: {
            type: Object,
            default: {}
        },
        /**
         * Danh sách tất cả item
         */
        allItems: {
            type: Array,
            default: []
        }
    },
    data() {
        return {
            instance: {},
        }
    },
    emits: ['onDelete'],
    created() {
        this.instance = this.item;
        this.initOnCreated();
    },
    mounted() {
        this.refs = [];
    },
    watch: {
        item() {
            this.instance = this.item;
        },
    },
    computed: {
    },
    methods: {
        initOnCreated() {
        },
        /**
         * Click xoá
         */
        clickDelete() {
            this.$emit('onDelete', this.instance);
        },
    }
}
</script>