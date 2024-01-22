using System.Diagnostics;

namespace WordFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // This snippet is useful for getting some stats about how the program performs
            var matrix = GenerateStreamOfWords(64, 64);

            // A very basic profiler:
            // "Profile" how much time it takes to set up the class
            var chrono = new Stopwatch();
            chrono.Start();
            var finder = new WordFinder(matrix);
            chrono.Stop();
            
            int numberOfWords = 20 * 1000000;
            Console.WriteLine($"Finder init time {chrono.ElapsedMilliseconds} ms");
            Console.WriteLine($"Creating {numberOfWords} random words to search...");
            var wordsToSearch = GenerateStreamOfWords(numberOfWords, 5);

            // "profile" the search
            Console.WriteLine($"Starting to search...");
            chrono.Restart();
            finder.Find(wordsToSearch);
            chrono.Stop();
            Console.WriteLine($"Search time: {chrono.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Creates a pseudo random list of random words
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static List<string> GenerateStreamOfWords(int quantity, int len)
        {
            var result = new List<string>(quantity);

            for (int i = 0; i < quantity; i++)
            {
                string newWord = GenerarePseudoRandomWord(len);
                result.Add(newWord);
            }

            return result;
        }

        /// <summary>
        /// Creates a pseudo random string
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        private static string GenerarePseudoRandomWord(int len)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random();
            var word = new char[len];

            for (int i = 0; i < len; i++)
            {
                int indice = random.Next(allowedChars.Length);
                word[i] = allowedChars[indice];
            }

            return new string(word);
        }

    }
}