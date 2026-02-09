

public interface IUserInterface
{
    void DisplayMessage(string message);
    string GetInput();
}

public class ConsoleUserInterface : IUserInterface
{
    public void DisplayMessage(string message) => Console.WriteLine(message);
    public string GetInput() => Console.ReadLine() ?? "";
}

public interface ITodoList
{
    void Add(string item);
    void RemoveAt(int index);
    string GetItemAt(int index);
    bool Contains(string item);
    int Count { get; }
    IReadOnlyList<string> GetAll();
}

public class TodoList : ITodoList
{
    private readonly List<string> _items = new List<string>();

    public void Add(string item) => _items.Add(item);
    public void RemoveAt(int index) => _items.RemoveAt(index);
    public string GetItemAt(int index) => _items[index];
    public bool Contains(string item) => _items.Contains(item);
    public int Count => _items.Count;
    public IReadOnlyList<string> GetAll() => _items.AsReadOnly();
}

public interface ITodoValidator
{
    bool IsValid(string description, out string errorMessage);
    bool IsValidIndex(string input, int maxIndex, out int parsedIndex, out string errorMessage);
}

public class TodoValidator : ITodoValidator
{
    private readonly ITodoList _todoList;

    public TodoValidator(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public bool IsValid(string description, out string errorMessage)
    {
        if (string.IsNullOrEmpty(description))
        {
            errorMessage = "The description cannot be empty.";
            return false;
        }
        if (_todoList.Contains(description))
        {
            errorMessage = "The description must be unique.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    public bool IsValidIndex(string input, int maxIndex, out int parsedIndex, out string errorMessage)
    {
        parsedIndex = 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "The index cannot be empty.";
            return false;
        }
        if (!int.TryParse(input, out parsedIndex))
        {
            errorMessage = "The given index is not valid.";
            return false;
        }
        if (parsedIndex < 1 || parsedIndex > maxIndex)
        {
            errorMessage = "The given index is not valid.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    public interface ITodoFormatter
    {
        string FormatList(IReadOnlyList<string> items);
        string FormatAddedMessage(string description);
        string FormatRemovedMessage(string description);
    }

    public class TodoFormatter : ITodoFormatter
    {
        public string FormatList(IReadOnlyList<string> items)
        {
            if (items.Count == 0)
                return "No TODOs have been added yet.";
            var result = "";
            for (int i = 0; i < items.Count; i++)
            {
                result += $"{i + 1}. {items[i]}{Environment.NewLine}";
            }
            return result;
        }
        public string FormatAddedMessage(string description) => $"TODO successfully added: {description}";
        public string FormatRemovedMessage(string description) => $"TODO removed: {description}";
    }

    public class TodoApplication
    {
        private readonly ITodoList _todoList;
        private readonly ITodoValidator _validator;
        private readonly ITodoFormatter _formatter;
        private readonly IUserInterface _ui;

        public TodoApplication(ITodoList todoList, ITodoValidator validator, ITodoFormatter formatter, IUserInterface ui)
        {
            _todoList = todoList;
            _validator = validator;
            _formatter = formatter;
            _ui = ui;
        }

        public void ShowAllTodos()
        {
            var items = _todoList.GetAll();
            var formatted = _formatter.FormatList(items);
            _ui.DisplayMessage(formatted);
        }

        public void AddTodo()
        {
            _ui.DisplayMessage("Enter the TODO description:");
            var description = _ui.GetInput();
            if (_validator.IsValid(description, out string errorMessage))
            {
                _todoList.Add(description);
                _ui.DisplayMessage(_formatter.FormatAddedMessage(description));
            }
            else
            {
                _ui.DisplayMessage(errorMessage);
            }
        }

        public void RemoveTodo()
        {
            if (_todoList.Count == 0)
            {
                _ui.DisplayMessage("No TODOs have been added yet.");
                return;
            }
            _ui.DisplayMessage("Enter the index of the TODO you want to remove:");
            _ui.DisplayMessage(_formatter.FormatList(_todoList.GetAll()));

            var input = _ui.GetInput();

            if (_validator.IsValidIndex(input, _todoList.Count, out int index, out string errorMessage))
            {
                string removedItem = _todoList.GetItemAt(index - 1);
                _todoList.RemoveAt(index - 1);
                _ui.DisplayMessage(_formatter.FormatRemovedMessage(removedItem));
            }
            else
            {
                _ui.DisplayMessage(errorMessage);
            }
        }
    }

    public enum MenuOption
    {
        SeeAllTodos,
        AddTodo,
        RemoveTodo,
        Exit,
        Invalid
    }

    public static class MenuParser
    {
        public static MenuOption Parse(string selection)
        {
            return selection?.ToUpper() switch
            {
                "S" => MenuOption.SeeAllTodos,
                "A" => MenuOption.AddTodo,
                "R" => MenuOption.RemoveTodo,
                "E" => MenuOption.Exit,
                _ => MenuOption.Invalid
            };
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var ui = new ConsoleUserInterface();
            var todoList = new TodoList();
            var validator = new TodoValidator(todoList);
            var formatter = new TodoFormatter();
            var app = new TodoApplication(todoList, validator, formatter, ui);
            while (true)
            {
                ShowMenu(ui);
                var selection = ui.GetInput();
                var option = MenuParser.Parse(selection);
                switch (option)
                {
                    case MenuOption.SeeAllTodos:
                        app.ShowAllTodos();
                        break;
                    case MenuOption.AddTodo:
                        app.AddTodo();
                        break;
                    case MenuOption.RemoveTodo:
                        app.RemoveTodo();
                        break;
                    case MenuOption.Exit:
                        return;
                    default:
                        ui.DisplayMessage("Incorrect input");
                        break;
                }
            }
        }
        private static void ShowMenu(IUserInterface ui)
        {
            ui.DisplayMessage("Hello!");
            ui.DisplayMessage("What do you want to do?");
            ui.DisplayMessage("[S]ee all TODOs");
            ui.DisplayMessage("[A]dd a TODO");
            ui.DisplayMessage("[R]emove a TODO");
            ui.DisplayMessage("[E]xit");
        }
    }
}


