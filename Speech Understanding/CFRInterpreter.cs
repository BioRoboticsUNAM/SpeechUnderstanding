using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SpeechUnderstanding
{
	public class CFRInterpreter
	{
		/// <summary>
		/// Gets or sets the path tho the py interpreter file 
		/// </summary>
		public static string InterpreterScript { get; set; }
		/// <summary>
		/// Gets or sets the Python executable path
		/// </summary>
		public static string PytonPath{ get; set; }

		/// <summary>
		/// The python process
		/// </summary>
		private Process python;

		static CFRInterpreter()
		{
			PytonPath = @"C:\Python\2.7\python.exe";
			InterpreterScript = @"interpreter\main.py";
		}

		public CFRInterpreter()
		{
			FileInfo scriptFileInfo = new FileInfo(InterpreterScript);
			python = new Process();
			ProcessStartInfo psi = new ProcessStartInfo(PytonPath, scriptFileInfo.Name);
			psi.RedirectStandardError = true;
			psi.RedirectStandardInput = true;
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = false;
			psi.WorkingDirectory = scriptFileInfo.DirectoryName;
			python.StartInfo = psi;
			python.Start();
			python.StandardInput.WriteLine();
			python.StandardInput.WriteLine();
		}

		public string Interpret(string transcript)
		{
			try
			{
				// return String.Empty;
				// Console.WriteLine(python.StandardOutput.ReadToEnd());
				Discard(); Discard(); Discard();
				python.StandardInput.WriteLine(transcript);
				string result = python.StandardOutput.ReadLine();
				return result;
			}
			catch { return String.Empty; }
		}

		private void Discard()
		{

			while (python.StandardOutput.Peek() > -1)
			{
				Console.Write((char)python.StandardOutput.Read());
			}
		}
	}
}
