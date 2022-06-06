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

# meshes
startTime = time.time()
blend_meshes = b2u.ops.meshes.get_blend_meshes()
meshes_json = blend_meshes.tojson()
meshes_file = assetPath + assetName + "_meshes.json"

with open(meshes_file, 'w') as f:
    f.write(meshes_json)
endTime = time.time()
print("Export Completed in " + str(endTime - startTime) + " seconds")
