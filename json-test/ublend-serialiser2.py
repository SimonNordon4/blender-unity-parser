# http://graphics.cs.cmu.edu/courses/15-466-f17/notes/models/export-layer.py
import bpy
import sys
sys.path.append('E:\\repos\\blender-to-unity\\json-test\\')

import ublend




projectExport = 'E:\\repos\\blender-to-unity\\json-test\\data_bpy.json'
unityExport = 'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\blender-to-unity\\data.ublend'

# data = ublend.ublenddata.mesh_to_unity_mesh.convert(bpy.data.objects[0])

# with open(projectExport, "w") as f:
#     _json = data.tojson()
#     f.write(_json)
#     f.close()

# data = open(projectExport, "r")

ublend.ublenddata.test.x()

unity_mesh = ublend.ublenddata.mesh_to_unity_mesh.convert(bpy.data.objects[0])
json_data = unity_mesh.tojson()
print(json_data)

with open(unityExport, "w") as f:
    f.write(json_data)
    f.close()

