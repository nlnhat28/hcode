<?php

define('MAX_TOKENS', 100);
define('MAX_STRING_LENGTH', 2560);

function splitString($inputString, &$tokenCount) {
    $delimiter = "|";
    $tokens = [];

    // Use explode to split the string
    $splitTokens = explode($delimiter, $inputString, MAX_TOKENS);

    // Add tokens to the array
    foreach ($splitTokens as $token) {
        $tokens[] = $token;
    }

    $tokenCount = count($tokens);

    return $tokens;
}

$inputString = stream_get_contents(STDIN);

$tokenCount = 0;
$tokens = splitString($inputString, $tokenCount);

// Print the tokens
for ($i = 0; $i < $tokenCount; $i++) {
    echo "Token " . ($i + 1) . ": " . $tokens[$i] . PHP_EOL;
}

// No explicit cleanup needed for arrays in PHP
?>
