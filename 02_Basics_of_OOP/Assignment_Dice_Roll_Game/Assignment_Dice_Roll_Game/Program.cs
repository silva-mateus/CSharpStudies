namespace Assignment_Dice_Roll_Game;

// Rolls dice -> generates random number 1-6 (not show the user) -> user have 3 tries to guess the number on the die
public static class Program
{
    public static void Main()
    {
        var io = new ConsoleGameIO();
        var dice = new Dice(6);
        var maxAttempts = 3;

        var game = new DiceRollGame(io, dice, maxAttempts);

        game.Play();
    }
}