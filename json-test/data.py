import json

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