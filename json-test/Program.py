import os
import json

path = 'E:\\repos\\blender-to-unity\\json-test\\'
cJson = "csharp_data.json"
pJson = "python_data.json"

# Write Json Data

j = {"name":"TestMesh","vertices":[{"x":1.0,"y":1.0,"z":1.0}],"normals":[{"x":1.0,"y":1.0,"z":1.0}],"triangles":[{"x":1,"y":0,"z":2}]}

# Write to File
with open(path + pJson, "w") as f:
    json.dump(j, f,ensure_ascii=False,indent=4)

f.close()

# Read to File

# Open the File & Load it's contents.

data = open(path + pJson, "r")
result = json.load(data)

print(result)

#region Utility Classes

class Vec3:
   def __init__(self,x,y,z):
       self.x = x;
       self.y = y;
       self.z = z;

class Vec3Int:
    def __init__(self,x,y,z):
       self.x = x;
       self.y = y;
       self.z = z;
    
#endregion

#region Asset Classes

#endregion