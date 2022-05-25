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
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.name = ""

@dataclass
class Vector3:
    ''' Unity Vector3'''
    def __init__(self,x,y,z):
        self.x = x
        self.y = y
        self.z = z
        