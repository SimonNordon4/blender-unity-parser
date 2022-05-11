# http://graphics.cs.cmu.edu/courses/15-466-f17/notes/models/export-layer.py

import os
import json
import bpy

# OBJECTIVE: SERIALIZE A MESH!

# region Unity Classes


class Vector2:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def toJson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)


class Vector3:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def toJson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)

# endregion

# region Asset Classes

# Represents a Unity Mesh.


class UMesh:
    def __init__(self):
        name = ""
        vertices = []
        normals = []
        triangles = []
        uv = []
        uv2 = []
        uv3 = []
        uv4 = []
        uv5 = []
        uv6 = []
        uv7 = []
        uv8 = []

    def toJson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)

# endregion

# region Blender Classes


projectExport = 'E:\\repos\\blender-to-unity\\json-test\\data_bpy.json'
unityExport = 'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\blender-to-unity\\data.ublend'

# Calculate mesh data ver Loops


class UMeshLoop2JSON:

    def get_uv(mesh):
        uv_maps = []
        for i in range(len(mesh.uv_layers)):
            uv_layer = []
            for j in range(len(mesh.uv_layers[i].data)):
                uv = Vector2(mesh.uv_layers[i].data[j].uv.x, mesh.uv_layers[i].data[j].uv.y)
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps

    def Get(obj):
        uMesh = UMesh()
        uMesh.name = obj.name + "_loops"
        uMesh.vertices = []
        uMesh.normals = []
        uMesh.triangles = []
        uMesh.uv = []

        mesh = obj.data

        # Triangulate.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)

        for loop in mesh.loops:
            norm = Vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = Vector3(v.x, v.z, v.y)
            uMesh.vertices.append(vert)
            uMesh.normals.append(norm)

        for tri in mesh.loop_triangles:
            uMesh.triangles.append(tri.loops[0])
            uMesh.triangles.append(tri.loops[2])
            uMesh.triangles.append(tri.loops[1])

        # Get UV Maps
        
        uv_maps = UMeshLoop2JSON.get_uv(mesh)
        uMesh.uv = uv_maps[0]
        if len(uv_maps) > 1:
            uMesh.uv2 = uv_maps[1]
        if len(uv_maps) > 2:
            uMesh.uv3 = uv_maps[2]
        if len(uv_maps) > 3:
            uMesh.uv4 = uv_maps[3]
        if len(uv_maps) > 4:
            uMesh.uv5 = uv_maps[4]
        if len(uv_maps) > 5:
            uMesh.uv6 = uv_maps[5]
        if len(uv_maps) > 6:
            uMesh.uv7 = uv_maps[6]
        if len(uv_maps) > 7:
            uMesh.uv8 = uv_maps[7]

        return uMesh.toJson()


class WriteJson:
    def Write(data, path):
        with open(path, "w") as f:
            f.write(data)
            f.close()


class ReadJson:
    def Read(path):
        data = open(projectExport, "r")
        return json.load(data)

# endregion

# Actual Code


os.system('cls')
print("starting export")
data = UMeshLoop2JSON.Get(bpy.data.objects[0])
print("json finished")
WriteJson.Write(data, unityExport)
result = ReadJson.Read(projectExport)

print(data)
