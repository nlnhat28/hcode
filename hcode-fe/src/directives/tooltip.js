/**
 * Tooltip
 *
 * Author: nlnhat (27/07/2023)
 */
const tooltip = {
    mounted(el, binding) {
        init(el, binding);
    },
    updated(el, binding) {
        init(el, binding);
    },
};

function init(el, binding) {
    let position = binding.arg || "top";
    let content = binding.value || "";
    el.setAttribute("position", position);
    el.setAttribute("tooltip", content);
}

export default tooltip;
