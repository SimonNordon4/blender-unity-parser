import os
import json
import bpy

## OBJECTIVE: SERIALIZE A MESH!

#region Classes

#region Utility Classes

path = 'E:\\repos\\blender-to-unity\\json-test\\'
bJson = "data_bpy.json"

class Vec3:
   def __init__(self,x,y,z):
       self.x = x;
       self.y = y;
       self.z = z;
   def toJson(self):
       return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

class Vec3Int:
    def __init__(self,x,y,z):
       self.x = x;
       self.y = y;
       self.z = z;
    def toJson(self):
        return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)
    
#endregion

#region Asset Classes

#endregion

class bMesh:
    def __init__(self):
        name = ""
        vertices = []
        normals = []
        triangles = []
    def toJson(self):
        return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

#endregion

for ob in bpy.data.objects:
    newMesh = bMesh()
    newMesh.name = ob.name
    newMesh.vertices = []
    newMesh.normals = []
    newMesh.triangles = []

    mesh = ob.data
    mesh.calc_loop_triangles() # convert poly to triangles.

    # get verts & normals
    for v in mesh.vertices:
        vert = Vec3(v.co.x,v.co.y,v.co.z)
        norm = Vec3(v.normal.x,v.normal.y,v.normal.z)
        newMesh.vertices.append(vert)
        newMesh.normals.append(norm)
   
    # get triangles
    for tri in mesh.loop_triangles:
        tri = Vec3Int(tri.vertices[0],tri.vertices[1],tri.vertices[2])
        newMesh.triangles.append(tri)

    print(newMesh)


_json = newMesh.toJson();



# Write JSON to File
with open(path + "data_bpy.json", "w") as f:
    #json.dump(j, f,ensure_ascii=False,indent=4) #We've already serialized our contents so do not need to double serialize.
    f.write(_json)

f.close()

# ReLoad JSON for Verification

data = open(path + bJson, "r")
result = json.load(data)

print(result)