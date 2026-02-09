namespace Assignment_Dice_Roll_Game;

public class Dice : IDice
{
    public int NumberOfFaces { get; }

    public Dice(int numberOfFaces)
    {
        if (numberOfFaces <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfFaces), "NumberOfFaces must be greater than 0");
        }
        NumberOfFaces = numberOfFaces;
    }

    public int Roll()
    {
        return Random.Shared.Next(1, NumberOfFaces + 1);
    }
}