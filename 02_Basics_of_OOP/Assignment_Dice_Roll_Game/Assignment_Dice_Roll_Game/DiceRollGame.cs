namespace Assignment_Dice_Roll_Game;

public static class GameMessages
{
    public const string DiceRolled = "Dice with rolled.";
    public const string EnterNumber = "Enter number:";
    public const string IncorrectInput = "Incorrect input";
    public const string WrongNumber = "Wrong number";
    public const string Win = "You win";
    public const string Lose = "You lose";
}

public enum GameResult
{
    Win,
    Lose
}

public class DiceRollGame
{
    private readonly IGameIO _io;
    private readonly IDice _dice;
    private readonly int _maxAttempts;

    public DiceRollGame(IGameIO io, IDice dice, int maxAttempts)
    {
        if (io == null) throw new ArgumentNullException(nameof(io));
        if (dice == null) throw new ArgumentNullException(nameof(dice));
        if (maxAttempts <= 0) throw new ArgumentOutOfRangeException(nameof(maxAttempts));
        _io = io;
        _dice = dice;
        _maxAttempts = maxAttempts;
    }

    private GameResult ProcessGuesses(int number)
    {
        int attempts = 0;
        while (attempts < _maxAttempts)
        {
            int guess = _io.ReadInteger(GameMessages.EnterNumber);
            if (guess == number)
            {
                return GameResult.Win;
            }
            _io.WriteLine(GameMessages.WrongNumber);
            attempts++;
        }
        return GameResult.Lose;
    }

    public GameResult Play()
    {
        _io.WriteLine(string.Format(GameMessages.DiceRolled, _dice.NumberOfFaces, _maxAttempts));
        int number = _dice.Roll();

        GameResult result = ProcessGuesses(number);
        _io.WriteLine(result == GameResult.Win ? GameMessages.Win : GameMessages.Lose);
        return result;
    }
}