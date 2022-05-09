import os
import json

import bmesh

#region Utility Classes

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

class bMesh:
    def __init__(self):
        name = ""
        vertices = []
        normals = []
        triangles = []
    def toJson(self):
        return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

#endregion


# Main Program

path = 'E:\\repos\\blender-to-unity\\json-test\\'
cJson = "data_cs.json"
pJson = "data_py.json"

x = bMesh()
v = Vec3(1.0,1.0,1.0)
i = Vec3Int(1,1,1)
x.name = "hello"
x.vertices = [v,v,v]
x.normals = [v,v,v]
x.triangles = [i,i,i]
y = x.toJson()

# Write to File
with open(path + "data_py.json", "w") as f:
    #json.dump(j, f,ensure_ascii=False,indent=4) #We've already serialized our contents so do not need to double serialize.
    f.write(y)

f.close()

# Read to File

# Open the File & Load it's contents.

data = open(path + cJson, "r")
result = json.load(data)

print(result)

