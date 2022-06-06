using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Blender.Importer
{
    [CreateAssetMenu(fileName = "BlendImporterGlobalSettings", menuName = "Blender/BlendImporterGlobalSettings")]
    public class BlendImporterGlobalSettings : ScriptableObject
    {
        private static BlendImporterGlobalSettings _instance;
        public static BlendImporterGlobalSettings instance {
            get
            {
                if (_instance == null)
                {
                    _instance = AssetDatabase.LoadAssetAtPath<BlendImporterGlobalSettings>("Assets/01-scripts/blender-importer/settings/BlendImporterGlobalSettings.asset");
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        [ReadOnly]
        public string BlenderExectuablePath = @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
        [ReadOnly]
        public string PythonMainFile = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\main.py";

        [Header("Console Logging")]
        public Color PythonConsoleLabelColor = new Color(0.22f, 1f, 0.0f);
        public Color PythonConsoleTextColor = new Color(0.44f, 1f, 0.22f);
        public Color ErrorConsoleLabelColor = new Color(1f, 0.4f, 0.4f);
        public Color ErrorConsoleTextColor = new Color(1f, 0.6f, 0.6f);
        public Color StopWatchColor = Color.white;
        public Color BlendDataColor = Color.white;
        public Color AssetCreationColor = Color.white;


        [Button]
        private void SetAsMainInstance()
        {
            instance = this;
        }
    }

}