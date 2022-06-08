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
from b2u import settings

# Remove after development.
importlib.reload(b2u)
importlib.reload(b2u.ops.meshes)
importlib.reload(b2u.data)
importlib.reload(b2u.settings)

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

@staticmethod
def apply_settings(import_settings):
    ''' Apply settings to the script '''
    settings.set_blend_path(import_settings["blend_path"])
    settings.set_blend_name(import_settings["blend_name"])
    settings.set_vec_precision(import_settings["vec_precision"])


import_settings = resolve_arguments(sys.argv)  # Get a dictionary from the cmd args
apply_settings(import_settings)  # Apply those args as global settings.

# meshes
startTime = time.time()
blend_data = b2u.ops.get_blend_data()  # Get the blend data
meshes_json = blend_data.tojson()
meshes_file = settings.blend_path + settings.blend_name + ".json"

with open(meshes_file, 'w') as f:
    f.write(meshes_json)
endTime = time.time()
print("Export Completed in " + str(endTime - startTime) + " seconds")
