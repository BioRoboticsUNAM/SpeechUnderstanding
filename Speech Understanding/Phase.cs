using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Recognition;
using System.Threading;

namespace SpeechUnderstanding
{
	public class Phase1
	{
		private static StreamWriter resultsFile;
		private static readonly Queue<string> pendingWavFiles;
		private SpeechRecognitionEngine engine;
		private Grammar grammar;
		private CFRInterpreter interpreter;

		static Phase1()
		{
			Phase1.pendingWavFiles = new Queue<string>();
		}

		private Phase1() : this(Path.Combine("Grammars", "grammar.xml")) { }

		private Phase1(string grammarFilePath)
		{	LoadGrammar(grammarFilePath);	
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

		public static void Run(string grammarFilePath, string ioPath)
		{
			Console.WriteLine("Running Phase 1");
			GetWaveFiles(ioPath);
			lock (pendingWavFiles) Console.WriteLine("\t{0} files found", pendingWavFiles.Count);
			InitializeResultsFile(ioPath);
			Console.WriteLine("\tresults.txt file initialized");
			int numWorkers = Math.Min(4, Math.Max(Environment.ProcessorCount, 1));
			List<Thread> workers = new List<Thread>(numWorkers);
			Console.WriteLine("\tRunning recognition with {0} threads", numWorkers);
			for (int i = 0; i < numWorkers; ++i)
			{
				Thread worker = new Thread(new ParameterizedThreadStart(WorkerTask));
				workers.Add(worker);
				worker.IsBackground = true;
				worker.Start();
			}
			for (int i = 0; i < workers.Count; ++i)
				workers[i].Join();
			CloseResultsFile();
			Console.WriteLine("Done!");
		}

		private static void GetWaveFiles(string path)
		{
			string[] wavFiles  = Directory.GetFiles(path, "*.wav", SearchOption.TopDirectoryOnly);
			lock (pendingWavFiles)
			{
				foreach (string waveFile in wavFiles)
					pendingWavFiles.Enqueue(waveFile);
			}
		}

		private static void WorkerTask(object grammarFilePath)
		{
			Phase1 p = new Phase1((string)grammarFilePath);
			while (true)
			{
				string waveFile;
				lock (pendingWavFiles)
				{
					if (pendingWavFiles.Count < 1)
						return;
					waveFile = pendingWavFiles.Dequeue();
				}
				p.ProcessWaveFile(waveFile);
			}
		}

		private void ProcessWaveFile(string waveFile)
		{
			this.engine.SetInputToWaveFile(waveFile);
			string transcript = this.engine.Recognize().Text;
			string cfr = interpreter.Interpret(transcript);
			resultsFile.WriteLine("{0} {1} {2}", waveFile, transcript, cfr);
		}

		private static void CloseResultsFile()
		{
			resultsFile.Flush();
			resultsFile.Close();
			resultsFile = null;
		}

		private static void InitializeResultsFile(string path)
		{
			resultsFile = new StreamWriter(Path.Combine(path, "results.txt"), true);
			resultsFile.WriteLine("Team: Pumas | Phase 1 | {0}", DateTime.Now.ToString("U"));
		}
	}
}
