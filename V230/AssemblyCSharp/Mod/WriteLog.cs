using System;
using System.IO;
internal class WriteLog
{
    private static readonly string _logFolder = "Data\\err";
    public static void write(string className, string path, string log)
    {
        try
        {
            string folder = $"{_logFolder}\\{className}";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string dir = $"{folder}\\{path}";
            if (!File.Exists(dir)) File.Create(dir).Close();

            File.AppendAllText(dir, $"----------{DateTime.Now.ToString("HH:mm dd/MM/yyyy")}----------\n\n{log}\n\n\n");
        }
        catch { }
    }

    public static void clearLog(string className, string path)
    {
        try
        {
            string dir = $"{_logFolder}\\{className}\\{path}";
            using FileStream fileStream = new(dir, FileMode.Create, FileAccess.Write);
        }
        catch { }
    }
}
