namespace Assignment_Dice_Roll_Game;

public interface IGameIO
{
    int ReadInteger(string prompt);
    void WriteLine(string message);
}