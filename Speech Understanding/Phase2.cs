using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Recognition;
using System.Threading;

namespace SpeechUnderstanding
{
	public class Phase2 : Phase
	{
		private string ioPath;
		private int audioFileIndex;

		protected Phase2() { }

		protected Phase2(string grammarFilePath)
			: base(grammarFilePath)
		{
			this.engine.SetInputToDefaultAudioDevice();
			this.engine.EndSilenceTimeout = TimeSpan.FromSeconds(3);
			this.engine.InitialSilenceTimeout = TimeSpan.FromSeconds(2);
			this.engine.MaxAlternates = 1;
			this.engine.SpeechRecognized+=new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
			this.engine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(SpeechRecognitionRejected);
			this.audioFileIndex = 1;
		}

		public static Phase2 Run(string grammarFilePath, string ioPath)
		{
			Console.WriteLine("Running Phase 2");
			InitializeResultsFile(ioPath);
			Console.WriteLine("\tresults.txt file initialized");
			Phase2 p = new Phase2(grammarFilePath);
			p.Start();
			p.ioPath = ioPath;
			return p;
		}

		public void Start()
		{
			this.engine.RecognizeAsync(RecognizeMode.Multiple);
		}

		public void Stop()
		{
			engine.RecognizeAsyncCancel();
			CloseResultsFile();
		}

		private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			try
			{
				string waveFile = SaveWaveFile(e.Result.Audio);
				string transcript = e.Result.Text;
				string cfr = interpreter.Interpret(transcript);
				Console.WriteLine("\tPhase 2: {0}", transcript);
				WriteResultsFile(waveFile, transcript, cfr);
			}
			catch { }
		}

		private void SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			try
			{
				string waveFile = SaveWaveFile(e.Result.Audio);
				Console.WriteLine("\tPhase 2: BAD RECOGNITION");
				WriteResultsFile(waveFile, "BAD_RECOGNITION", "NO_INTERPRETATION");
			}
			catch { }
		}

		private string SaveWaveFile(RecognizedAudio recognizedAudio)
		{
			string fileName = string.Format("fb_mic_phase_speech_audio_{0}.wav", audioFileIndex++);
			try
			{
				using (FileStream fs = File.OpenWrite(Path.Combine(ioPath, fileName)))
				{
					recognizedAudio.WriteToWaveStream(fs);
				}
			}
			catch { }
			return fileName;
		}

		protected static void InitializeResultsFile(string path)
		{
			Phase.InitializeResultsFile(path, 1);
		}
	}
}
