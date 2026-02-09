namespace Assignment_Password_Generator_Refactoring;

class Program
{
    static void Main(string[] args)
    {
        var passwordGenerator = new PasswordGenerator(new SystemRandomGenerator());
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(passwordGenerator.Generate(5, 10, true));
        }
    }
}