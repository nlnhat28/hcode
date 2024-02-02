using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
class Program
{   
    static void Main()
    {
        var a = "[1 , 2, 3]";
        var r = stringToInts(a);
        Console.WriteLine(arrayToText(r));
    }
    static string stringInArray(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            var m = Regex.Match(s.Trim(),@"^\[(.+)\]$");
            return m.Success ? m.Groups[1].Value : throw exp;
        }
        catch { throw exp; }
    }
    static int stringToInt(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return int.TryParse(s.Trim(), out int i) ? i : throw exp;
        }
        catch { throw exp; }
    }
    static int[] stringToInts(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return stringInArray(s).Split(",").Select(i => stringToInt(i)).ToArray();
        }
        catch { throw exp; }
    }
    static double stringToDouble(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return double.TryParse(s.Trim(), out double i) ? i : throw exp;
        }
        catch { throw exp; }
    }
    static double[] stringToDoubles(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return stringInArray(s).Split(",").Select(i => stringToDouble(i)).ToArray();
        }
        catch { throw exp; }
    }
    static string[] stringToStrings(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return stringInArray(s).Split(",").Select(i => i?.ToString()).ToArray();
        }
        catch { throw exp; }
    }
    static bool stringToBool(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return bool.TryParse(s.Trim(), out bool i) ? i : throw exp;
        }
        catch { throw exp; }
    }
    static bool[] stringToBools(string s) 
    {
        var exp = new Exception($"Invalid argument: {s}");
        try {
            return stringInArray(s).Split(",").Select(i => stringToBool(i)).ToArray();
        }
        catch { throw exp; }
    }
    static string toText(dynamic i) {
        return i == null ? "null" : i.ToString();
    }
    static string arrayToText<T>(T[] r) {
        if (r == null) return "null";
        return $"[{string.Join(",", r.Select(i => toText(i)))}]";
    }
}