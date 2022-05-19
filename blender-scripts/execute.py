import importlib
import sys
sys.path.append('E:\\repos\\blender-to-unity\\blender-scripts')
import ublend
importlib.reload(ublend.ops) # Reloading allows us to modify modules without quitting blender.
importlib.reload(ublend.data)



UNITY_PROJECT = 'E:\\repos\\blender-to-unity\\blender-to-unity\\'
UNITY_EXPORT = UNITY_PROJECT + 'Assets\\blender-to-unity\\data.ublend'

u_blend_file_data = ublend.ops.CreateUBlend().create_ublend()
u_blend_data = u_blend_file_data.tojson()

print(u_blend_data)

with open(UNITY_EXPORT, "w") as f:
    f.write(u_blend_data)
    f.close()

# with open(_json, "w") as f:
#     _json = data.tojson()
#     f.write(_json)
#     f.close()

# data = open(_json, "r")