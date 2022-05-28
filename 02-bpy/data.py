''' UBlend Data Classes'''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson
from enum import Enum

class ShaderType(Enum):
    ''' Enum for Shader Types '''
    LIT = 0
    UNLIT = 1
    
class RenderType(Enum):
    ''' Enum for Render Types '''
    OPAQUE = 0
    TRANSPARENT = 1

@dataclass
class UData:
    ''' Data Conainer for entire UBlend file. '''
    def __init__ (self):
        self.u_meshes = []
        self.u_materials = []
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
        
@dataclass
class UMaterial:
    ''' Data on Materials'''
    def __init__(self):
        self.name = ""
        self.shader = ""
        self.rendertype = ""
        self.base_color = Color()
        self.roughness = 1.0
        self.metallic = 0.0
        self.emission_color = Color()

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
        rotation = Vector3()
        scale = Vector3()
        
        #Mesh props
        mesh_name = ""
        material_names = []
@dataclass
class Color:
    ''' Unity Color '''
    def __init__(self,r=1,g=1,b=1,a=1):
        self.r = r
        self.g = g
        self.b = b
        self.a = a

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