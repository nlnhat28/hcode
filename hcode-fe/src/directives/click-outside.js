/**
 * Click outside to hide element
 *
 * Author: nlnhat (27/07/2023)
 */
const clickOutside = {
    mounted(el, { value }) {
        const clickOutsideEvent = (event) => {
            if (!(el === event.target || el.contains(event.target))) {
                value(event);
            }
        };
        el._clickOutsideEvent = clickOutsideEvent;
        document.body.addEventListener("click", clickOutsideEvent);
    },
    unmounted(el) {
        document.body.removeEventListener("click", el._clickOutsideEvent);
    },
};

export default clickOutside;
