#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Kaitai;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;


//MESH: SdnaStruct[71] OBJECT: SdnaStruct[172]
public class BlendBinaryTest : MonoBehaviour
{
    private string m_blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\cube1.blend";
    [Button]
    public void GetBlend()
    {
        var blend = BlenderBlend.FromFile(m_blendFilePath);


        // Find all mesh Blocks.
        for (int i = 0; i < blend.Blocks.Count; i++)
        {
            if(blend.Blocks[i].SdnaIndex == 71)
            {
                var meshBlock = blend.Blocks[i];
                print(meshBlock.Code);
                print(i);
                print(meshBlock.SdnaIndex);
                foreach(var field in meshBlock.SdnaStruct.Fields)
                {
                    print($"\tfield.Name {field.Name} fieled.Type {field.Type}");
                }
                print(meshBlock.Body.ToString());


                // https://wiki.blender.jp/Dev:Source/Architecture/File_Format
                
                print(meshBlock.M_RawBody);
            }
        }

    }
}
#endif