using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Blender.Importer
{
    [CreateAssetMenu(fileName = "BlendImporterGlobalSettings", menuName = "Blender/BlendImporterGlobalSettings")]
    public class BlendImporterGlobalSettings : ScriptableSingleton<BlendImporterGlobalSettings>
    {
        public string BlenderExectuablePath = string.Empty;
        public string PythonScriptDirectory = string.Empty;

        [Header("Console Logging")]
        public Color PythonConsoleLabelColor = Color.white;
        public Color PythonConsoleTextColor = Color.white;
        public Color StopWatchColor = Color.white;
        public Color BlendDataColor = Color.white;
        public Color AssetCreationColor = Color.white;
    }

}