
import re
{userCode}
def stringInArray(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        m = re.match(r"^\[(.+)\]$", s.strip())
        return m.group(1) if m else exp
    except:
        raise exp
def stringToInt(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return int(s.strip())
    except:
        raise exp
def stringToInts(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [stringToInt(i) for i in stringInArray(s).split(",")]
    except:
        raise exp
def stringToDouble(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return float(s.strip())
    except:
        raise exp
def stringToDoubles(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [stringToDouble(i) for i in stringInArray(s).split(",")]
    except:
        raise exp
def stringToStrings(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [i.strip() for i in stringInArray(s).split(",")]
    except:
        raise exp
def stringToBool(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return bool(s.strip())
    except:
        raise exp
def stringToBools(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [stringToBool(i) for i in stringInArray(s).split(",")]
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
    print(f"[{','.join(map(str, a))}]", end='')

def printInts(a):
    print(f"[{','.join(map(str, a))}]", end='')

def printDoubles(a):
    print(f"[{','.join(map(str, a))}]", end='')

def printBools(a):
    print(f"[{','.join(map(str, a))}]", end='')
def split_string(input_string="", delimiter='|'):
    return input_string.split(delimiter) if input_string else []
if __name__ == "__main__":
    input_string = input()
    a = split_string(input_string, '|')
    sol = Solution()
    {stdout}(sol.solve({args}))
