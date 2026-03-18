using Assignment_Quote_Finder.UserCommunication;

namespace Assignment_Quote_Finder.Fakes;

public class FakeUserCommunication : IUserCommunication
{
    private readonly Queue<string> _inputs;

    public FakeUserCommunication(params string[] inputs)
    {
        _inputs = new Queue<string>(inputs);
    }

    public List<string> PrintedMessages{ get; } = new();
    public void PrintMessage(string message) => PrintedMessages.Add(message);
    public bool ReadBool(string prompt) => _inputs.Dequeue() == "y";
    public int ReadInt(string prompt) => int.Parse(_inputs.Dequeue());
    public string ReadWord(string prompt) => _inputs.Dequeue();
}

