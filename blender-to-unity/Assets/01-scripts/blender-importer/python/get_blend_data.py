from os.path import exists

EXPORT_FILE = "E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\blend_data.json"

print("Retrieving Blender data")

savedData :str = "My Save Data"

if exists(EXPORT_FILE):
    with open(EXPORT_FILE, "w") as f:
        f.write(savedData)
else:
    with open(EXPORT_FILE, "w") as f:
        f.write(savedData)

