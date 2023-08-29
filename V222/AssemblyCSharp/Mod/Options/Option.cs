using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace AssemblyCSharp.Mod.Options
{
	internal class Option
	{
		public static int status;

		public static string data;

		public static string filename;

		public static void SaveOpt(string filename, string data)
		{
			if (Thread.CurrentThread.Name == Main.mainThreadName)
			{
				__saveOpt(filename, data);
			}
			else
			{
				_saveOpt(filename, data);
			}
		}
		public static void SaveOptInt(string filname, int n)
        {
			try
			{
				SaveOpt(filname, n.ToString());
			}
			catch (Exception)
			{
			}
		}
		public static int LoadOptInt(string filename)
        {
			return (LoadOpt(filename) != null) ? Int32.Parse(LoadOpt(filename)) : -1;
        }
		public static string LoadOpt(string filename)
		{
			if (Thread.CurrentThread.Name == Main.mainThreadName)
			{
				return __loadOpt(filename);
			}
			return _loadOpt(filename);
		}
		private static void __saveOpt(string filename, string data)
		{
			string path = GetDocumentsPath() + "/" + filename;

			if (!File.Exists(path))
			{
				File.Create(path).Close();
            }
            if (CheckAccExistOnFile(path))
			{
				string[] array = File.ReadAllLines(path);
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Trim().Split('|');
					if (array2 != null && array2[0] == Char.myCharz().cName && array2[1] == GameMidlet.IP)
					{
						array[i] = Char.myCharz().cName + "|" + GameMidlet.IP + "|" + data;
					}
				}
				File.WriteAllLines(path, array);
			}
			else
            {
				string text = Char.myCharz().cName + "|" + GameMidlet.IP + "|" + data;
				File.AppendAllText(path, text + Environment.NewLine);
			}
			Main.setBackupIcloud(path);
		}
		private static void _saveOpt(string filename, string data)
		{
			if (status != 0)
			{
				Debug.LogError("Cannot save RMS " + filename + " because current is saving " + Option.filename);
				return;
			}
			Option.filename = filename;
			Option.data = data;
			status = 2;
			int i;
			for (i = 0; i < 500; i++)
			{
				Thread.Sleep(5);
				if (status == 0)
				{
					break;
				}
			}
			if (i == 500)
			{
				Debug.LogError("TOO LONG TO SAVE RMS " + filename);
			}
		}
		private static string __loadOpt(string filename)
		{
			try
			{
				string path = GetDocumentsPath() + "/" + filename;
				string opt = GetOptOnFile(path);
				return opt;
			}
			catch (Exception)
			{
				return null;
			}
		}
		private static string _loadOpt(string filename)
		{
			if (status != 0)
			{
				Debug.LogError("Cannot load RMS " + filename + " because current is loading " + Option.filename);
				return null;
			}
			Option.filename = filename;
			Option.data = null;
			status = 3;
			int i;
			for (i = 0; i < 500; i++)
			{
				Thread.Sleep(5);
				if (status == 0)
				{
					break;
				}
			}
			if (i == 500)
			{
				Debug.LogError("TOO LONG TO LOAD RMS " + filename);
			}
			return data;
		}
		public static void update()
		{
			if (status == 2)
			{
				status = 1;
				__saveOpt(filename, data);
				status = 0;
			}
			else if (status == 3)
			{
				status = 1;
				data = __loadOpt(filename);
				status = 0;
			}
		}
		public static bool CheckAccExistOnFile(string path)
        {
			string[] array = File.ReadAllLines(path);
			for(int i = 0; i < array.Length; i++)
            {
				string[] array2 = array[i].Trim().Split('|');
				if(array2[0] == Char.myCharz().cName && array2[1] == GameMidlet.IP)
				{
					return true;
                }
            }
			return false;
        }
		public static string GetOptOnFile(string path)
        {
			string[] array = File.ReadAllLines(path);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Trim().Split('|');
				if (array2[0] == Char.myCharz().cName && array2[1] == GameMidlet.IP)
				{
					return array2[2];
				}
			}
			return null;
		}
		public static bool CheckFileExist(string filename)
		{
			string path = GetDocumentsPath() + "/" + filename;
			if (File.Exists(path))
			{
				return true;
			}
			return false;
		}
		public static void DeleteFileOpt(string filename)
		{
			string path = GetDocumentsPath() + "/" + filename;
			if (!File.Exists(path))
			{
				return;
			}
			File.Delete(path);
		}
		public static string GetDocumentsPath()
		{
			DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
			string path = Path.GetTempPath() + info.Name + "/Mod";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}
		public static void clearAllOpt()
		{
			return;
			FileInfo[] files = new DirectoryInfo(GetDocumentsPath() + "/").GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				fileInfo.Delete();
			}
			Directory.Delete(GetDocumentsPath());
		}
	}
}
