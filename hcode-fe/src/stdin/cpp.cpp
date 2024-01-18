#include <iostream>
#include <sstream>
#include <string>
#include <vector>

#define MAX_TOKENS 100
#define MAX_STRING_LENGTH 2560

void splitString(const std::string &inputString, char delimiter, std::vector<std::string> &tokens) {
    std::istringstream ss(inputString);
    std::string token;

    // Tokenize the string
    while (std::getline(ss, token, delimiter) && tokens.size() < MAX_TOKENS) {
        tokens.push_back(token);
    }
}

int main() {
    std::string inputString;
    std::getline(std::cin, inputString);

    char delimiter = '|';
    std::vector<std::string> tokens;

    // Split the string and get tokens
    splitString(inputString, delimiter, tokens);

    // Print the tokens
    for (size_t i = 0; i < tokens.size(); i++) {
        std::cout << "Token " << (i + 1) << ": " << tokens[i] << std::endl;
    }

    return 0;
}
