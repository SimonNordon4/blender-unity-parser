import json
import bpy

class vector2:
    ''' Unity Vector2 https://docs.unity3d.com/ScriptReference/Vector2.html'''
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)


class vector3:
    ''' Unity Vector3 https://docs.unity3d.com/ScriptReference/Vector3.html '''
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
    
class unity_scene:
    ''' Unity Scene Data Container. Represents Root Object more than the scene'''
    def __init__(self):
        name = ""
        
class unity_gameobject:
    ''' Unity Game Object Representation https://docs.unity3d.com/ScriptReference/GameObject.html'''

class unity_monobehaviour:
    ''' Parental Monobehaviour Class https://docs.unity3d.com/ScriptReference/MonoBehaviour.html'''
    
class unity_transform:
    ''' Unity Transform Representation https://docs.unity3d.com/ScriptReference/Transform.html'''

class unity_mesh:
    ''' Unity Mesh Representation https://docs.unity3d.com/ScriptReference/Mesh.html'''
    def __init__(self):
        name = ""
        bounds = [0,0];
        
        vertices = []
        triangles = []
        
        normals = []
        tangents =[]        
        colors = []
        
        uv = []
        uv2 = []
        uv3 = []
        uv4 = []
        uv5 = []
        uv6 = []
        uv7 = []
        uv8 = []

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
    
class mesh_to_unity_mesh:
    # Serialize a Blender Mesh into a Unity Friendly JSON
    def get_uv(mesh):
        uv_maps = []
        for i in range(len(mesh.uv_layers)):
            uv_layer = []
            for j in range(len(mesh.uv_layers[i].data)):
                uv = vector2(mesh.uv_layers[i].data[j].uv.x, mesh.uv_layers[i].data[j].uv.y)
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps

    def convert(obj):
        u_mesh = unity_mesh()
        u_mesh.name = obj.name + "_loops"
        u_mesh.vertices = []
        u_mesh.normals = []
        u_mesh.triangles = []
        u_mesh.uv = []

        # TODO: apply modifiers and create virtual copy of the mesh.
        # prep the mesh
        mesh = obj.data
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)

        for loop in mesh.loops:
            norm = vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = vector3(v.x, v.z, v.y)
            u_mesh.vertices.append(vert)
            u_mesh.normals.append(norm)

        for tri in mesh.loop_triangles:
            u_mesh.triangles.append(tri.loops[0])
            u_mesh.triangles.append(tri.loops[2])
            u_mesh.triangles.append(tri.loops[1])

        # Get UV Maps
        
        uv_maps = mesh_to_unity_mesh.get_uv(mesh)
        if len(uv_maps) > 0:
            u_mesh.uv = uv_maps[0]
        if len(uv_maps) > 1:
            u_mesh.uv2 = uv_maps[1]
        if len(uv_maps) > 2:
            u_mesh.uv3 = uv_maps[2]
        if len(uv_maps) > 3:
            u_mesh.uv4 = uv_maps[3]
        if len(uv_maps) > 4:
            u_mesh.uv5 = uv_maps[4]
        if len(uv_maps) > 5:
            u_mesh.uv6 = uv_maps[5]
        if len(uv_maps) > 6:
            u_mesh.uv7 = uv_maps[6]
        if len(uv_maps) > 7:
            u_mesh.uv8 = uv_maps[7]

        return u_mesh
    
class test:
    def x():
        print("test")