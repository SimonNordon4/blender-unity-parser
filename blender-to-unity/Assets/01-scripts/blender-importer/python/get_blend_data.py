import bpy
print("VertStart")
for mesh in bpy.data.meshes:
    for v in mesh.vertices:
        print(v.co.x, v.co.y, v.co.z)
print("VertEnd")    
