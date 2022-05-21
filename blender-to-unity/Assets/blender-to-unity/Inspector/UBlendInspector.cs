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

    // [CustomPropertyDrawer(typeof(UComponent))]
    // public class UComponentDrawer: PropertyDrawer
    // {
    //     public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) 
    //     {
    //         return EditorGUI.GetPropertyHeight(property);
    //     }
    
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         Debug.Log(property.serializedObject.targetObject.GetType());
    //         EditorGUI.PropertyField(position, property, label, true);
    //     }
    // }
}