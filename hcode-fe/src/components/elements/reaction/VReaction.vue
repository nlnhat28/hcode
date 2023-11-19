<template>
    <div class="reaction-wrapper">
        <div class="reaction__item">
            {{ rate }}
            <v-icon icon='far fa-star'></v-icon>
        </div>
        <div class="reaction__item">
            {{ $cv.numberToSuffix(comment) }}
            <v-icon icon='far fa-comment'></v-icon>
        </div>
        <div class="reaction__item">
            {{ $cv.numberToSuffix(seen)  }}
            <v-icon icon='far fa-eye'></v-icon>
        </div>
    </div>
</template>
<script>
export default {
    name: 'VReaction',
    props: {
        /**
         * Đánh giá
         */
        rate: {
            type: [String, Number],
            default: null,
        },
        /**
         * Lượt bình luận
         */
        comment: {
            type: [String, Number],
            default: 0,
        },
        /**
         * Đã xem
         */
        seen: {
            type: [String, Number],
            default: 0,
        }
    }
}
</script>
<style scoped>
@import './reaction.css';
</style>