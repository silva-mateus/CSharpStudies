namespace JD01_xUnit_Advanced_Testing;

/// <summary>
/// A string-based calculator. DO NOT MODIFY THIS CLASS -- write tests for it.
/// </summary>
public class StringCalculator
{
    private int _callerCount;

    /// <summary>
    /// Adds comma- or newline-separated numbers from a string.
    /// Empty string returns 0. Negative numbers throw ArgumentException.
    /// </summary>
    public int Add(string numbers)
    {
        _callerCount++;

        if (string.IsNullOrEmpty(numbers))
            return 0;

        var parts = numbers.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var sum = 0;

        foreach (var part in parts)
        {
            var num = int.Parse(part.Trim());
            if (num < 0)
                throw new ArgumentException($"Negatives not allowed: {num}");
            sum += num;
        }

        return sum;
    }

    /// <summary>
    /// Multiplies comma- or newline-separated numbers from a string.
    /// Empty string returns 1. Negative numbers throw ArgumentException.
    /// </summary>
    public int Multiply(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return 1;

        var parts = numbers.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var product = 1;

        foreach (var part in parts)
        {
            var num = int.Parse(part.Trim());
            if (num < 0)
                throw new ArgumentException($"Negatives not allowed: {num}");
            product *= num;
        }

        return product;
    }

    /// <summary>
    /// Parses and evaluates a simple expression like "3 + 4".
    /// Supports +, -, *, /. Throws DivideByZeroException for division by zero.
    /// Throws FormatException for invalid expressions.
    /// </summary>
    public double ParseExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new FormatException("Expression cannot be empty.");

        var parts = expression.Trim().Split(' ');
        if (parts.Length != 3)
            throw new FormatException($"Invalid expression format: '{expression}'. Expected 'a op b'.");

        if (!double.TryParse(parts[0], out var left))
            throw new FormatException($"Invalid left operand: '{parts[0]}'");

        var op = parts[1];

        if (!double.TryParse(parts[2], out var right))
            throw new FormatException($"Invalid right operand: '{parts[2]}'");

        return op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => right == 0
                ? throw new DivideByZeroException("Cannot divide by zero.")
                : left / right,
            _ => throw new FormatException($"Unknown operator: '{op}'")
        };
    }

    /// <summary>
    /// Returns how many times Add has been called since the last Reset.
    /// </summary>
    public int GetCallerCount() => _callerCount;

    /// <summary>
    /// Resets the Add call counter to zero.
    /// </summary>
    public void Reset() => _callerCount = 0;
}
