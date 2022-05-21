from dataclasses import dataclass
import sys
sys.path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class UBlendData:
    ''' Map Ublend File Data to Unity Format'''
    def __init__(self):
        self.u_gameobjects = [] # GameObjects
        self.u_meshes = [] # Meshs
        self.u_materials = [] # Materials
    def tojson(self):
        ''' Convert entire UBlendData to json'''
        return orjson.dumps(self).decode("utf-8")

@dataclass
class UScene:
    ''' Unity Scene Data Container. Represents Root Object more than the scene'''
    def __init__(self):
        self.u_name = None
        
@dataclass
class UGameObject:
    ''' JSON Gameobject Representation https://docs.unity3d.com/ScriptReference/GameObject.html'''
    def __init__(self):
        self.u_name = None
        self.u_components = []

@dataclass
class UComponent:
    ''' Parental Monobehaviour Class https://docs.unity3d.com/ScriptReference/MonoBehaviour.html'''
    def __init__(self):
        self.u_type = type(self).__name__
    
@dataclass
class UTransform(UComponent):
    '''JSON Transform Representation  https://docs.unity3d.com/ScriptReference/Transform.html '''
    def __init__(self):
        self.u_type = type(self).__name__
        self.u_parent = None; #Reference a UGameObject by name
        self.u_position = Vector3(0,0,0)
        self.u_rotation = Vector3(0,0,0)
        self.u_scale = Vector3(0,0,0)
@dataclass
class UMeshFilter(UComponent):
    def __init__(self,mesh_name):
        self.u_type = type(self).__name__
        self.u_mesh_ref = mesh_name; #Reference a UMesh by name
        
@dataclass
class UMeshRenderer(UComponent):
    def __init__(self):
        self.u_type = type(self).__name__
        self.u_materials = []

@dataclass
class UMesh:
    ''' Unity Mesh Representation https://docs.unity3d.com/ScriptReference/Mesh.html'''
    def __init__(self):
        self.u_name = ""
        self.u_vertices = []
        self.u_normals = []
        # sub meshes.
        self.u_submesh_count = 1
        self.u_submesh_triangles = []
        self.u_uvs = []

@dataclass
class Vector2:
    ''' Unity Vector2 https://docs.unity3d.com/ScriptReference/Vector2.html'''
    def __init__(self, u_x, u_y):
        self.u_x = u_x
        self.u_y = u_y

@dataclass
class Vector3:
    ''' Unity Vector3 https://docs.unity3d.com/ScriptReference/Vector3.html '''
    def __init__(self, u_x, u_y, u_z):
        self.u_x = u_x
        self.u_y = u_y
        self.u_z = u_z