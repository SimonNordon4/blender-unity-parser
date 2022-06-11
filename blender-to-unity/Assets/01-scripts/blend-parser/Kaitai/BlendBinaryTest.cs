#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Kaitai;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Text;


//MESH: SdnaStruct[71] OBJECT: SdnaStruct[172]
public class BlendBinaryTest : MonoBehaviour
{
    private string m_blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\cube1 - Copy.blend";
    [Button]
    public void GetBlendKaiti()
    {
        var blend = BlenderBlend.FromFile(m_blendFilePath);

        var blocks = blend.Blocks;
        var sdnas = blend.SdnaStructs;

        // Block 1724 is a mesh.
        var mesh_cube = blocks[1724];
        var mesh_sdna = sdnas[(int)mesh_cube.SdnaIndex];

        foreach (var field in mesh_sdna.Fields)
        {
            Debug.Log(field.Name);
            Debug.Log(field.Type);
        }

        var mesh_bytes = (byte[])mesh_cube.Body;
        
        //https://wiki.blender.jp/Dev:Source/Architecture/File_Format
        using (BinaryReader reader = new BinaryReader(new MemoryStream(mesh_bytes)))
        {
            print(reader.BaseStream.Position);
            var id = reader.ReadBytes((int)mesh_cube.LenBody);
            print(Encoding.ASCII.GetString(id));
            print(reader.BaseStream.Position);
        }
        print("");
    }
}
#endif