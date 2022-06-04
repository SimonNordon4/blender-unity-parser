JSON_PATH = "E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\TEST_json.json"
JSON_EXPORT = "E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\TEST_json_export.json"

with open(JSON_PATH, "r") as f:
    json = f.read()
    
print(json)
