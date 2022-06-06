import sys
import importlib
import time
from pathlib import Path

print("Starting Python Export Script")

# Append the path of the execution script, whichs is where we expect to find our modules.
appendPath = str(Path(__file__).parent.resolve()) + "\\"
print("Appending Path: " + str(appendPath))
sys.path.append(appendPath)

# Now we can import our modules.
import b2u

# Remove after development.
importlib.reload(b2u)
importlib.reload(b2u.ops.meshes)
importlib.reload(b2u.data)

argv = sys.argv
if "--" in argv:
    argv = argv[argv.index("--") + 1:]  # get all args after "--"
else:
    argv = [
        'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer',
        'test_export',
        "False",
        "False"]

assetPath = argv[0]
assetName = argv[1]
exportFile = assetPath + assetName + ".json"

print("Export Blend Data to: " + exportFile)

blend_meshes = b2u.ops.meshes.get_blend_meshes()
print(blend_meshes.tojson())

startTime = time.time()
# ublenddata = ublend.ops.get_u_data()
# json = ublenddata.tojson()

# with open(exportFile, 'w') as f:
#     f.write(json)
endTime = time.time()
print("Export Completed in " + str(endTime - startTime) + " seconds")
