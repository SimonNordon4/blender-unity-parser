import importlib
import sys
sys.path.append('E:\\repos\\blender-to-unity\\02-bpy')
import time
import ublend
from pathlib import Path

importlib.reload(ublend)
importlib.reload(ublend.ops)
importlib.reload(ublend.data)
importlib.reload(ublend.settings)

print("Python Recieved")

argv = sys.argv
if "--" in argv:
    argv = argv[argv.index("--") + 1:]  # get all args after "--"
else:
    argv = ['E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer', 'test_export',"False","False"]

print(argv)
assetPath = argv[0]
assetName = argv[1]

exportFile = assetPath + "\\" + assetName + ".json"

print(exportFile)

startTime = time.time()
ublenddata = ublend.ops.get_u_data()
json = ublenddata.tojson()

with open(exportFile, 'w') as f:
    f.write(json)
endTime = time.time()
print("Export Completed in " + str(endTime - startTime) + " seconds")