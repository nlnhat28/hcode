
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
                static string[] stringToBool(string s) 
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
                    Console.Write(s);
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
                static List<string> splitString(string inputString = string.Empty, char delimiter)
                {
                    return inputString?.Split(delimiter)?.ToList();
                }
                static void Main()
                {
                    string inputString = Console.ReadLine();
                    var a = splitString(inputString, '|');
                    var sol = new Solution();
                    {output}(sol.solve({args}));
                }
                {userCode}
            };