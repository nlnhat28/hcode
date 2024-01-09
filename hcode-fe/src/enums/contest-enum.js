import { t } from "@/i18n/i18n.js";

const contestEnum = {
    contestFilter: {
        public: {
            value: 1,
            label: t("contest.filter.public"),
        },
        private: {
            value: 2,
            label: t("contest.filter.private"),
        },
        joined: {
            value: 2,
            label: t("contest.filter.joined"),
        },
    },
    contestState: {
        pending: {
            value: 1,
            label: t("contest.contestState.pending"),
        },
        goingOn: {
            value: 2,
            label: t("contest.contestState.goingOn"),
        },
        finish: {
            value: 2,
            label: t("contest.contestState.finish"),
        },
    },
};

export default contestEnum;