using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmGUI());
		}
	}
}
