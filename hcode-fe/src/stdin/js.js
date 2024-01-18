const MAX_TOKENS = 100;
const MAX_STRING_LENGTH = 2560;

function splitString(inputString, delimiter) {
    return inputString.split(delimiter, MAX_TOKENS - 1);
}

function main() {
    let inputString = "";

    process.stdin.on("data", (chunk) => {
        inputString += chunk.toString();
    });

    process.stdin.on("end", () => {
        const delimiter = "|";
        const tokens = splitString(inputString, delimiter);

        // Print the tokens
        tokens.forEach((token, i) => {
            console.log(`Token ${i + 1}: ${token}`);
        });
    });
}

if (require.main === module) {
    main();
}
