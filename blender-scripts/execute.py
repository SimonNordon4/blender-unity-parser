import importlib
import sys
from sysconfig import get_path
import time

from pkg_resources import get_distribution
sys.path.append('E:\\repos\\blender-to-unity\\blender-scripts')
import ublend
importlib.reload(ublend.ops) # Reloading allows us to modify modules without quitting blender.
importlib.reload(ublend.data)

UNITY_PROJECT = 'E:\\repos\\blender-to-unity\\blender-to-unity\\'
UNITY_EXPORT = UNITY_PROJECT + 'Assets\\blender-to-unity\\data.ublend'

start = time.time()
u_blend_file_data = ublend.ops.CreateUBlend().create_ublend()
time_create_blend = time.time()
u_blend_data = u_blend_file_data.tojson()
time_json = time.time()

print('Total Vertices: ' + str(len(u_blend_file_data.u_meshes[0].vertices)))
print('Time to create blend (ms): ' + str((time_create_blend - start)*1000))
print('Time to json (ms): ' + str((time_json - time_create_blend)*1000))

# with open(UNITY_EXPORT, "w") as f:
#     f.write(u_blend_data)
#     f.close()

# with open(_json, "w") as f:
#     _json = data.tojson()
#     f.write(_json)
#     f.close()

# data = open(_json, "r")
 