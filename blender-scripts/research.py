import bpy

for objects in bpy.data.objects:
    print(objects.name)
    print(objects.users)

for mesh in bpy.data.meshes:
    print(mesh.users) # shows how many objects are using this mesh.
    
for material in bpy.data.materials:
    print(material.name)
    print(material.users)
    
for collection in bpy.data.collections:
    print(collection.name)
    print(collection.users)