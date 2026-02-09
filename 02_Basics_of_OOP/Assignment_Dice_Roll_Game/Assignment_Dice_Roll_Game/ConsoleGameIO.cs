namespace Assignment_Dice_Roll_Game;

public class ConsoleGameIO : IGameIO
{
    public int ReadInteger(string prompt)
    {
        int result;
        do
        {
            Console.WriteLine(prompt);
        } while (!int.TryParse(Console.ReadLine(), out result));
        return result;
    }
    public void WriteLine(string message) => Console.WriteLine(message);
}