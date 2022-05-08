import json
import bpy

# MESHDATA
    # Vertex Array - world position of every vertex
    # Normal Array - normal vector of every vertex (mapped by index)
    # Triangle Array - List Vertices in Typles where every tuple is a triangle.
    # UV Array - Vertex to UV mapping.
# MeshData Api https://docs.unity3d.com/ScriptReference/Mesh.html
    # vertices, triangles, tangents, uv .. uv8, normals, color, colors32, bounds etc.

x = {'test':'1',
   'test2':'2'
}

try:

    for ob in bpy.data.objects:
        objectData = {
            "name":ob.data.name,
            "vertices":{},
            "normals":{}
        }
        for v in ob.data.vertices:
            #Add vertices to string
            objectData["vertices"][v.index] = str(v.co)
            objectData["normals"][v.index] = str(v.normal)

 
    y = json.dumps(x)
    print(y)
    with open(r"E:\repos\blender-to-unity\blender-to-unity\Assets\json\data.json", 'w', encoding='utf-8') as f:
        json.dump(y, f, ensure_ascii=False, indent=4)

    f1 = open(r"E:\repos\blender-to-unity\blender-to-unity\Assets\json\data.json", "w")
    data = json.load(f1)
    print(data)

    
except Exception as e:
    print(e)


