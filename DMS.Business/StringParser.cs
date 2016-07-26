using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DMS.Business
{
    public class StringParser
    {
        private string sourceText { get; set; }

        public StringParser(string sourceText)
        {
            this.sourceText = sourceText;
        }

        private List<string> SplitSentences()
        {
            // create a local string variable
            // set to contain the string passed it
            string sTemp = sourceText;

            // create the array list that will
            // be used to hold the sentences
            List<string> al = new List<string>();

            // split the sentences with a regular expression
            string[] splitSentences =
                Regex.Split(sTemp, @"(?<=['""A-Za-z0-9][\.\!\?])\s+(?=[A-Z])");

            // loop the sentences
            for (int i = 0; i < splitSentences.Length; i++)
            {
                // clean up the sentence one more time, trim it,
                // and add it to the array list
                string sSingleSentence =
                    splitSentences[i].Replace(Environment.NewLine, string.Empty);
                al.Add(sSingleSentence.Trim());
            }

            // return the arraylist with
            // all sentences added
            return al;
        }

        private List<string> ReasonableParser()
        {
            List<string> al = new List<string>();

            // get a string from the contents of a textbox
            string sTemp = sourceText;
            sTemp = sTemp.Replace(Environment.NewLine, " ");

            // split the string using sentence terminations
            char[] arrSplitChars = { '.', '?', '!' };  // things that end a sentence

            //do the split
            string[] splitSentences = sTemp.Split(arrSplitChars, StringSplitOptions.RemoveEmptyEntries);

            // loop the array of splitSentences
            for (int i = 0; i < splitSentences.Length; i++)
            {
                // find the position of each sentence in the
                // original paragraph and get its termination ('.', '?', '!') 
                int pos = sTemp.IndexOf(splitSentences[i]);
                char[] arrChars = sTemp.Trim().ToCharArray();
                char c = arrChars[pos + splitSentences[i].Length];

                // since this approach looks only for the first instance
                // of the string, it does not handle duplicate sentences
                // with different terminations.  You could expand this
                // approach to search for later instances of the same
                // string to get the proper termination but the previous
                // method of using the regular expression to split the
                // string is reliable and less bothersome.

                // add the sentences termination to the end of the sentence
                al.Add(splitSentences[i].ToString().Trim() + c.ToString());
            }

            return al;
        }
        private List<string> IDontCareHowItEndsParser()
        {
            string sTemp = sourceText;
            sTemp = sTemp.Replace(Environment.NewLine, " ");

            // split the string using sentence terminations
            char[] arrSplitChars = { '.', '?', '!' };  // things that end a sentence

            //do the split
            string[] splitSentences = sTemp.Split(arrSplitChars, StringSplitOptions.RemoveEmptyEntries);

            List<string> al = new List<string>();
            for (int i = 0; i < splitSentences.Length; i++)
            {
                splitSentences[i] = splitSentences[i].ToString().Trim();
                al.Add(splitSentences[i]);
            }

            return al;
        }
        public int GenerateCharacterCount()
        {
            int rtn = 0;

            // clean up the string by
            // removing newlines and by trimming
            // both ends
            string sTemp = sourceText;
            sTemp = sTemp.Replace(Environment.NewLine, string.Empty);
            sTemp = sTemp.Trim();

            // split the string into sentences 
            // using a regular expression
            string[] splitSentences =
                Regex.Split(sTemp,
                    @"(?<=['""A-Za-z0-9][\.\!\?])\s+(?=[A-Z])");

            // loop through the sentences to get character counts
            for (int cnt = 0; cnt < splitSentences.Length; cnt++)
            {
                // get the current sentence
                string sSentence = splitSentences[cnt].ToString();

                // trim it
                sSentence = sSentence.Trim();

                // convert it to a character array
                char[] sentence = sSentence.ToCharArray();

                // test each character and
                // add it to the return value
                // if it passes
                for (int i = 0; i < sentence.Length; i++)
                {
                    // make sure it is a letter, number,
                    // punctuation or whitespace before
                    // adding it to the tally
                    if (char.IsLetterOrDigit(sentence[i]) ||
                        char.IsPunctuation(sentence[i]) ||
                        char.IsWhiteSpace(sentence[i]))
                        rtn += 1;
                }
            }

            // return the final tally
            return rtn;
        }

        public int GenerateWordCount(Func<List<string>> action)
        {
            // declare a return value
            int rtn = 0;

            var sentences = action();

            // iterate through the entire list
            // of sentences
            foreach (string sSentence in sentences)
            {
                // define an empty space as the split
                // character
                char[] arrSplitChars = { ' ' };

                // create a string array and populate
                // it with a split on the current sentence;
                // use the string split option to remove
                // empty entries so that empty sentences do not
                // make it into the word count.
                var arrWords = sSentence.Split(arrSplitChars, StringSplitOptions.RemoveEmptyEntries);
                rtn += arrWords.Length;
            }

            // return the final word count
            return rtn;
        }

        public int GenerateSentenceCount(Func<List<string>> action)
        {
            // create a return value
            int rtn = 0;

            var sentences = action();
            // set the return value to
            // the length of the sentences array
            rtn = sentences.Count;

            // return the count
            return rtn;
        }

        public List<string> SplitByCount(int count)
        {
            var charCount = GenerateCharacterCount();
            var sentences = SplitSentences();
            var avgCharCountByPart = charCount / count;

            var result = new List<string>();

            var sum = 0;
            var lastSentenceCount = 0;
            for (int j = 0; j < sentences.Count; j++)
            {
                sum += sentences[j].Length;

                if (sum > avgCharCountByPart)
                {
                    result.Add(string.Join(" ", sentences.Skip(lastSentenceCount).Take(j - lastSentenceCount)));
                    lastSentenceCount = j;
                    sum = 0;
                }
            }
            result.Add(string.Join(" ", sentences.Skip(lastSentenceCount).Take(sentences.Count - lastSentenceCount)));

            return result;
        }
    }
}
