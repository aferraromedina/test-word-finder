using System.Collections.Concurrent;
using System.Text;

namespace WordFinder
{
    public sealed class WordFinder
    {
        private readonly Dictionary<string, int> WordMatrix = new Dictionary<string, int>();

        public WordFinder(IEnumerable<string> matrix)
        {
            // The code assumes that the "stream" will be much larger than the matrix word, and that the find will be used 
            // many times with the same matrix.

            // I tried to optimize the search using simple and basic collections,
            // other structures that I found on the internet did not perform as expected with a real volume of data.
            // Having access to a real dictionary of words could make this approach faster.
            // On average, I get 150ms to set up and 180ms for searching 2M words of 5 characters.
            var matrixAsList = matrix.ToList();

            // Create vertical words from matrix
            var verticalWords = new List<string>();
            for (int i = 0; i < matrixAsList.Count; i++)
            {
                var verticalWordBuilder = new StringBuilder();
                foreach (var fila in matrix)
                {
                    verticalWordBuilder.Append(fila[i]);
                }
                verticalWords.Add(verticalWordBuilder.ToString());
            }

            // Add words to matrix
            matrixAsList.AddRange(verticalWords);

            for (int row = 0; row < matrixAsList.Count; row++)
            {
                for (var column = 0; column < matrixAsList[row].Length; column++)
                {
                    // Create a new "sub word" with every combination in the line
                    var horizonalWordBuilder = new StringBuilder();
                    foreach (var character in matrixAsList[row].Substring(column))
                    {
                        horizonalWordBuilder.Append(character);
                        var word = horizonalWordBuilder.ToString();
                        if (WordMatrix.ContainsKey(word))
                        {
                            WordMatrix[word]++;
                        }
                        else
                        {
                            WordMatrix.Add(word, 1);
                        }
                    }
                }
            }
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            var result = new ConcurrentDictionary<string, int>();
            int value = 0;

            // On localhost, parallel version is 50% faster, but this may not be the best option for a productive enviroment.
            Parallel.ForEach(wordstream, (word) =>
            {
                if (WordMatrix.TryGetValue(word, out value))
                {
                    // If any word in the word stream is found more than once within the stream,
                    // the search results should count it only once.
                    // So I do not care if TryAdd fails.
                    result.TryAdd(word, value);
                }
            });

            return result
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .Take(10);
        }
    }
}
