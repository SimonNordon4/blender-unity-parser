using Newtonsoft.Json;
using System.IO;

namespace JsonTest
{
    class Program
    {

        private static string path = @"E:\repos\blender-to-unity\json-test\";
        private static string cJson = "csharp_data.json";
        private static string pJson = "python_data.json";

        static void Main(string[] args)
        {
            // Create Dummy Data.
            var v = new Vec3(1.0f, 1.0f, 1.0f);
            var i = new Vec3Int(1, 0, 2);
            var m = new bMesh();

            m.name = "TestMesh";
            m.vertices.Add(v);
            m.normals.Add(v);
            m.triangles.Add(i);

            // Write to Json
            var json = JsonConvert.SerializeObject(m);
            File.WriteAllText(path + cJson, json);

            // Load and read Json.
            StreamReader sr = new StreamReader(path + pJson);
            var data = sr.ReadToEnd();
            if(data is null) return;
            var result = JsonConvert.DeserializeObject<bMesh>(data);

            print(data);
            print(result.vertices[0].x);
            print(result.name);

        }

        public static void print(object obj)
        {
            Console.WriteLine(obj.ToString());
        }

        public void Test()
        {
            Console.WriteLine("Hello");
        }
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
    }

    #endregion

    #region Asset Classes

    public class bMesh
    {
        public string name = "";
        public List<Vec3> vertices = new List<Vec3>();
        public List<Vec3> normals = new List<Vec3>();
        public List<Vec3Int> triangles = new List<Vec3Int>();
    }

    #endregion
}