using System;
using UnityEngine;
using UnityEditor.AssetImporters;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace UnityToBlender
{
    [CustomEditor(typeof(UBlendImporter))]
    public class UBlendInspector: ScriptedImporterEditor
    {
        SerializedProperty uBlend;
     
        
        public override void OnEnable() {
            uBlend = serializedObject.FindProperty("uBlend");
            base.OnEnable();
        }

        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            //EditorGUILayout.LabelField("UBlend Inspector");
            serializedObject.Update();
            // EditorGUILayout.PropertyField(uBlend);
            // serializedObject.ApplyModifiedProperties();
        }
    }
}