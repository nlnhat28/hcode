
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
            using System.Collections.Generic;
            class Program
            {
                static void SplitString(string inputString, char delimiter, out List<string> tokens)
                {
                    tokens = new List<string>();
                    var splitTokens = inputString.Split(delimiter);
                    foreach (var token in splitTokens) { tokens.Add(token); }
                }
                static void Main()
                {
                    string inputString = Console.ReadLine(); char delimiter = '|'; List<string> tokens;
                    SplitString(inputString, delimiter, out tokens);
                    var sol = new Solution(); var a = new List<dynamic>();
                    if (tokens?.Count > 0)
                    {
                        for (int i = 0; i < tokens.Count; i++)
                        {
                            a.Add(tokens[i]);
                        }
                        Console.WriteLine(sol.solve({args}));
                    }
                    else
                    {
                        Console.WriteLine(sol.solve({args}));
                    }
                }
                {userCode}
            }";
    }
}
