using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Recognition;
using System.Threading;

namespace SpeechUnderstanding
{
	public abstract class Phase
	{
		protected SpeechRecognitionEngine engine;
		protected Grammar grammar;
		protected Grammar grammarAlt;
		protected CFRInterpreter interpreter;
		private static StreamWriter resultsFile;
		private static object resultsFileLock;

		static Phase()
		{
			resultsFileLock = new Object();
		}

		protected Phase() : this(Path.Combine("Grammars", "grammar.xml")) { }

		protected Phase(string grammarFilePath)
		{
			this.engine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-GB"));
			LoadGrammar(grammarFilePath);
			LoadGrammarAlt(grammarFilePath);
			this.interpreter = new CFRInterpreter();
		}

		protected void LoadGrammar(string grammarFilePath)
		{
			try
			{
				this.grammar = new Grammar(grammarFilePath);

			}
			catch(Exception ex)
			{
				Console.WriteLine("Error while loading grammar. DictationGrammar will be used");
				this.grammar = new DictationGrammar();
				ex.ToString();
			}
			this.engine.LoadGrammar(grammar);
		}

		protected abstract void LoadGrammarAlt(string grammarFilePath);

		protected void LoadGrammarAlt(SpeechRecognitionEngine engine, string grammarFilePath)
		{
			try
			{
				grammarAlt = new Grammar(grammarFilePath, "sentenceAlt");
			}
			catch
			{
				grammarAlt = new DictationGrammar();
			}
			grammarAlt.Enabled = false;
			engine.LoadGrammar(grammarAlt);
		}

		protected static void CloseResultsFile()
		{
			lock (resultsFileLock)
			{
				resultsFile.Flush();
				resultsFile.Close();
				resultsFile = null;
			}
		}

		protected static void InitializeResultsFile(string path, int phase)
		{
			lock (resultsFileLock)
			{
				resultsFile = new StreamWriter(Path.Combine(path, "results.txt"), true);
				resultsFile.WriteLine("Team: Pumas :: Phase {0} :: {1}", phase, DateTime.Now.ToString("U"));
			}
		}

		protected static void WriteResultsFile(string waveFile, string transcript, string cfr)
		{
			if (String.IsNullOrEmpty(transcript) || (transcript.Trim().Length < 1))
				transcript = "BAD_RECOGNITION";
			lock (resultsFileLock)
			{
				resultsFile.WriteLine("{0}|{1}|{2}", waveFile, transcript, cfr);
				resultsFile.Flush();
			}
		}
	}
}
