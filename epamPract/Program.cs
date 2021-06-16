using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace epamPract
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which file you'd like to parse?");
            jsonObj obj = new jsonObj();
            obj.fileName = Console.ReadLine().Trim();   // "test.haha"
            try
            {
                using (StreamReader sr = new StreamReader(obj.fileName, System.Text.Encoding.Default))
                {
                    string line;
                    string pattern = @"\W+|(\d+,\d+)|(\d+\.\d+)|(\w+-\w+)";
                    obj.fileSize = sr.BaseStream.Length;

                    while ((line = sr.ReadLine()) != null)
                    {
                        obj.linesCount++;

                        obj.punctuation += Regex.Matches(line, @"\.|,|:|;|'|""").Count();

                        var someWords = Regex.Split(line, pattern, RegexOptions.IgnoreCase).Where(a =>
                        {
                            foreach (char z in a) if (!char.IsLetter(z) && z != '-') return false;
                            return true;
                        }).Where(z => z != "");

                        foreach (string z in someWords)
                        {
                            if (!obj.words.ContainsKey(z)) obj.words.Add(z, 1);
                            else obj.words[z]++;
                            if (z.Contains('-')) obj.wordsWithHyphen++;
                        }
                        obj.wordsCount += someWords.Count();

                        foreach (char z in line)
                        {
                            if (char.IsLetter(z) && !obj.letters.ContainsKey(z)) obj.letters.Add(z, 1);
                            else if (char.IsLetter(z)) obj.letters[z]++;
                        }

                        obj.digitsCount += line.Where(a => char.IsDigit(a)).Count();
                        obj.numbersCount += Regex.Split(line, pattern, RegexOptions.IgnoreCase).Where(a =>
                        {
                            foreach (char z in a) if (!char.IsDigit(z) && z != '.' && z != ',') return false;
                            return true;
                        }).Where(z => z != "").Count();

                    }

                }
            }catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return;
            }
            
            obj.lettersCount = obj.letters.Values.Sum();
            obj.letters = obj.letters.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); ;
            obj.words = obj.words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            obj.longestWord = obj.words.Keys.Where(k => k.Length == (obj.words.Keys.Select(key => key.Length).Max())).First();

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                IgnoreNullValues = true,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize<jsonObj>(obj, options);
            
            try
            {
                File.WriteAllText($"{obj.fileName}.json", json);
            } catch (Exception e)
            {
                Console.WriteLine($"Oopsie smth went wrong\n{e.Message}");
            }
        }
        
    }
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
