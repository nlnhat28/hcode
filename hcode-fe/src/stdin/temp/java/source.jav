import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

{userCode}
public class Main {
    static String stringInArray(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            var m = Pattern.compile("^\\[(.+)]$").matcher(s.trim());
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
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Integer.parseInt(s.trim());
        } catch (NumberFormatException e) {
            throw exp;
        }
    }
    static int[] stringToInts(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Arrays.stream(stringInArray(s).split(","))
                    .mapToInt(i -> stringToInt(i.trim()))
                    .toArray();
        } catch (Exception e) {
            throw exp;
        }
    }
    static double stringToDouble(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Double.parseDouble(s.trim());
        } catch (NumberFormatException e) {
            throw exp;
        }
    }
    static double[] stringToDoubles(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Arrays.stream(stringInArray(s).split(","))
                    .mapToDouble(i -> stringToDouble(i.trim()))
                    .toArray();
        } catch (Exception e) {
            throw exp;
        }
    }
    static String[] stringToStrings(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Arrays.stream(stringInArray(s).split(","))
                    .toArray(String[]::new);
        } catch (Exception e) {
            throw exp;
        }
    }
    static boolean stringToBool(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            return Boolean.parseBoolean(s.trim());
        } catch (NumberFormatException e) {
            throw exp;
        }
    }
    static boolean[] stringToBools(String s) {
        var exp = new RuntimeException("Invalid argument: " + s);
        try {
            String[] parts = stringInArray(s).split(",");
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
        System.out.print("[" + String.join(",", a) + "]");
    }
    static void printInts(int[] a) {
        System.out.print("[" + Arrays.toString(a) + "]");
    }
    static void printDoubles(double[] a) {
        System.out.print("[" + Arrays.toString(a) + "]");
    }
    static void printBools(boolean[] a) {
        System.out.print("[" + Arrays.toString(a) + "]");
    }
    static List<String> splitString(String inputString, char delimiter) {
        return inputString != null ? Arrays.asList(inputString.split(String.valueOf(delimiter))) : null;
    }
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        String inputString = scanner.nextLine();
        List<String> a = splitString(inputString, '|');
        Solution sol = new Solution();
        {output}(sol.solve({args}));
    }
}
