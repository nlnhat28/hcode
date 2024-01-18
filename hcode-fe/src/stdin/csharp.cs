using System;
using System.Collections.Generic;

class Program
{
    const int MAX_TOKENS = 100;
    const int MAX_STRING_LENGTH = 2560;

    static void SplitString(string inputString, char delimiter, out List<string> tokens)
    {
        tokens = new List<string>();
        string[] splitTokens = inputString.Split(delimiter);

        // Add tokens to the list
        foreach (string token in splitTokens)
        {
            tokens.Add(token);
        }
    }

    static void Main()
    {
        string inputString = Console.ReadLine();

        char delimiter = '|';
        List<string> tokens;

        // Split the string and get tokens
        SplitString(inputString, delimiter, out tokens);

        var sol = new Solution();
        var res = new object();
        
        if (tokens?.Count > 0) {
            // Print the tokens
            for (int i = 0; i < tokens.Count; i++)
            {
                Console.WriteLine($"Token {i + 1}: {tokens[i]}");
            }



            var result = sol.solve({1});
        }
        else {
            var res = sol.solve();
        }
    }

    {2}
}
