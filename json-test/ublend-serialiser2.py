# http://graphics.cs.cmu.edu/courses/15-466-f17/notes/models/export-layer.py

import os
import json
import bpy
import ublend

# projectExport = 'E:\\repos\\blender-to-unity\\json-test\\data_bpy.json'
# unityExport = 'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\blender-to-unity\\data.ublend'


# class ublenda:
#     # manage ublend read and write (a.k.a JSON)
#     def write(data, path):
#         with open(path, "w") as f:
#             _json = data.tojson()
#             f.write(_json)
#             f.close()
#     def read(path):
#         data = open(projectExport, "r")
#         return json.load(data)

# os.system('cls')

# data = ublend.ublenddata.mesh_to_unity_mesh.convert(bpy.data.objects[0])
# ublenda.write(data, unityExport)
# result = ublenda.read(projectExport)
# print(data)

ublend.ublenddata.test