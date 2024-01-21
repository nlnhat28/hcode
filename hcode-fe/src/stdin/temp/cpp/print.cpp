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