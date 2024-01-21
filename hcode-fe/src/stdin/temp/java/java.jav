import java.util.Arrays;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {
    public static void main(String[] args) {
        String a = "[1 , 2, 3]";
        int[] r = stringToInts(a);
        Arrays.stream(r).forEach(System.out::println);
    }
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
}
