#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#define MAX_TOKENS 100
#define MAX_STRING_LENGTH 2560
char* stringInArray(char* s) {
    char* exp = "Invalid argument";
    char* result = NULL;
    char* trimmed = strdup(s);
    trimmed[strcspn(trimmed, "\r\n")] = 0;

    if (trimmed[0] == '[' && trimmed[strlen(trimmed) - 1] == ']') {
        result = malloc(strlen(trimmed) - 1);
        strncpy(result, trimmed + 1, strlen(trimmed) - 2);
        result[strlen(trimmed) - 2] = '\0';
    } else {
        fprintf(stderr, "%s: %s\n", exp, s);
        exit(1);
    }

    free(trimmed);
    return result;
}
int stringToInt(char* s) {
    char* exp = "Invalid argument";
    int result;
    if (sscanf(s, "%d", &result) == 1) {
        return result;
    } else {
        fprintf(stderr, "%s: %s\n", exp, s);
        exit(1);
    }
}
int* stringToInts(char* s, int* count) {
    char* exp = "Invalid argument";
    char* str = stringInArray(s);
    char* token = strtok(str, ",");
    int* result = malloc(sizeof(int) * strlen(token));
    int i = 0;

    while (token != NULL) {
        result[i++] = stringToInt(token);
        token = strtok(NULL, ",");
    }

    *count = i;
    free(str);
    return result;
}
double stringToDouble(char* s) {
    char* exp = "Invalid argument";
    double result;
    if (sscanf(s, "%lf", &result) == 1) {
        return result;
    } else {
        fprintf(stderr, "%s: %s\n", exp, s);
        exit(1);
    }
}
double* stringToDoubles(char* s, int* count) {
    char* exp = "Invalid argument";
    char* str = stringInArray(s);
    char* token = strtok(str, ",");
    double* result = malloc(sizeof(double) * strlen(token));
    int i = 0;

    while (token != NULL) {
        result[i++] = stringToDouble(token);
        token = strtok(NULL, ",");
    }

    *count = i;
    free(str);
    return result;
}
char** stringToStrings(char* s, int* count) {
    char* exp = "Invalid argument";
    char* str = stringInArray(s);
    char* token = strtok(str, ",");
    char** result = malloc(sizeof(char*) * strlen(token));
    int i = 0;

    while (token != NULL) {
        result[i] = strdup(token);
        token = strtok(NULL, ",");
        i++;
    }

    *count = i;
    free(str);
    return result;
}
_Bool stringToBool(char* s) {
    char* exp = "Invalid argument";
    _Bool result;
    if (strcmp(s, "true") == 0 || strcmp(s, "1") == 0) {
        return 1;
    } else if (strcmp(s, "false") == 0 || strcmp(s, "0") == 0) {
        return 0;
    } else {
        fprintf(stderr, "%s: %s\n", exp, s);
        exit(1);
    }
}
_Bool* stringToBools(char* s, int* count) {
    char* exp = "Invalid argument";
    char* str = stringInArray(s);
    char* token = strtok(str, ",");
    _Bool* result = malloc(sizeof(_Bool) * strlen(token));
    int i = 0;

    while (token != NULL) {
        result[i++] = stringToBool(token);
        token = strtok(NULL, ",");
    }

    *count = i;
    free(str);
    return result;
}
char** splitString(char *inputString, char *delimiter) {
    char *token = strtok(inputString, delimiter);
    char** result = malloc(sizeof(char*) * MAX_TOKENS);
    int count = 0;
    while (token != NULL && count < MAX_TOKENS) {
        result[count] = malloc(strlen(token) + 1);
        strcpy(result[count++], token);
        token = strtok(NULL, delimiter);
    }
    result[count] = NULL;
    return result;
}
char* toText(void* i, size_t size) {
    return i == NULL ? "null" : (char*)i;
}
char* arrayToText(char** r, size_t size) {
    if (r == NULL) return "null";
    char buffer[1024];
    buffer[0] = '['; buffer[1] = '\0';
    for (size_t i = 0; i < size; ++i) {
        strcat(buffer, toText(r[i],0));
        if (i < size - 1) {
            strcat(buffer, ",");
        }
    }
    strcat(buffer, "]");
    return buffer;
}
int main() {
    char inputString[MAX_STRING_LENGTH];
    fgets(inputString, MAX_STRING_LENGTH, stdin);
    inputString[strcspn(inputString, "\n")] = '\0';
    char **a = splitString(inputString, "|");
    printf("%s", arrayToText(a));
    return 0;
}
    
