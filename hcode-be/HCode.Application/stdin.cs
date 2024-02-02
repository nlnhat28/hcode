
//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text.Json;
//class Program
//{
//    static void SplitString(string inputString, char delimiter, out List<string> tokens)
//    {
//        tokens = new List<string>();
//        var splitTokens = inputString.Split(delimiter);
//        foreach (var token in splitTokens) { tokens.Add(token); }
//    }
//    static void Main()
//    {
//        string inputString = Console.ReadLine(); char delimiter = '|'; List<string> tokens;
//        SplitString(inputString, delimiter, out tokens);
//        var sol = new Solution(); var a = new List<dynamic>();
//        if (tokens?.Count > 0)
//        {
//            for (int i = 0; i < tokens.Count; i++)
//            {
//                a.Add(tokens[i]);
//            }
//            object res = sol.solve(a[0], a[1]);
//        }
//        var result = JsonSerializer.Serialize(res);
//        Console.WriteLine(result);
//    }
//    public static ManagedToNativeComInteropStubAttribute()
//    public class Solution
//    {
//        public int solve(int num1, int num2)
//        {
//            return num1 + num2;
//        }
//    }
//}
