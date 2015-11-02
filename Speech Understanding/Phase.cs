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
			LoadGrammar(grammarFilePath);	
			this.engine = new SpeechRecognitionEngine();
			this.engine.LoadGrammar(grammar);
			this.interpreter = new CFRInterpreter();
		}

		protected void LoadGrammar(string grammarFilePath)
		{
			try
			{
				this.grammar = new Grammar(grammarFilePath);

			}
			catch
			{
				Console.WriteLine("Error while loading grammar. DictationGrammar will be used");
				this.grammar = new DictationGrammar();
			}
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
				resultsFile.WriteLine("Team: Pumas | Phase {0} | {1}", phase, DateTime.Now.ToString("U"));
			}
		}

		protected static void WriteResultsFile(string waveFile, string transcript, string cfr)
		{
			lock (resultsFileLock)
			{
				resultsFile.WriteLine("{0} {1} {2}", waveFile, transcript, cfr);
			}
		}
	}
}
