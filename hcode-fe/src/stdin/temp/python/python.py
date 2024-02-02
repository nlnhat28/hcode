MAX_TOKENS = 100
MAX_STRING_LENGTH = 2560

def split_string(input_string, delimiter):
    return input_string.split(delimiter, MAX_TOKENS - 1)

def main():
    input_string = input()

    delimiter = '|'
    tokens = split_string(input_string, delimiter)

    # Print the tokens
    for i, token in enumerate(tokens):
        print(f"Token {i + 1}: {token}")

if __name__ == "__main__":
    main()
