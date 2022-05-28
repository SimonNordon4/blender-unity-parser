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
    def tojson(self):
        return orjson.dumps(self).decode("utf-8")

        
@dataclass
class UMesh:
    ''' Unity Mesh Representation https://docs.unity3d.com/ScriptReference/Mesh.html'''
    def __init__(self):
        self.name = ""
        self.vertices = []
        self.normals = []
        self.uv = []
        self.uv2 = []
        self.uv3 = []
        self.uv4 = []
        self.uv5 = []
        self.uv6 = []
        self.uv7 = []
        self.submeshes = []
        
@dataclass
class USubMesh:
    ''' Data on submeshes '''
    def __init__(self):
        self.triangles = []

#region Objects

@dataclass
class UGameObject:
    ''' Unity GameObject Representation
    https://docs.unity3d.com/ScriptReference/GameObject.html
    We also append the transform properties (every gameobject has a transfrom)
    https://docs.unity3d.com/ScriptReference/Transform.html
    We also append the MeshFilter and MeshRenderer props (we only look at Mesh Objects)
    https://docs.unity3d.com/ScriptReference/MeshFilter.html '''
    def __init__(self):
        self.name = ""
        
        #Transform props
        parent_name = ""
        position = Vector3()
        rotation = Quarternion()
        scale = Vector3()
        
        #Mesh props
        mesh_name = ""

@dataclass
class Vector3:
    ''' Unity Vector3'''
    def __init__(self,x=0,y=0,z=0):
        self.x = x
        self.y = y
        self.z = z
        
@dataclass
class Vector2:
    ''' Unity Vector '''
    def __init__(self,x=0,y=0):
        self.x = x
        self.y = y