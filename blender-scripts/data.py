import json

class Vector2:
    ''' Unity Vector2 https://docs.unity3d.com/ScriptReference/Vector2.html'''
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)

class Vector3:
    ''' Unity Vector3 https://docs.unity3d.com/ScriptReference/Vector3.html '''
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
    
class UnityScene:
    ''' Unity Scene Data Container. Represents Root Object more than the scene'''
    def __init__(self):
        name = ""
        
class GameObject:
    ''' Unity Game Object Representation https://docs.unity3d.com/ScriptReference/GameObject.html'''

class MonoBehaviour:
    ''' Parental Monobehaviour Class https://docs.unity3d.com/ScriptReference/MonoBehaviour.html'''
    
class Transform:
    ''' Unity Transform Representation https://docs.unity3d.com/ScriptReference/Transform.html'''

class UnityMesh:
    ''' Unity Mesh Representation https://docs.unity3d.com/ScriptReference/Mesh.html'''
    def __init__(self):
        self.name = ""
        self.bounds = [0,0]
        self.vertices = []
        self.triangles = []
        self.normals = []
        self.tangents = []    
        self.colors = []
        
        # sub meshes.
        self.submesh_count = 1;
        
        self.uv = []
        self.uv2 = []
        self.uv3 = []
        self.uv4 = []
        self.uv5 = []
        self.uv6 = []
        self.uv7 = []
        self.uv8 = []
        
    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)