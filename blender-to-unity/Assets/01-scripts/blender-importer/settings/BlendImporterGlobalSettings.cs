using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Blender.Importer
{
    [CreateAssetMenu(fileName = "BlendImporterGlobalSettings", menuName = "Blender/BlendImporterGlobalSettings")]
    public class BlendImporterGlobalSettings : ScriptableSingleton<BlendImporterGlobalSettings>
    {
        public string BlenderExectuablePath = @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
        public string PythonMainFile = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\main.py";

        [Header("Console Logging")]
        public Color PythonConsoleLabelColor = Color.white;
        public Color PythonConsoleTextColor = Color.white;
        public Color ErrorConsoleLabelColor = Color.red;
        public Color ErrorConsoleTextColor = Color.red;
        public Color StopWatchColor = Color.white;
        public Color BlendDataColor = Color.white;
        public Color AssetCreationColor = Color.white;
    }

}