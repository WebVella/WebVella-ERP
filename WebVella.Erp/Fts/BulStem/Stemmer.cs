using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WebVella.Erp.Fts.BulStem
{
	public class Stemmer
	{
		Hashtable _stemmingRules = new Hashtable();
		int STEM_BOUNDARY = 1;
		static Regex vocals = new Regex("[^аъоуеияю]*[аъоуеияю]", RegexOptions.Compiled);
		static Regex p = new Regex("([а-я-]+)\\s==>\\s([а-я]+)\\s([0-9]+)", RegexOptions.Compiled);

		StemmingLevel _level;
		public StemmingLevel Level
		{
			get { return _level; }
		}

		/// <summary>
		/// Constructor with stemming level
		/// </summary>
		/// <param name="level">Stemming level</param>
		public Stemmer(StemmingLevel level = StemmingLevel.Low)
		{
			_level = level;
			SetLevel(level);
		}

		/// <summary>
		/// Set current stemming level - reloads stemming rules
		/// </summary>
		/// <param name="level">Stemming level</param>
		public void SetLevel(StemmingLevel level)
		{
			LoadStemmingRules(level);
		}

		/// <summary>
		/// Loads stemming rules depending of stemming level
		/// </summary>
		/// <param name="level"></param>
		private void LoadStemmingRules(StemmingLevel level)
		{
			switch (level)
			{
				case StemmingLevel.Low:
					LoadStemmingRulesFromEmbeddedResource("WebVella.Erp.Fts.BulStem.Rules.stem_rules_context_1_utf8.txt");
					break;
				case StemmingLevel.Medium:
					LoadStemmingRulesFromEmbeddedResource("WebVella.Erp.Fts.BulStem.Rules.stem_rules_context_2_utf8.txt");
					break;
				case StemmingLevel.High:
					LoadStemmingRulesFromEmbeddedResource("WebVella.Erp.Fts.BulStem.Rules.stem_rules_context_3_utf8.txt");
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Loads stemming rules from embedded resources
		/// </summary>
		/// <param name="resourceName">Embedded resource name</param>
		/// <example >LoadStemmingRulesFromEmbeddedResource("BulStem.Rules.stem_rules_context_1_utf8.txt");</example>   
		private void LoadStemmingRulesFromEmbeddedResource(String resourceName)
		{
			_stemmingRules.Clear();
			var assembly = Assembly.GetExecutingAssembly();
			using (Stream resTream = assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader br = new StreamReader(resTream, new UTF8Encoding(true, true)))
				{
					String s = null;
					while ((s = br.ReadLine()) != null)
					{
						Match m = p.Match(s);
						if (m.Success)
						{

							int groupsCount = m.Groups.Count;
							if (groupsCount == 4)
							{
								if (int.Parse(m.Groups[3].Value) > STEM_BOUNDARY)
								{
									_stemmingRules.Add(m.Groups[1].Value, m.Groups[2].Value);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Stem given word
		/// </summary>
		/// <param name="word">Word to stem</param>
		/// <returns>Stem for the given word if applicable or word itself if not</returns>
		public String Stem(String word)
		{
			string wordLowered = word.ToLower();
			word = GetSubstitutionWord(word);

			Match vocalMatch = vocals.Match(wordLowered, 0);
			if (!vocalMatch.Success)
			{
				return wordLowered;
			}

			int matchEnd = vocalMatch.Index + vocalMatch.Captures[0].Value.Length;
			for (int i = matchEnd + 1; i < wordLowered.Length; i++)
			{
				String suffix = wordLowered.Substring(i);

				string newSuffix = (String)_stemmingRules[suffix];
				if (newSuffix != null)
				{
					return wordLowered.Substring(0, i) + newSuffix;
				}
			}
			return wordLowered;
		}

		public String StemText(String text)
		{
			List<string> words = text.Split(new[] { ' ', '-', '.', ',', '!', '?', ';', ':', '@', '&' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			string resultText = "";
			foreach (var word in words)
			{
				resultText += Stem(word) + " ";
			}

			if (resultText.Length > 1)
				resultText = resultText.Remove(resultText.Length - 1);

			return resultText;
		}

		private string GetSubstitutionWord(string word) {
			var wordDictionary = new Dictionary<string,string>();

			#region << Dictionary definition >>
			//Those dictionary has substitute words for better search, and fine tuning of the stemmer. 
			//Example: word "черен" is stemmed to "черен", but "черно" is stemmed to "черн". In this case searching 
			//for "черен" will not find the stemmed "черн"
			wordDictionary["черно"] = "черен";
			wordDictionary["черна"] = "черен";
			wordDictionary["черни"] = "черен";
			#endregion

			if (wordDictionary.ContainsKey(word)) {
				return wordDictionary[word];
			}
			return word;
		}

	}

}
