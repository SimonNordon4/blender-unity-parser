''' UBlend Data Classes'''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class UData:
    ''' Data Conainer for entire UBlend file. '''
    def __init__ (self):
        self.u_meshes = []
        self.u_gameobjects = []

# @dataclass
# class UData:
#     ''' Data Conainer for entire UBlend file.
#         Abstract Class for storing assets, objects and settings - these are implictly inferred in c#'''
#     def __init__(self):
#         self.u_assets = UAssets()
#         self.u_objects = UObjects()
#         self.u_settings = USettings()
#     def tojson(self):
#         ''' Convert the entire file into Json'''
#         return orjson.dumps(self).decode("utf-8")
      
# @dataclass
# class UAssets:
#     ''' Class for stroing all Asset Types'''
#     def __init__(self):
#         self.u_meshes = []
#         self.u_materials = []
#         self.u_textures = []
        
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

#region Objects
  
@dataclass
class UObjects:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.u_gameobjects = []

@dataclass
class UGameObject:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.name = ""
        self.u_transform = UTransform()
        self.u_components = []
    
@dataclass
class UComponent:
    ''' Unity MonoBehaviour Representation'''
    def __init__(self):
        self.type = type(self).__name__
        
@dataclass
class UTransform:
    ''' Unity MonoBehaviour Representation'''
    def __init__(self):
        self.position = [0,0,0]
        self.rotation = [0,0,0]
        self.scale = [1,1,1]
        self.parent_name = None
        

@dataclass
class UMeshFilter:
    ''' Unity MeshFilter Representation'''
    def __init__(self):
        self.type = type(self).__name__
        self.mesh_name = ""
        
#endregion

@dataclass
class USettings:
    ''' Ublend File Settings'''
    def __init__(self):
        return None
    