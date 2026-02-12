namespace Assignment_Quote_Finder.UserCommunication;

public interface IUserCommunication
{
    public void PrintMessage(string message);
    public string ReadWord(string prompt);
    public bool ReadBool(string prompt);
    public int ReadInt(string prompt);
}

