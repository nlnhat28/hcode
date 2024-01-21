function stringInArray(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        const m = s.trim().match(/^\[(.+)\]$/);
        if (m) return m[1];
        throw exp;
    } catch { throw exp; }
}
function stringToInt(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        const i = parseInt(s.trim());
        return !isNaN(i) ? i : exp;
    } catch { throw exp; }
}
function stringToInts(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        return stringInArray(s).split(",").map(i => stringToInt(i));
    } catch { throw exp; }
}
function stringToDouble(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        const i = parseFloat(s.trim());
        return !isNaN(i) ? i : exp;
    } catch { throw exp;    }
}
function stringToDoubles(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        return stringInArray(s).split(",").map(i => stringToDouble(i));
    } catch { throw exp; }
}
function stringToStrings(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        return stringInArray(s).split(",").map(i => i?.toString());
    } catch { throw exp; }
}
function stringToBool(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        const i = JSON.parse(s.trim().toLowerCase());
        return typeof i === 'boolean' ? i : exp;
    } catch { throw exp; }
}
function stringToBools(s) {
    const exp = new Error(`Invalid argument: ${s}`);
    try {
        return stringInArray(s).split(",").map(i => stringToBool(i));
    } catch { throw exp; }
}
function splitString(inputString, delimiter) {
    return inputString.split(delimiter);
}
function toText(i) {
    return i == null ? "null" : i.toString();
}
function arrayToText(r) {
    if (r == null) return "null";
    return `[${r.map(i => toText(i)).join(",")}]`;
}

function main() {
    let inputString = "";
    process.stdin.on("data", (chunk) => {
        inputString += chunk.toString();
    });
    process.stdin.on("end", () => {
        const r =  stringToInts("[1 , 2, 3]")
        console.log(arrayToText(r));
    });
}
if (require.main === module) {
    main();
}