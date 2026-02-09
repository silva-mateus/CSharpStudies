# Any & All
Using LINQ, implement the IsAnyWordWhiteSpace method, which checks if, in a given collection of strings, any word consists only of white space characters.

White space characters are all "empty" chars, like a space or a new line symbol. We can check if a character is a white space by using the char.IsWhiteSpace method.

For example:

- for words {"hello", "There    "} the result shall be false because no word consists only of white space characters

- for words {"hello", "      "}, the result shall be true because "      " consists only of white space characters

- for empty collection, the result shall be false because no word consists only of whitespace characters (because there are no words at all)