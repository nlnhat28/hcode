/**
 * Format string. Vd: var a = "Năm {0}" => a.format(2023) = "Năm 2023"
 */
String.prototype.format = function () {
    var a = this;
    for (var k in arguments) {
        a = a.replace(new RegExp("\\{" + k + "\\}", "g"), arguments[k]);
    }
    return a;
};