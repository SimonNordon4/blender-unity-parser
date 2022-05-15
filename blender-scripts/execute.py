import bpy
from sys import path
path.append('E:\\repos\\blender-to-unity\\blender-scripts')
import ublend

unityExport = 'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\blender-to-unity\\data.ublend'

data = ublend.ops.mesh_to_unity_submesh.convert(bpy.data.objects[0])
_json = data.tojson()

print(_json)

with open(unityExport, "w") as f:
    f.write(_json)
    f.close()

# with open(_json, "w") as f:
#     _json = data.tojson()
#     f.write(_json)
#     f.close()

# data = open(_json, "r")