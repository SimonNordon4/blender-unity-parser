using Newtonsoft.Json;
using System.IO;

namespace JsonTest
{
    class Program
    {

        private static string path = @"E:\repos\blender-to-unity\json-test\";
        private static string cJson = "data_cs.json";
        private static string pJson = "data_py.json";
        private static string bJson = "data_bpy.json";

        static void Main(string[] args)
        {
            // Create Dummy Data.
            var v = new Vec3(1.0f, 1.0f, 1.0f);
            var tri = new Vec3Int(1, 1, 1);
            var m = new bMesh();

            m.name = "TestMesh";
            m.vertices.Add(v);
            m.vertices.Add(v);
            m.vertices.Add(v);
            m.normals.Add(v);
            m.normals.Add(v);
            m.normals.Add(v);
            m.triangles.Add(tri);
            m.triangles.Add(tri);
            m.triangles.Add(tri);

            // Write to Json
            var json = JsonConvert.SerializeObject(m);
            File.WriteAllText(path + "data_cs.json", json);

            // Load and read Json.
            StreamReader sr = new StreamReader(path + bJson);
            var data = sr.ReadToEnd();
            if(data is null) return;
            var result = JsonConvert.DeserializeObject<bMesh>(data);

            // Verify Results
            print(data);
            print($"Mesh Name: {result.name}"); // Name
            for( int i = 0; i < result.vertices.Count; i++) print($"Vertex[{i}]: {result.vertices[i]}"); // Vertices
            for( int i = 0; i < result.normals.Count; i++) print($"Normal[{i}]: {result.normals[i]}"); // Normals
            for( int i = 0; i < result.triangles.Count; i++) print($"Triangle[{i}]: {result.triangles[i]}"); // Triangles
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

    public class bMesh
    {
        public string name = "";
        public List<Vec3> vertices = new List<Vec3>();
        public List<Vec3> normals = new List<Vec3>();
        public List<Vec3Int> triangles = new List<Vec3Int>();
    }

    #endregion
}