namespace Assignment_Dice_Roll_Game;

public interface IDice
{
    int NumberOfFaces { get; }
    int Roll();
}