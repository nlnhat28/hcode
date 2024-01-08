/**
 * Right click outside event
 *
 * Author: nlnhat (27/07/2023)
 */
const contextmenuOutside = {
    mounted(el, { value }) {
        const contextmenuOutsideEvent = (event) => {
            if (!(el === event.target || el.contains(event.target))) {
                value(event);
            }
        };
        el._contextmenuOutsideEvent = contextmenuOutsideEvent;
        document.body.addEventListener("contextmenu", contextmenuOutsideEvent);
    },
    unmounted(el) {
        document.body.removeEventListener(
            "contextmenu",
            el._contextmenuOutsideEvent
        );
    },
};

export default contextmenuOutside;
