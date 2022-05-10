from asyncore import write
import os
import json
import bpy

## OBJECTIVE: SERIALIZE A MESH!

#region Unity Classes

class Vector3:
   def __init__(self,x,y,z):
       self.x = x;
       self.y = y;
       self.z = z;
   def toJson(self):
       return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

#endregion

#region Asset Classes

# Represents a Unity Mesh.
class UMesh:
    def __init__(self):
        name = ""
        vertices = []
        normals = []
        triangles = []
    def toJson(self):
        return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

#endregion

#region Blender Classes

projectExport = 'E:\\repos\\blender-to-unity\\json-test\\data_bpy.json'
unityExport = 'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\blender-to-unity\\data.ublend'

class UMeshToJson:
    def Get(obj):
        newMesh = UMesh()
        newMesh.name = obj.name
        newMesh.vertices = []
        newMesh.normals = []
        newMesh.triangles = []

        mesh = obj.data
        mesh.calc_loop_triangles() # convert poly to triangles.

        # get verts & normals
        for v in mesh.vertices:
            vert = Vector3(v.co.x,v.co.y,v.co.z)
            norm = Vector3(v.normal.x,v.normal.y,v.normal.z)
            newMesh.vertices.append(vert)
            newMesh.normals.append(norm)
    
        # get triangles as an array of ints.
        for tri in mesh.loop_triangles:
            #tri = Vec3Int(tri.vertices[0],tri.vertices[1],tri.vertices[2])
            newMesh.triangles.append(tri.vertices[0])
            newMesh.triangles.append(tri.vertices[1])
            newMesh.triangles.append(tri.vertices[2])
        
        return newMesh.toJson();

class WriteJson:
    def Write(data,path):
        with open(path, "w") as f:       
            f.write(data)
            f.close()
        
class ReadJson:
    def Read(path):
        data = open(projectExport, "r")
        return json.load(data)

#endregion

data = UMeshToJson.Get(bpy.data.objects[0])
WriteJson.Write(data,unityExport)
result = ReadJson.Read(projectExport)

print(result)
