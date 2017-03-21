using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovDutchNameGenerator
{
    public class MarkovNameGenerator
    {
        // a random number generator
        private Random rnd = new Random();

        // order (or length) of each ngram
        private int order;
        // what is the maximum amount we will generate?
        private int max;

        // each ngram is the key, a list of possible next elements are the values
        private Dictionary<string, List<char>> ngrams = new Dictionary<string, List<char>>();

        // a separate list of possible beginnings to generated text
        private List<string> beginnings = new List<string>();


        public MarkovNameGenerator(int order, int max)
        {
            this.order = order;
            this.max = max;
        }


        /// <summary>
        /// Feed new text to the Markov chain
        /// </summary>
        /// <param name="text"></param>
        void Feed(string text)
        {
            // discard this line if it's too short
            if (text.Length < order)
                return;

            // store the first ngram of this line
            string beginning = text.Substring(0, order);
            beginnings.Add(beginning);

            // now let's go through everything and create the dictionary
            for (int i = 0; i < text.Length - order; i++)
            {
                string gram = text.Substring(i, order);
                char next = text[i + order];

                // is this a new one?
                if (!ngrams.TryGetValue(gram, out List<char> nextChars))
                {
                    nextChars = new List<char>();
                    ngrams[gram] = nextChars;
                }
                nextChars.Add(next);
            }
        }

        /// <summary>
        /// Feed each line in a file to the Markov chain
        /// </summary>
        /// <param name="path"></param>
        public void FeedFile(string path)
        {
            var lines = System.IO.File.ReadAllLines(path, Encoding.UTF8);
            foreach (string line in lines)
                Feed(line);
        }

        /// <summary>
        /// Generate a text from the information ngrams
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            // get a random beginning
            string current = beginnings[rnd.Next(beginnings.Count)];
            string output = current;

            // generate a new token max number of times
            for (int i = 0; i < max; i++)
            {
                // if this is a valid ngram
                if (ngrams.TryGetValue(current, out List<char> possibleNext))
                {
                    // possibleNext is all possible next tokens
                    // pick one randomly
                    char next = possibleNext[rnd.Next(possibleNext.Count)];
                    // add to the output
                    output += next;
                    // get the last N entries of the output; we'll use this
                    // to look up an ngram in the next iteration of the loop
                    current = output.Substring(output.Length - order);
                }
                else
                {
                    // we don't have a valid ngram, so we're done here
                    break;
                }
            }

            return output;
        }
    }
}
