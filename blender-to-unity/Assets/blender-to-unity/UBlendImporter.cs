using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[ScriptedImporter(1, "ublend")]
public class UBlendImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Debug.Log($"Importing {ctx.assetPath}");
        var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);

        var json = (File.ReadAllText(ctx.assetPath));
    }

    #region Utility Classes

    public class Vec3
    {
        public Vec3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public float x;
        public float y;
        public float z;

        public override string ToString()
        {
            return $"({x:0.0000},{y:0.0000},{z:0.0000})";
        }
    }

    public class Vec3Int
    {
        public Vec3Int(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public int x;
        public int y;
        public int z;

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }

    #endregion

    #region Asset Classes

    public class UMesh
    {
        public string name = "";
        public List<Vec3> vertices = new List<Vec3>();
        public List<Vec3> normals = new List<Vec3>();
        public List<Vec3Int> triangles = new List<Vec3Int>();
    }

    #endregion
}


