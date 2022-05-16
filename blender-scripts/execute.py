from sys import path
import bpy
import ublend

path.append('E:\\repos\\blender-to-unity\\blender-scripts')

UNITY_PROJECT = 'E:\\repos\\blender-to-unity\\blender-to-unity\\'
UNITY_EXPORT = UNITY_PROJECT + 'Assets\\blender-to-unity\\data.ublend'

data = ublend.ops.BMeshToUMesh.convert(bpy.data.objects[0])
_json = data.tojson()

print(_json)

# with open(UNITY_EXPORT, "w") as f:
#     f.write(_json)
#     f.close()

# with open(_json, "w") as f:
#     _json = data.tojson()
#     f.write(_json)
#     f.close()

# data = open(_json, "r")