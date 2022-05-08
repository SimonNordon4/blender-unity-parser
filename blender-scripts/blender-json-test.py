# Clear console.
from operator import truediv
import os
os.system("CLS")

# Demonstration on how to use JSON for Blender.
import json
import bpy


try:
    #fore each object in the scene
    for ob in bpy.data.objects:
    
        mesh = ob.data
        mesh.calc_loop_triangles() # We need to convert mesh to triangles as it doesn't happen by default. remindme: maybe we can use quads at some point?

        meshData = {
            "name":mesh.name,
            "vertices":{},
            "normals":{},
            "uv":{},
            "triangles":{}
        }

        # get vertex and normal data
        for v in mesh.vertices:
            meshData["vertices"][v.index] = [v.co[0], v.co[1], v.co[2]] # Add Vertex List
            meshData["normals"][v.index] = [v.normal[0], v.normal[1], v.normal[2]] # Add Normal List

        # get triangle vertex indices.
        for tri in mesh.loop_triangles:
            meshData["triangles"][tri.index] = [tri.vertices[0],tri.vertices[1],tri.vertices[2]]

    #region JSON SERIALIZE

    # Save the json in the current scripts directory.
    scriptDir = r"E:\repos\blender-to-unity\blender-scripts"
    jsonDataDir = scriptDir + r"\demoData.json"

    # Write to File

    with open(jsonDataDir, "w") as f:
        json.dump(meshData, f,ensure_ascii=False,indent=4 )

    f.close()

    # Open the File & Load it's contents.

    jsonLoadFile = open(jsonDataDir, "r")
    jsonData = json.load(jsonLoadFile)

    print(jsonData)

    jsonLoadFile.close()
    #endregion

except Exception as e:
    print(e)