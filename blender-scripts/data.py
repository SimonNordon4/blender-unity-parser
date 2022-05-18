from dataclasses import dataclass
import json

@dataclass
class UBlendData:
    ''' Map Ublend File Data to Unity Format'''
    def __init__(self):
        self.u_gameobjects = [] # GameObject
        self.u_meshes = [] # Mesh
        self.u_materials = [] # Materials
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
class Quaternion:
    ''' Unity Quaternion https://docs.unity3d.com/ScriptReference/Quaternion.html '''
    def __init__(self, x, y, z, w):
        self.x = x
        self.y = y
        self.z = z
        self.w = w

    def tojson(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
    
    
@dataclass  
class UScene:
    ''' Unity Scene Data Container. Represents Root Object more than the scene'''
    def __init__(self):
        name = ""
@dataclass
class UGameObject:
    ''' JSON Gameobject Representation https://docs.unity3d.com/ScriptReference/GameObject.html'''
    def __init__(self):
        self.name = None
        self.transform = UTransform()

@dataclass
class UComponent:
    ''' Parental Monobehaviour Class https://docs.unity3d.com/ScriptReference/MonoBehaviour.html'''
    def __init__(self):
        self.u_gameobject = ""
    
@dataclass
class UTransform:
    '''JSON Transform Representation  https://docs.unity3d.com/ScriptReference/Transform.html '''
    def __init__(self):
        self.parent = None;
        self.position = Vector3(0,0,0)
        self.rotation = Vector3(0,0,0)
        self.lossy_scale = Vector3(0,0,0)

@dataclass
class UMesh:
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
