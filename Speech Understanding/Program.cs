using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.IO;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;

namespace SpeechUnderstanding
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Console.WriteLine("Speech Understanding Functionality Benchmark"); 
			Console.WriteLine("RoCKIn 2015, Lisbon");
			Console.WriteLine();

			// new CFRInterpreter().Interpret("Bring me the milk");
			// GrammarTest();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmGUI());
		}

		private static void GrammarTest()
		{
			try
			{
				RecognitionResult result;
				ConsoleColor cc = Console.ForegroundColor;

				new System.Xml.XmlDocument().Load("Grammars\\grammar.xml");
				// Stream cfgFile = File.Open("Grammars\\grammar.cfg", FileMode.Create, FileAccess.Write);
				// SrgsGrammarCompiler.Compile("Grammars\\grammar.xml", cfgFile);
				// cfgFile.Flush();
				// cfgFile.Close();
				Grammar grammar = new Grammar("Grammars\\grammar.xml");
				Grammar grammarAlt = new Grammar("Grammars\\grammar.xml", "sentenceAlt");
				grammarAlt.Enabled = false;
				SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
				engine.LoadGrammar(grammar);
				engine.LoadGrammar(grammarAlt);

				string[] lines = System.IO.File.ReadAllLines(@"Z:\Robocup Apps\Speech Understanding\AllTranscriptions.txt");
				Console.BufferHeight = lines.Length;
				int perfect = 0;
				int ok = 0;

				Console.WriteLine("Runnig grammar test...");
				result = engine.EmulateRecognize("bring slowly the box near the counter of the kitchen");
				Console.Write("\t0 of {0}\r", lines.Length);
				//for (int i = 0; i < 200; ++i)
				for (int i = 0; i < lines.Length; ++i)
				{
					if (String.IsNullOrEmpty(lines[i]))
						continue;
					result = engine.EmulateRecognize(lines[i]);
					if (result == null)
					{
						grammarAlt.Enabled = !grammarAlt.Enabled;
						if (grammarAlt.Enabled)
						{
							--i;
							continue;
						}
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("\tFailed: {0}", lines[i]);
						Console.ForegroundColor = cc;
						Console.Write("\t{0} of {1}\r", i, lines.Length);
						continue;
					}

					grammarAlt.Enabled = false;
					if (String.Compare(lines[i], result.Text, true) == 0) ++perfect;
					else if (result.Confidence > 0.8)
					{
						Console.ForegroundColor = ConsoleColor.DarkYellow;
						Console.WriteLine("\tAprox:  {0}", lines[i]);
						Console.ForegroundColor = cc;
						++ok;
					}
					Console.Write("\t{0} of {1}\r", i, lines.Length);
				}
				Console.Write("\t\t\t\r");
				Console.WriteLine("\rGrammar test results:");
				Console.WriteLine("\tPerfect:    {0}/{1}", perfect, lines.Length);
				Console.WriteLine("\tAcceptable: {0}/{1}",ok, lines.Length);
				Console.WriteLine("\tTotal:      {0}/{1}", perfect + ok, lines.Length);
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
		}
	}
}
