#include <stdio.h>
#include <string.h>

#define MAX_TOKENS 100
#define MAX_STRING_LENGTH 2560

void splitString(char *inputString, char *delimiter, char **tokens, int *tokenCount) {
    char *token = strtok(inputString, delimiter);
    int count = 0;

    // Tokenize the string
    while (token != NULL && count < MAX_TOKENS) {
        tokens[count++] = token;
        token = strtok(NULL, delimiter);
    }

    *tokenCount = count;
}

int main() {
    char inputString[MAX_STRING_LENGTH];
    fgets(inputString, MAX_STRING_LENGTH, stdin);

    // Remove newline character from inputString
    inputString[strcspn(inputString, "\n")] = '\0';

    char *delimiter = "|";
    char *tokens[MAX_TOKENS];
    int tokenCount = 0;

    // Split the string and get tokens
    splitString(inputString, delimiter, tokens, &tokenCount);

    // Print the tokens
    for (int i = 0; i < tokenCount; i++) {
        printf("Token %d: %s\n", i + 1, tokens[i]);
    }

    return 0;
}
