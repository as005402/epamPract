using System.Collections.Generic;

namespace epamPract
{
    class jsonObj
    {
        public string fileName { get; set; }
        public long fileSize { get; set; } = 0;
        public int lettersCount { get; set; } = 0;
        public Dictionary<char, int> letters { get; set; } = new Dictionary<char, int>();
        public int wordsCount { get; set; } = 0;
        public Dictionary<string, int> words { get; set; } = new Dictionary<string, int>();
        public int linesCount { get; set; } = 0;
        public int digitsCount { get; set; } = 0;
        public int numbersCount { get; set; } = 0;
        public string longestWord { get; set; }
        public int wordsWithHyphen { get; set; } = 0;
        public int punctuation { get; set; } = 0;
    }
}
