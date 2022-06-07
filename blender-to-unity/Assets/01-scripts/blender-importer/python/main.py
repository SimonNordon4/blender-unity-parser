from pydoc import resolve
import sys
import importlib
import time
from pathlib import Path

print("Starting Python Export Script")

# Append the path of the execution script, whichs is where we expect to find our modules.
appendPath = str(Path(__file__).parent.resolve()) + "\\"
print("Appending Path: " + str(appendPath))
sys.path.append(appendPath)

# Now we can import our modules after appending the path the script belongs in.
import b2u

# Remove after development.
importlib.reload(b2u)
importlib.reload(b2u.ops.meshes)
importlib.reload(b2u.data)

# METHODS

@staticmethod
def resolve_arguments(argv):
    ''' Convert arguments to a dictonary '''
    if "--" not in argv:  # no arguments, add defaults.
        return {
            "path": "E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer",
            "name": "default_export",
        }
    argv = argv[argv.index("--") + 1:]
    settings = {}
    print(argv)
    for arg in argv:
        if "=" in arg:
            key, value = arg.split("=")
            settings[key] = value
        else:
            print("ERROR: find argument in non dictionary format: " + arg)
    return settings

test_arg = [
    'C:\\Program Files\\Blender Foundation\\Blender 3.1\\blender.exe',
    '--background',
    'Assets/01-scripts/blender-importer/cube1.blend',
    '--python',
    'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\python\\main.py',
    '--',
    'path=E:/repos/blender-to-unity/blender-to-unity/Assets/01-scripts/blender-importer/',
    'name=cube1'
    ]

import_settings = resolve_arguments(test_arg)
for key in import_settings:
    print("Setting: " + key + " = " + import_settings[key])

# MAIN
# argv = sys.argv
# if "--" in argv:
#     argv = argv[argv.index("--") + 1:]  # get all args after "--"
# else:
#     argv = [
#         'E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer',
#         'test_export',
#         "False",
#         "False"]

# print(argv[0])
# print(argv[1])


resolve_arguments(sys.argv)

'''
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
'''

