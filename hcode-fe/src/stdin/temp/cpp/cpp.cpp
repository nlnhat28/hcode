#include <iostream>
#include <vector>
#include <sstream>
#include <regex> 

class Program {
public:
    static void Main() {
        std::string a = "[1 , 2, 3]";
        auto r = StringToInts(a);
        for (const auto& i : r) {
            std::cout << i << std::endl;
        }
    }

    static std::string StringInArray(const std::string& s) {
        try {
            std::smatch m;
            std::regex_match(s, m, std::regex(R"(\[(.+)\])"));
            return m.size() == 2 ? m[1].str() : throw std::invalid_argument("Invalid argument: " + s);
        }
        catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }

    static int StringToInt(const std::string& s) {
        try {
            return std::stoi(s);
        }
        catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }

    static std::vector<int> StringToInts(const std::string& s) {
        try {
            std::vector<int> result;
            std::istringstream ss(StringInArray(s));
            std::string token;
            while (std::getline(ss, token, ',')) {
                result.push_back(StringToInt(token));
            }
            return result;
        }
        catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }

    static double StringToDouble(const std::string& s) {
        try {
            return std::stod(s);
        }
        catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }

    // Similar adjustments for other conversion functions
};

int main() {
    Program::Main();
    return 0;
}
