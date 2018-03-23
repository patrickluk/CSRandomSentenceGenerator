using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovChain
{
	public class Program
	{
		// passage text for the markov chain
		// passage can also be imported from a text file
		static string passage = "there is a type of thing that tries to do a thing that something can do and so" + 
			" it does not mean 'do' in the sense of what it has thing to do then it means" + 
			" 'do' if you give it some time to do what it has to, then you will get something and" + 
			" this gets hard because you have to know what that means, which is a thing" + 
			" it self that you can't do where the good thing is that there is a thing that if" + 
			" you can think of how to get a thing in you, you can get a thing to do it for you then" + 
			" a thing think of this, and think a good way to do what you can do" + 
			" if you can get something good then you have things to do because things can be good. " + 
			" so if you can't think that hard, and there can be something that it can't do. " + 
			" then the thing will be good in that thing and also a thing to do. ";
		
		static Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>(); // dictionary for storing markov chain
		static List<string> keys; // list of keys in the dictionary
		static Random rnd = new Random(); // for generating random things

		// main function
		public static void Main(string[] args) {
			InitializeWordList();            
			for (int i = 0; i < 15; i++) Console.WriteLine(RandomSentence(10, 20)); // generate 15 random sentences with length 10 to 20
		}

		// function for initializing the random sentence generator
		static void InitializeWordList() {

			// generate word list from the passage
			passage = new string(passage.Where(
				c => !char.IsPunctuation(c) || c.Equals('/') || c.Equals('\'') || c.Equals('-')).ToArray()); // remove punctuations from passage text
			var words = passage.Replace('/', ' ').Replace('-', ' ').Split(' ').ToList(); // get list of word from the passage

			// clean up the word list
			for (int i = words.Count - 1; i >= 0; i--) {
				words[i] = words[i].Trim(new char[] {'\''}).ToLower(); // remove single quotations
				if (words[i].Equals("i")) words[i] = "I"; // capitalizing the word "i"
				if (string.IsNullOrWhiteSpace(words[i])) words.RemoveAt(i); // remove empty string words
			}

			// generate markov chain dictionary
			for (int i = 0; i < words.Count - 2; i++) {
				var word1 = words[i]; // first word of the key
				var word2 = words[i + 1]; // second word of the key
				var key = word1 + " " + word2; // the key of the dictionary
				var wordNext = words[i + 2]; // a possible value of the key
				if (dict.ContainsKey(key)) dict[key].Add(wordNext); // add value to key if the key exists in the dictionary
				else dict.Add(key, new List<string>() { wordNext }); // create a new key if the key is not in the dictionary
			}

			keys = Enumerable.ToList(dict.Keys); // get list of keys from the dictionary
		}

		// function for generating a random sentence with random length
		static string RandomSentence(int minWords, int maxWords) { // minimum/maximum number of words of the sentence

			int numWords = rnd.Next(minWords, maxWords + 1);// set the number of words of the sentence

			string word1 = ""; // initializing the first word for the markov chain
			string word2 = ""; // initializing the second word for the markov chain
			StringBuilder result = new StringBuilder(); // the resulting sentence

			// walk through the markov chain
			for (int i = 0; i < numWords; i++) {

				if (i > 0) result.Append(" "); // add space after each word in result string

				var key = word1 + " " + word2; // key of the dictionary

				// get random words from the dictionary when the chain ends
				if (!dict.ContainsKey(key)) {
					var randomIndex = rnd.Next(keys.Count); // get random index
					key = keys[randomIndex]; // get random key
					word1 = key.Split()[0]; // get first word from the key
					word2 = key.Split()[1]; // get second word from the key
				}

				// get list of possible words from the dictionary
				var possibleWords = dict[key];

				// add word to string
				result.Append(word1);

				// move to the next state of the markov chain
				word1 = word2;
				word2 = possibleWords[rnd.Next(possibleWords.Count)];
			}

			return result.ToString();
		}

	}
}
