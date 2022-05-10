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
class uMeshToJson():
    def Convert(obj):
        mesh = obj.data
        
        polys = mesh.polygons
       
         # Used for split normals export
        poly_index_pairs = [(poly, index) for index, poly in enumerate(polys)]
        

         # Calculate the split normals from the shading
        mesh.calc_normals_split()
        loops = mesh.loops
        for loop in loops:
            print(loop.normal)  

        for poly, poly_index in poly_index_pairs:
            #gather the face vertices
            face_vertices = []
            for v in poly.vertices:
                face_vertices.append(mesh.vertices[v])
            face_vertices_length = len(face_vertices)

            vertices = []
            for v in face_vertices:
                vertices.append(Vector3(v.co.x,v.co.y,v.co.z))

            normals = []
            for l_idx in poly.loop_indices:
                normals.append(Vector3(loops[l_idx].normal.x,loops[l_idx].normal.y,loops[l_idx].normal.z))

            uMesh = UMesh()
            uMesh.name = mesh.name  
            uMesh.vertices = vertices
            uMesh.normals = normals

            return uMesh.toJson()

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

data = uMeshToJson.Convert(bpy.data.objects[0])
#WriteJson.Write(data,unityExport)
#result = ReadJson.Read(projectExport)

#print(data)
