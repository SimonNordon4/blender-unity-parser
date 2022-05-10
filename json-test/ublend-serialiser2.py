# http://graphics.cs.cmu.edu/courses/15-466-f17/notes/models/export-layer.py

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

# Calculate Mesh Data via Vertices
class UMeshVert2JSON:
    def Get(obj):
        uMesh = UMesh()
        uMesh.name = obj.name + "_verts"
        uMesh.vertices = []
        uMesh.normals = []
        uMesh.triangles = []

        mesh = obj.data

        # Triangulate.
        mesh.calc_loop_triangles()
        # Split Normals are only accessible via loops (not verts)
        mesh.calc_normals_split()

        # get verts & normals. note: We swap the order of y & z to convert to Unity directions from a base level.
        for v in mesh.vertices:
            vert = Vector3(v.co.x,v.co.z,v.co.y)
            norm = Vector3(v.normal.x,v.normal.z,v.normal.y)
            uMesh.vertices.append(vert)
            uMesh.normals.append(norm)

        # get triangles as an array of ints.
        for tri in mesh.loop_triangles:
            uMesh.triangles.append(tri.vertices[0])
            uMesh.triangles.append(tri.vertices[2])
            uMesh.triangles.append(tri.vertices[1])
            print(tri)
        
        return uMesh.toJson();

# Calculate mesh data ver Loops
class UMeshLoop2JSON:
    def Get(obj):
        uMesh = UMesh()
        uMesh.name = obj.name + "_loops"
        uMesh.vertices = []
        uMesh.normals = []
        uMesh.triangles = []

        mesh = obj.data

        # Triangulate.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)

        for loop in mesh.loops:
            norm = Vector3(loop.normal.x,loop.normal.z,loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = Vector3(v.x,v.z,v.y)
            uMesh.vertices.append(vert)
            uMesh.normals.append(norm)
        
            

        for tri in mesh.loop_triangles:
            uMesh.triangles.append(tri.loops[0])
            uMesh.triangles.append(tri.loops[2])
            uMesh.triangles.append(tri.loops[1])
            for loop in tri.loops:
                print(mesh.loops[loop].normal) # THE ANSWER LIES HERE <---! Get the loop normal / vert via the triangle
            
        
        return uMesh.toJson();

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

#Actual Code

os.system('cls')

data = UMeshLoop2JSON.Get(bpy.data.objects[0])
WriteJson.Write(data,unityExport)
result = ReadJson.Read(projectExport)

#print(data)
