#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Kaitai;


//MESH: SdnaStruct[71] OBJECT: SdnaStruct[172]
public class BlendBinaryTest : MonoBehaviour
{
    private string m_blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\cube1.blend";
    [Button]
    public void GetBlend()
    {
        var blend = BlenderBlend.FromFile(m_blendFilePath);

        for (int i = 0; i < blend.SdnaStructs.Count; i++)
        {
            var block = blend.SdnaStructs[i];
            print(block.Type);
            foreach (var field in block.Fields)
            {
                print("\t"+field.Name);
                print("\t"+field.Type);
            }
        }

        
    }
}
#endif