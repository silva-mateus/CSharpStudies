namespace Coding.Exercise
{
    public class Exercise
    {
        public static Task<string> FormatSquaredNumbersFrom1To(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("N must not be negative");
            }
            Task<List<int>> task = Task.Run(() =>
            {
                List<int> numbers = new List<int>();
                for (int i = 1; i <= n; i++)
                {
                    numbers.Add(i * i);
                }
                return numbers;
            });
            return task.ContinueWith(t => string.Join(", ", t.Result));
        }

    }
}
