
using System.Globalization;

namespace HCode.Domain
{
    /// <summary>
    /// SourcCode gửi lên Judge0
    /// </summary>
    public static class SourceCode
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CSharp = @"
            using System;
            using System.Linq;
            using System.Text.RegularExpressions;
            using System.Collections.Generic;

            class Program
            {
                static string stringInArray(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        var m = Regex.Match(s.Trim(),@""^\[(.+)\]$"");
                        return m.Success ? m.Groups[1].Value : throw exp;
                    }
                    catch { throw exp; }
                }
                static int stringToInt(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return int.TryParse(s.Trim(), out int i) ? i : throw exp;
                    }
                    catch { throw exp; }
                }
                static int[] stringToInts(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return stringInArray(s).Split("","").Select(i => stringToInt(i)).ToArray();
                    }
                    catch { throw exp; }
                }
                static double stringToDouble(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return double.TryParse(s.Trim(), out double i) ? i : throw exp;
                    }
                    catch { throw exp; }
                }
                static double[] stringToDoubles(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return stringInArray(s).Split("","").Select(i => stringToDouble(i)).ToArray();
                    }
                    catch { throw exp; }
                }
                static string[] stringToStrings(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return stringInArray(s).Split("","").Select(i => i?.ToString()).ToArray();
                    }
                    catch { throw exp; }
                }
                static bool stringToBool(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return bool.TryParse(s.Trim(), out bool i) ? i : throw exp;
                    }
                    catch { throw exp; }
                }
                static bool[] stringToBools(string s) 
                {
                    var exp = new Exception($""Invalid argument: {s}"");
                    try {
                        return stringInArray(s).Split("","").Select(i => stringToBool(i)).ToArray();
                    }
                    catch { throw exp; }
                }
                static void printString(string i)
                {
                    Console.Write(i);
                }
                static void printInt(int i)
                {
                    Console.Write(i);
                }
                static void printDouble(double i)
                {
                    Console.Write(i);
                }
                static void printBool(bool i)
                {
                    Console.Write(i);
                }
                static void printStrings(string[] a)
                {
                    Console.Write($""[{string.Join("","", a)}]"");
                }
                static void printInts(int[] a)
                {
                    Console.Write($""[{string.Join("","", a)}]"");
                }
                static void printDoubles(double[] a)
                {
                    Console.Write($""[{string.Join("","", a)}]"");
                }
                static void printBools(bool[] a)
                {
                    Console.Write($""[{string.Join("","", a)}]"");
                }
                static List<string> splitString(string inputString, string delimiter)
                {
                    return inputString?.Split(delimiter)?.ToList();
                }
                {userCode}
                static void Main()
                {
                    string inputString = Console.ReadLine();
                    var a = splitString(inputString, ""|"");
                    var sol = new Solution();
                    {stdout}(sol.solve({args}));
                }
            }
        ";

        public const string C = @"
            #include <stdio.h>
            #include <string.h>
            #include <stdlib.h>
            #define MAX_TOKENS 100
            #define MAX_STRING_LENGTH 2560
            char* stringInArray(char* s) {
                char* exp = ""Invalid argument"";
                char* result = NULL;
                char* trimmed = strdup(s);
                trimmed[strcspn(trimmed, ""\r\n"")] = 0;

                if (trimmed[0] == '[' && trimmed[strlen(trimmed) - 1] == ']') {
                    result = malloc(strlen(trimmed) - 1);
                    strncpy(result, trimmed + 1, strlen(trimmed) - 2);
                    result[strlen(trimmed) - 2] = '\0';
                } else {
                    fprintf(stderr, ""%s: %s\n"", exp, s);
                    exit(1);
                }

                free(trimmed);
                return result;
            }
            int stringToInt(char* s) {
                char* exp = ""Invalid argument"";
                int result;
                if (sscanf(s, ""%d"", &result) == 1) {
                    return result;
                } else {
                    fprintf(stderr, ""%s: %s\n"", exp, s);
                    exit(1);
                }
            }
            int* stringToInts(char* s, int* count) {
                char* exp = ""Invalid argument"";
                char* str = stringInArray(s);
                char* token = strtok(str, "","");
                int* result = malloc(sizeof(int) * strlen(token));
                int i = 0;

                while (token != NULL) {
                    result[i++] = stringToInt(token);
                    token = strtok(NULL, "","");
                }

                *count = i;
                free(str);
                return result;
            }
            double stringToDouble(char* s) {
                char* exp = ""Invalid argument"";
                double result;
                if (sscanf(s, ""%lf"", &result) == 1) {
                    return result;
                } else {
                    fprintf(stderr, ""%s: %s\n"", exp, s);
                    exit(1);
                }
            }
            double* stringToDoubles(char* s, int* count) {
                char* exp = ""Invalid argument"";
                char* str = stringInArray(s);
                char* token = strtok(str, "","");
                double* result = malloc(sizeof(double) * strlen(token));
                int i = 0;

                while (token != NULL) {
                    result[i++] = stringToDouble(token);
                    token = strtok(NULL, "","");
                }

                *count = i;
                free(str);
                return result;
            }
            char** stringToStrings(char* s, int* count) {
                char* exp = ""Invalid argument"";
                char* str = stringInArray(s);
                char* token = strtok(str, "","");
                char** result = malloc(sizeof(char*) * strlen(token));
                int i = 0;

                while (token != NULL) {
                    result[i] = strdup(token);
                    token = strtok(NULL, "","");
                    i++;
                }

                *count = i;
                free(str);
                return result;
            }
            _Bool stringToBool(char* s) {
                char* exp = ""Invalid argument"";
                _Bool result;
                if (strcmp(s, ""true"") == 0 || strcmp(s, ""1"") == 0) {
                    return 1;
                } else if (strcmp(s, ""false"") == 0 || strcmp(s, ""0"") == 0) {
                    return 0;
                } else {
                    fprintf(stderr, ""%s: %s\n"", exp, s);
                    exit(1);
                }
            }
            _Bool* stringToBools(char* s, int* count) {
                char* exp = ""Invalid argument"";
                char* str = stringInArray(s);
                char* token = strtok(str, "","");
                _Bool* result = malloc(sizeof(_Bool) * strlen(token));
                int i = 0;

                while (token != NULL) {
                    result[i++] = stringToBool(token);
                    token = strtok(NULL, "","");
                }

                *count = i;
                free(str);
                return result;
            }
            void printString(char *i) {
                printf(""%s"", i);
            }
            void printInt(int i) {
                printf(""%d"", i);
            }
            void printDouble(double i) {
                printf(""%f"", i);
            }
            void printBool(int i) {
                printf(""%d"", i);
            }
            void printStrings(char **a, int size) {
                printf(""["");
                for (int j = 0; j < size; ++j) {
                    printf(""%s"", a[j]);
                    if (j < size - 1) {
                        printf("","");
                    }
                }
                printf(""]"");
            }
            void printInts(int *a, int size) {
                printf(""["");
                for (int j = 0; j < size; ++j) {
                    printf(""%d"", a[j]);
                    if (j < size - 1) {
                        printf("","");
                    }
                }
                printf(""]"");
            }
            void printDoubles(double *a, int size) {
                printf(""["");
                for (int j = 0; j < size; ++j) {
                    printf(""%f"", a[j]);
                    if (j < size - 1) {
                        printf("","");
                    }
                }
                printf(""]"");
            }
            void printBools(int *a, int size) {
                printf(""["");
                for (int j = 0; j < size; ++j) {
                    printf(""%d"", a[j]);
                    if (j < size - 1) {
                        printf("","");
                    }
                }
                printf(""]"");
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
            {userCode}
            int main() {
                char inputString[MAX_STRING_LENGTH];
                fgets(inputString, MAX_STRING_LENGTH, stdin);
                inputString[strcspn(inputString, ""\n"")] = '\0';
                char **a = splitString(inputString, ""|"");
                {stdout}(solve({args}));
                return 0;
            }
        ";

        public const string Cpp = @"
            #include <iostream>
            #include <vector>
            #include <sstream>
            #include <regex> 

            class Program {
            public:
                static std::string stringInArray(const std::string& s) {
                    try {
                        std::smatch m;
                        std::regex_match(s, m, std::regex(R""(\[(.+)\])""));
                        return m.size() == 2 ? m[1].str() : throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                    catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                static int stringToInt(const std::string& s) {
                    try {
                        return std::stoi(s);
                    }
                    catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                static std::vector<int> stringToInts(const std::string& s) {
                    try {
                        std::vector<int> result;
                        std::istringstream ss(stringInArray(s));
                        std::string token;
                        while (std::getline(ss, token, ',')) {
                            result.push_back(stringToInt(token));
                        }
                        return result;
                    }
                    catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                static double stringToDouble(const std::string& s) {
                    try {
                        return std::stod(s);
                    }
                    catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                std::vector<double> stringToDoubles(const std::string& s) {
                    try {
                        std::vector<double> result;
                        std::istringstream ss(stringInArray(s));
                        std::string token;
                        while (std::getline(ss, token, ',')) {
                            result.push_back(stringToDouble(token));
                        }
                        return result;
                    } catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                std::vector<std::string> stringToStrings(const std::string& s) {
                    try {
                        std::vector<std::string> result;
                        std::istringstream ss(stringInArray(s));
                        std::string token;
                        while (std::getline(ss, token, ',')) {
                            result.push_back(token);
                        }
                        return result;
                    } catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                bool stringToBool(const std::string& s) {
                    try {
                        return std::stoi(s) != 0;
                    } catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
                    }
                }
                std::vector<bool> stringToBools(const std::string& s) {
                    try {
                        std::vector<bool> result;
                        std::istringstream ss(stringInArray(s));;
                        std::string token;
                        while (std::getline(ss, token, ',')) {
                            result.push_back(stringToBool(token));
                        }
                        return result;
                    } catch (...) {
                        throw std::invalid_argument(""Invalid argument: "" + s);
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
                            std::cout << "","";
                        }
                    }
                    std::cout << ']';
                }
                static void printInts(const std::vector<int>& a) {
                    std::cout << '[';
                    for (size_t i = 0; i < a.size(); ++i) {
                        std::cout << a[i];
                        if (i < a.size() - 1) {
                            std::cout << "","";
                        }
                    }
                    std::cout << ']';
                }
                static void printDoubles(const std::vector<double>& a) {
                    std::cout << '[';
                    for (size_t i = 0; i < a.size(); ++i) {
                        std::cout << a[i];
                        if (i < a.size() - 1) {
                            std::cout << "","";
                        }
                    }
                    std::cout << ']';
                }
                static void printBools(const std::vector<bool>& a) {
                    std::cout << '[';
                    for (size_t i = 0; i < a.size(); ++i) {
                        std::cout << std::boolalpha << a[i];
                        if (i < a.size() - 1) {
                            std::cout << "","";
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
                    std::vector<std::string> a = splitString(inputString, '|');
                    Solution sol;
                    {stdout}(sol.solve({args}));
                }
            };
            int main() {
                Program::Main();
                return 0;
            }
        ";

        public const string Javascript = @"
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
                    return stringInArray(s).split("","").map(i => stringToInt(i));
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
                    return stringInArray(s).split("","").map(i => stringToDouble(i));
                } catch { throw exp; }
            }
            function stringToStrings(s) {
                const exp = new Error(`Invalid argument: ${s}`);
                try {
                    return stringInArray(s).split("","").map(i => i.toString());
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
                    return stringInArray(s).split("","").map(i => stringToBool(i));
                } catch { throw exp; }
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
                console.log(`[${a.join("","")}]`);
            }
            function printInts(a) {
                console.log(`[${a.join("","")}]`);
            }
            function printDoubles(a) {
                console.log(`[${a.join("","")}]`);
            }
            function printBools(a) {
                console.log(`[${a.join("","")}]`);
            }
            function splitString(inputString, delimiter) {
                return inputString.split(delimiter);
            }
            {userCode}
            function main() {
                let inputString = """";
                process.stdin.on(""data"", (chunk) => {
                    inputString += chunk.toString();
                });
                process.stdin.on(""end"", () => {
                    const a = splitString(inputString, ""|"");
                    {stdout}(solve({args}));
                });
            }
            if (require.main === module) {
                main();
            }
        ";

        public const string Php = @"
            <?php
            function stringInArray($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    preg_match('/^\[(.+)\]$/', trim($s), $matches);
                    if ($matches) {
                        return $matches[1];
                    }
                    else throw $exp;
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToInt($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    if (is_numeric(trim($s))) {
                        return (int)$s;
                    }
                    else throw $exp;
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToInts($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    return array_map('stringToInt', explode(',', stringInArray($s)));
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToDouble($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    if (is_numeric(trim($s))) {
                        return (double)$s;
                    }
                    else throw $exp;
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToDoubles($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    return array_map('stringToDouble', explode(',', stringInArray($s)));
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToStrings($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    return array_map('strval', explode(',', stringInArray($s)));
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToBool($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    return filter_var(trim($s), FILTER_VALIDATE_BOOLEAN, FILTER_NULL_ON_FAILURE);
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function stringToBools($s)
            {
                $exp = new Exception(""Invalid argument: $s"");
                try {
                    return array_map('stringToBool', explode(',', stringInArray($s)));
                } catch (Exception $e) {
                    throw $exp;
                }
            }
            function printString($i) {
                echo $i;
            }
            function printInt($i) {
                echo $i;
            }
            function printDouble($i) {
                echo $i;
            }
            function printBool($i) {
                echo $i;
            }
            function printStrings($a) {
                echo ""["" . implode("","", $a) . ""]"";
            }
            function printInts($a) {
                echo ""["" . implode("","", $a) . ""]"";
            }
            function printDoubles($a) {
                echo ""["" . implode("","", $a) . ""]"";
            }
            function printBools($a) {
                echo ""["" . implode("","", $a) . ""]"";
            }
            function splitString($inputString, $delimiter) {
                return explode($delimiter, $inputString);
            }
            {userCode}
            $inputString = fgets(STDIN);
            $a = splitString($inputString, '|');
            $sol = new Solution();
            {stdout}($sol->solve({args}));
            ?>
        ";

        public const string Java = @"
            import java.util.Arrays;
            import java.util.List;
            import java.util.Scanner;
            import java.util.regex.Matcher;
            import java.util.regex.Pattern;

            {userCode}
            public class Main {
                static String stringInArray(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        var m = Pattern.compile(""^\\[(.+)]$"").matcher(s.trim());
                        if (m.matches()) {
                            return m.group(1);
                        } else {
                            throw exp;
                        }
                    } catch (Exception e) {
                        throw exp;
                    }
                }
                static int stringToInt(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Integer.parseInt(s.trim());
                    } catch (NumberFormatException e) {
                        throw exp;
                    }
                }
                static int[] stringToInts(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Arrays.stream(stringInArray(s).split("",""))
                                .mapToInt(i -> stringToInt(i.trim()))
                                .toArray();
                    } catch (Exception e) {
                        throw exp;
                    }
                }
                static double stringToDouble(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Double.parseDouble(s.trim());
                    } catch (NumberFormatException e) {
                        throw exp;
                    }
                }
                static double[] stringToDoubles(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Arrays.stream(stringInArray(s).split("",""))
                                .mapToDouble(i -> stringToDouble(i.trim()))
                                .toArray();
                    } catch (Exception e) {
                        throw exp;
                    }
                }
                static String[] stringToStrings(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Arrays.stream(stringInArray(s).split("",""))
                                .toArray(String[]::new);
                    } catch (Exception e) {
                        throw exp;
                    }
                }
                static boolean stringToBool(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        return Boolean.parseBoolean(s.trim());
                    } catch (NumberFormatException e) {
                        throw exp;
                    }
                }
                static boolean[] stringToBools(String s) {
                    var exp = new RuntimeException(""Invalid argument: "" + s);
                    try {
                        String[] parts = stringInArray(s).split("","");
                        boolean[] result = new boolean[parts.length];
                        for (int i = 0; i < parts.length; i++) {
                            result[i] = stringToBool(parts[i].trim());
                        }
                        return result;
                    } catch (Exception e) {
                        throw exp;
                    }
                } 
                static void printString(String i) {
                    System.out.print(i);
                }
                static void printInt(int i) {
                    System.out.print(i);
                }
                static void printDouble(double i) {
                    System.out.print(i);
                }
                static void printBool(boolean i) {
                    System.out.print(i);
                }
                static void printStrings(String[] a) {
                    System.out.print(""["" + String.join("","", a) + ""]"");
                }
                static void printInts(int[] a) {
                    System.out.print(""["" + Arrays.toString(a) + ""]"");
                }
                static void printDoubles(double[] a) {
                    System.out.print(""["" + Arrays.toString(a) + ""]"");
                }
                static void printBools(boolean[] a) {
                    System.out.print(""["" + Arrays.toString(a) + ""]"");
                }
                static String[] splitString(String inputString, String delimiter) {
                    return inputString != null ? inputString.split(""\\Q"" + delimiter + ""\\E"") : null;
                }
                public static void main(String[] args) {
                    Scanner scanner = new Scanner(System.in);
                    String inputString = scanner.nextLine();
                    var a = splitString(inputString, ""|"");
                    Solution sol = new Solution();
                    {stdout}(sol.solve({args}));
                }
            }
        ";

        public const string Python = @"
import re
{userCode}
def stringInArray(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        m = re.match(r""^\[(.+)\]$"", s.strip())
        return m.group(1) if m else exp
    except:
        raise exp
def stringToInt(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return int(s.strip())
    except:
        raise exp
def stringToInts(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return [stringToInt(i) for i in stringInArray(s).split("","")]
    except:
        raise exp
def stringToDouble(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return float(s.strip())
    except:
        raise exp
def stringToDoubles(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return [stringToDouble(i) for i in stringInArray(s).split("","")]
    except:
        raise exp
def stringToStrings(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return [i.strip() for i in stringInArray(s).split("","")]
    except:
        raise exp
def stringToBool(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return bool(s.strip())
    except:
        raise exp
def stringToBools(s):
    exp = Exception(f""Invalid argument: {s}"")
    try:
        return [stringToBool(i) for i in stringInArray(s).split("","")]
    except:
        raise exp
def printString(s):
    print(s, end='')

def printInt(i):
    print(i, end='')

def printDouble(i):
    print(i, end='')

def printBool(i):
    print(i, end='')

def printStrings(a):
    print(f""[{','.join(map(str, a))}]"", end='')

def printInts(a):
    print(f""[{','.join(map(str, a))}]"", end='')

def printDoubles(a):
    print(f""[{','.join(map(str, a))}]"", end='')

def printBools(a):
    print(f""[{','.join(map(str, a))}]"", end='')
def split_string(input_string="""", delimiter='|'):
    return input_string.split(delimiter) if input_string else []
if __name__ == ""__main__"":
    input_string = input()
    a = split_string(input_string, '|')
    sol = Solution()
    {stdout}(sol.solve({args}))
";
    }
}
