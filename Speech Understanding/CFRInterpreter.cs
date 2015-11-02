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
		/// Gets or sets the Python path
		/// </summary>
		public static string PytonPath{ get; set; }

		static CFRInterpreter()
		{
			PytonPath = @"C:\Python\2.7\python.exe";
			InterpreterScript = @"interpreter\egprs_interpreter.py";
		}

		public CFRInterpreter()
		{
			
		}

		public string Interpret(string transcript)
		{
			try
			{
				return String.Empty;
			}
			catch { return String.Empty; }
		}
	}
}
