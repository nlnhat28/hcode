#include <iostream>
#include <vector>
#include <sstream>
#include <regex> 

class Program {
public:
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
    std::vector<double> stringToDoubles(const std::string& s) {
        try {
            std::vector<double> result;
            std::istringstream ss(StringInArray(s));
            std::string token;
            while (std::getline(ss, token, ',')) {
                result.push_back(stringToDouble(token));
            }
            return result;
        } catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }
    std::vector<std::string> stringToStrings(const std::string& s) {
        try {
            std::vector<std::string> result;
            std::istringstream ss(StringInArray(s));
            std::string token;
            while (std::getline(ss, token, ',')) {
                result.push_back(token);
            }
            return result;
        } catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }
    bool stringToBool(const std::string& s) {
        try {
            return std::stoi(s) != 0;
        } catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }
    std::vector<bool> stringToBools(const std::string& s) {
        try {
            std::vector<bool> result;
            std::istringstream ss(StringInArray(s));;
            std::string token;
            while (std::getline(ss, token, ',')) {
                result.push_back(stringToBool(token));
            }
            return result;
        } catch (...) {
            throw std::invalid_argument("Invalid argument: " + s);
        }
    }
    static void printString(const std::string& s) {
        std::cout << s;
    }
    static void printInt(int i) {
        std::cout << i;
    }
    static void printDouble(double d) {
        std::cout << d;
    }
    static void printBool(bool b) {
        std::cout << std::boolalpha << b;
    }
    static void printStrings(const std::vector<std::string>& a) {
        std::cout << '[';
        for (size_t i = 0; i < a.size(); ++i) {
            std::cout << a[i];
            if (i < a.size() - 1) {
                std::cout << ",";
            }
        }
        std::cout << ']';
    }
    static void printInts(const std::vector<int>& a) {
        std::cout << '[';
        for (size_t i = 0; i < a.size(); ++i) {
            std::cout << a[i];
            if (i < a.size() - 1) {
                std::cout << ",";
            }
        }
        std::cout << ']';
    }
    static void printDoubles(const std::vector<double>& a) {
        std::cout << '[';
        for (size_t i = 0; i < a.size(); ++i) {
            std::cout << a[i];
            if (i < a.size() - 1) {
                std::cout << ",";
            }
        }
        std::cout << ']';
    }
    static void printBools(const std::vector<bool>& a) {
        std::cout << '[';
        for (size_t i = 0; i < a.size(); ++i) {
            std::cout << std::boolalpha << a[i];
            if (i < a.size() - 1) {
                std::cout << ",";
            }
        }
        std::cout << ']';
    }
    static std::vector<std::string> splitString(const std::string& inputString, char delimiter) {
        std::vector<std::string> result;
        std::stringstream ss(inputString);
        std::string token;
        while (std::getline(ss, token, delimiter)) {
            result.push_back(token);
        }
        return result;
    }
    {userCode}
    static void Main() {
        std::string inputString;
        std::getline(std::cin, inputString);
        std::vector<std::string> a = splitString(inputString, delimiter);
        Solution sol;
        {output}(sol.solve({args}));
        return 0;
    }
};
int main() {
    Program::Main();
    return 0;
}
