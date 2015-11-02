using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace SpeechUnderstanding
{
	public class CFRInterpreter
	{
		private readonly ScriptEngine engine;
		private readonly ScriptScope scope;
		private readonly Func<string, string> interpreterFunction;

		/// <summary>
		/// Gets or sets the path of the interpreter directory
		/// </summary>
		public static string InterpreterPath { get; set; }
		/// <summary>
		/// Gets or sets the name of the py interpreter file 
		/// </summary>
		public static string InterpreterScript { get; set; }
		/// <summary>
		/// Gets or sets the name of the interpreter function in the interpreter script file
		/// </summary>
		public static string InterpreterFunction { get; set; }
		/// <summary>
		/// Gets or sets the Python library path
		/// </summary>
		public static string LibPath { get; set; }

		static CFRInterpreter()
		{
			// LibPath = @"C:\Program Files (x86)\IronPython 2.7\Lib";
			LibPath = @"C:\Python\2.7\Lib";
			// InterpreterScript = @"Z:\Robocup Apps\interprete_lenguaje_egsr_tmr_2015\egprs_interpreter.py";
			// InterpreterFunction = "interpret_command";
			InterpreterPath = @"interpreter";
			InterpreterScript = @"egprs_interpreter.py";
			InterpreterFunction = "interpret_command";
		}

		public CFRInterpreter()
		{
			engine = Python.CreateEngine();
			SetupEnginePaths();
			// Works but sometims crashes. Maybe mutual exclusion issue.
			scope = engine.ExecuteFile(InterpreterScript);
			interpreterFunction = scope.GetVariable<Func<string, string>>(InterpreterFunction);
		}

		private void SetupEnginePaths()
		{
			FileInfo scriptFileInfo = new FileInfo(Path.Combine(InterpreterPath, InterpreterScript));
			InterpreterPath = scriptFileInfo.DirectoryName;
			InterpreterScript = scriptFileInfo.Name;
			engine.Runtime.Host.PlatformAdaptationLayer.CurrentDirectory = InterpreterPath;
			ICollection<string> paths = engine.GetSearchPaths();
			// paths.Add(scriptFileInfo.DirectoryName);
			// DirectoryInfo[] scriptPathSubDirs = scriptFileInfo.Directory.GetDirectories("*", SearchOption.AllDirectories);
			// foreach (DirectoryInfo subdir in scriptPathSubDirs)
			//	paths.Add(subdir.FullName);

			DirectoryInfo libPathInfo = new DirectoryInfo(LibPath);
			paths.Add(libPathInfo.FullName);
			DirectoryInfo[] libPathSubDirs = libPathInfo.GetDirectories("*", SearchOption.AllDirectories);
			foreach (DirectoryInfo subdir in libPathSubDirs)
				paths.Add(subdir.FullName);
			engine.SetSearchPaths(paths);
		}

		public string Interpret(string transcript)
		{
			try
			{
				return interpreterFunction(transcript);
			}
			catch { return String.Empty; }
		}
	}
}
