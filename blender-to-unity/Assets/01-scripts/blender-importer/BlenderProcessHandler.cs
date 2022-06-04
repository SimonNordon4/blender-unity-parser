using System.Diagnostics;
using System.IO;

namespace Blender.Importer
{
    public static class BlenderProcessHandler
    {
        public delegate void BlenderProcessEvent(string result);
        public static event BlenderProcessEvent OnBlenderProcessFinished;

        public static void RunBlender(string blenderExectuablePath, string pythonExectuablePath, string blendFilePath, string args)
        {
            if(!IsValidArguments(blenderExectuablePath, pythonExectuablePath, blendFilePath)) return;
            
            var start = new ProcessStartInfo();
            start.FileName = blenderExectuablePath;
            start.Arguments = $"--background {blendFilePath} --python {pythonExectuablePath} -- {args}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    process.WaitForExit();
                    OnBlenderProcessFinished?.Invoke(result);
                }
            }
        }

        public static bool IsValidArguments(string blenderExectuablePath, string pythonExectuablePath, string blendFilePath)
        {
            if (blenderExectuablePath == string.Empty || pythonExectuablePath == string.Empty || blendFilePath == string.Empty)
            {
                f.printError("Blender executable path, python executable path, blend file path cannot be empty.");
                return false;
            }
            if (!File.Exists(blenderExectuablePath))
            {
                f.printError("Blender executable not found at: " + blenderExectuablePath);
                return false;
            }
            if(!File.Exists (pythonExectuablePath))
            {
                f.printError("Python executable not found at: " + pythonExectuablePath);
                return false;
            }
            if (!File.Exists(blendFilePath))
            {
                f.printError("Blend file not found at: " + blendFilePath);
                return false;
            }
            return true;
        }
    }
}