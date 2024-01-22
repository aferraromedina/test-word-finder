namespace WordFinderTest
{
    public class WordFinderTests
    {
        [Test]
        public void ShouldFindWordsInMatrix()
        {
            // Arrange
            var matrix = new List<string>{ "abcdc", 
                                           "fgwio", 
                                           "chill", 
                                           "pqnsd", 
                                           "uvdxy" };
            var finder = new WordFinder.WordFinder(matrix);

            // Act
            var expected = new[] { "cold", "wind", "chill" };
            var actual = finder.Find(new[] {"cold", "wind", "snow", "chill" });

            // Assert 
            CollectionAssert.AreEquivalent(expected, actual);
        }


        [Test]
        public void ShouldReturnEmptyList()
        {
            // Arrange
            var matrix = new List<string>{ "abcdc",
                                           "fgwio",
                                           "chill",
                                           "pqnsd",
                                           "uvdxy" };
            var finder = new WordFinder.WordFinder(matrix);

            // Act
            var expected = new List<string>();
            var actual = finder.Find(new[] { "not", "found" });

            // Assert 
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}