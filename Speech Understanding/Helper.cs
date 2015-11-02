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
	}
}
