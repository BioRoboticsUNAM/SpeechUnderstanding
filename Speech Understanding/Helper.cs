using System;
using System.IO;
using System.Collections.Generic;


namespace SpeechUnderstanding
{
	public static class Helper
	{

		public static List<string> GetRemvableDriveList(){
			List<string> fullNames = new List<String>();
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.DriveType != DriveType.Removable)
					continue;
				fullNames.Add(driveInfo.RootDirectory.FullName);
			}
			return fullNames;
		}

		public static string GetTranscript(System.Speech.Recognition.RecognitionResult result)
		{
			string transcript = (result == null) ? String.Empty : result.Text;
			CleanTranscript(ref transcript);
			return transcript;
		}

		public static void CleanTranscript(ref string transcript)
		{
			transcript = transcript.Replace("...", String.Empty);
			transcript = transcript.Replace("  ", " ");
		}
	}
}
