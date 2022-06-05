using System.Diagnostics;
using System.IO;
using UnityEngine;
namespace Blender.Importer
{
    /// <summary>
    /// Manage a Blender Process from within Unity.
    /// </summary>
    public static class BlenderProcessHandler
    {
        public static void RunBlender(string blenderExectuablePath, string pythonExectuablePath, string blendFilePath, string args)
        {
            if(!IsValidArguments(blenderExectuablePath, pythonExectuablePath, blendFilePath)) throw new System.ArgumentException("Invalid Arguments Supplied to Blender Process Handler.");
            
            // set initial process arguments.
            var start = new ProcessStartInfo();
            start.FileName = blenderExectuablePath;
            // This is the command line argument for everything that comes after ../blender.exe
            start.Arguments = $"--background {blendFilePath} --python {pythonExectuablePath} -- {args}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;

            // begin the blender process.
            Process process = new Process();
            process.StartInfo = start;
            process.EnableRaisingEvents = true;
            
            // We debug.log everytime a print line is registered in the blender process.
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    f.print(args.Data,BlendImporterGlobalSettings.instance.PythonConsoleTextColor,"py",BlendImporterGlobalSettings.instance.PythonConsoleLabelColor);
                }
            };

            process.ErrorDataReceived += (sender,args) =>
            {
                if (args.Data != null)
                {
                    f.printError(args.Data,"py");
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.CancelOutputRead();
            process.CancelErrorRead();

            return;
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