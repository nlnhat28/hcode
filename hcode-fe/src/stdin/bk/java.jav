import java.util.ArrayList;
import java.util.Scanner;

public class StringSplitter {
    private static final int MAX_TOKENS = 100;
    private static final int MAX_STRING_LENGTH = 2560;

    public static ArrayList<String> splitString(String inputString) {
        String delimiter = "\\|";
        ArrayList<String> tokens = new ArrayList<>();

        // Use String.split to split the string
        String[] splitTokens = inputString.split(delimiter, MAX_TOKENS);

        // Add tokens to the ArrayList
        for (String token : splitTokens) {
            tokens.add(token);
        }

        return tokens;
    }

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        String inputString = scanner.nextLine();

        ArrayList<String> tokens = splitString(inputString);

        // Print the tokens
        for (int i = 0; i < tokens.size(); i++) {
            System.out.println("Token " + (i + 1) + ": " + tokens.get(i));
        }

        // No explicit cleanup needed for ArrayList

        scanner.close();
    }
}
