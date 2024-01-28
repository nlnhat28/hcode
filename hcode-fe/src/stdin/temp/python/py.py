import re

def string_in_array(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        m = re.match(r"^\[(.+)\]$", s.strip())
        return m.group(1) if m else exp
    except:
        raise exp

def string_to_int(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return int(s.strip())
    except:
        raise exp

def string_to_ints(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [string_to_int(i) for i in string_in_array(s).split(",")]
    except:
        raise exp

def string_to_double(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return float(s.strip())
    except:
        raise exp

def string_to_doubles(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [string_to_double(i) for i in string_in_array(s).split(",")]
    except:
        raise exp

def string_to_strings(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [i.strip() for i in string_in_array(s).split(",")]
    except:
        raise exp

def string_to_bool(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return bool(s.strip())
    except:
        raise exp

def string_to_bools(s):
    exp = Exception(f"Invalid argument: {s}")
    try:
        return [string_to_bool(i) for i in string_in_array(s).split(",")]
    except:
        raise exp

if __name__ == "__main__":
    a = "[1, 2, 3]"
    r = string_to_ints(a)
    for i in r:
        print(i)
