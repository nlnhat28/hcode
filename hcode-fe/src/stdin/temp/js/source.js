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
function printString(i) {
    console.log(i);
}
function printInt(i) {
    console.log(i.toString());
}
function printDouble(i) {
    console.log(i.toString());
}
function printBool(i) {
    console.log(i.toString());
}
function printStrings(a) {
    console.log(`[${a.join(",")}]`);
}
function printInts(a) {
    console.log(`[${a.join(",")}]`);
}
function printDoubles(a) {
    console.log(`[${a.join(",")}]`);
}
function printBools(a) {
    console.log(`[${a.join(",")}]`);
}
function main() {
    let inputString = "";
    process.stdin.on("data", (chunk) => {
        inputString += chunk.toString();
    });
    process.stdin.on("end", () => {
        const a = splitString(inputString, "|");
        console.log({output}(solve({args})));
    });
}
{userCode}
if (require.main === module) {
    main();
}