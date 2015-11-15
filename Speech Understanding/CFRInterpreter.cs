using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SpeechUnderstanding
{
	public class CFRInterpreter
	{
		/// <summary>
		/// The "NO_INTERPRETATION" string
		/// </summary>
		public const string NO_INTERPRETATION = "NO_INTERPRETATION";
		
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

		/// <summary>
		/// Event for waiting python output to be ready
		/// </summary>
		private AutoResetEvent pythonOutputWaitHandle;

		/// <summary>
		/// Stores the python output
		/// </summary>
		private string output;

		/// <summary>
		/// Object to lock the access to the output string
		/// </summary>
		private readonly object outputLock;

		static CFRInterpreter()
		{
			PytonPath = @"C:\Python\2.7\python.exe";
			InterpreterScript = @"interpreter\main.py";
		}

		public CFRInterpreter()
		{
			outputLock = new Object();
			pythonOutputWaitHandle = new AutoResetEvent(false);
			SetupProcess();
		}

		~CFRInterpreter()
		{
			if ((python != null) && !python.HasExited)
			{
				python.CancelOutputRead();
				python.CancelErrorRead();
				python.Kill();
				python.WaitForExit();
				python.Close();
			}
		}

		private void SetupProcess()
		{
			FileInfo scriptFileInfo = new FileInfo(InterpreterScript);
			python = new Process();
			ProcessStartInfo psi = new ProcessStartInfo(PytonPath, String.Format("-i {0}", scriptFileInfo.Name));
			psi.RedirectStandardError = true;
			psi.RedirectStandardInput = true;
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = false;
			psi.WorkingDirectory = scriptFileInfo.DirectoryName;
			python.StartInfo = psi;
			python.OutputDataReceived += new DataReceivedEventHandler(DataReceivedFromPythonOutput);
			python.ErrorDataReceived += new DataReceivedEventHandler(DataReceivedFromPythonError);
			python.EnableRaisingEvents = true;				

			python.Start();
			python.BeginErrorReadLine();
			python.BeginOutputReadLine();
		}

		private void DataReceivedFromPythonOutput(object sender, DataReceivedEventArgs e)
		{
			lock (outputLock)
			{
				output = e.Data;
				pythonOutputWaitHandle.Set();
			}
		}

		private void DataReceivedFromPythonError(object sender, DataReceivedEventArgs e)
		{
			lock (outputLock)
			{
				output = NO_INTERPRETATION;
				pythonOutputWaitHandle.Set();
			}
		}

		public string Interpret(string transcript)
		{
			try
			{
				python.StandardInput.WriteLine(transcript);
				if(!pythonOutputWaitHandle.WaitOne(1000))
					return NO_INTERPRETATION;
				lock (outputLock)
				{
					return output;
				}
			}
			catch { return NO_INTERPRETATION; }
		}
	}
}
