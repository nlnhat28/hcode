<template>
    <div class="reaction-wrapper">
        <!-- <div class="reaction__item">
            {{ $fm.formatDecimal(rate, 1) }}
            <v-icon
                icon='far fa-star'
                v-if="rate"
            ></v-icon>
        </div>
        <div class="reaction__item">
            {{ $cv.numberToSuffix(comment) }}
            <v-icon icon='far fa-comment'></v-icon>
        </div> -->
        <div class="reaction__item">
            {{ $cv.numberToSuffix(seen) }}
            <v-icon
                icon='far fa-eye'
                color="light"
            ></v-icon>
        </div>
        <div class="reaction__item">
            {{ $cv.rateToPercent(accepted, 1, '0%') }}
            <v-icon
                icon='far fa-circle-check'
                color="success"
            ></v-icon>
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
            default: null,
        },
        /**
         * Đã xem
         */
        accepted: {
            type: [String, Number],
            default: null,
        },
        /**
         * Đã xem
         */
        wrong: {
            type: [String, Number],
            default: null,
        },
        /**
         * Đã xem
         */
        seen: {
            type: [String, Number],
            default: null,
        }
    }
}
</script>
<style scoped>@import './reaction.css';</style>