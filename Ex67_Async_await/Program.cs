using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public async void Test()
        {
            string data = await DownloadDataAsync("test.com", "some content");
            Console.WriteLine(data);
        }

        private async Task<string> DownloadDataAsync(string v1, string v2)
        {
            await Task.Delay(1000);
            return $"Content from URL '{v1}' is '{v2}'";
        }
    }
}
