from dataclasses import dataclass
import json

@dataclass
class UBlendFileData:
    ''' Map Ublend File Data to Unity Format'''
    def __init__(self):
        self.objects = [] # GameObject
        self.u_meshes = [] # Mesh
        self.materials = [] # Materials
    def tojson(self):
        return json.dumps(self,default=lambda o: o.__dict__,sort_keys=True,indent=4)

@dataclass
class Vector2:
    ''' Unity Vector2 https://docs.unity3d.com/ScriptReference/Vector2.html'''
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
@dataclass
class Vector3:
    ''' Unity Vector3 https://docs.unity3d.com/ScriptReference/Vector3.html '''
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
@dataclass  
class UnityScene:
    ''' Unity Scene Data Container. Represents Root Object more than the scene'''
    def __init__(self):
        name = ""
@dataclass
class GameObject:
    ''' Unity Game Object Representation https://docs.unity3d.com/ScriptReference/GameObject.html'''
@dataclass
class MonoBehaviour:
    ''' Parental Monobehaviour Class https://docs.unity3d.com/ScriptReference/MonoBehaviour.html'''
@dataclass
class Transform:
    ''' Unity Transform Representation https://docs.unity3d.com/ScriptReference/Transform.html'''
@dataclass
class UnityMesh:
    ''' Unity Mesh Representation https://docs.unity3d.com/ScriptReference/Mesh.html'''
    def __init__(self):
        self.name = ""
        self.vertices = []
        self.normals = []
        # sub meshes.
        self.submesh_count = 1
        self.submesh_triangles = []
        self.uvs = []

    def tojson(self):
        '''Convert this data class to a JSON'''
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
