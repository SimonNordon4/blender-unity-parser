''' Blender To Unity Data Classes '''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class BlendData:
    ''' Blend Data Class '''
    def __init__(self):
        self.blend_meshes = []
        self.blend_gameoobjects = []

    def tojson(self):
        return orjson.dumps(self).decode("utf-8")

@dataclass
class BlendMesh:
    ''' Blender To Unity Data Class: BlendMesh '''
    def __init__(self):
        self.name_id = ""
        self.vertices = []
        self.normals = ""
        self.uv = []
        self.uv1 = []
        self.uv2 = []
        self.uv3 = []
        self.uv4 = []
        self.uv5 = []
        self.uv6 = []
        self.uv7 = []
        self.uv8 = []
        self.sub_meshes = []

@dataclass
class BlendSubMesh:
    ''' Blender To Unity Data Class: BlendSubMesh '''
    def __init__(self):
        self.triangles = []


@dataclass
class BlendGameObject:
    ''' Unity GameObject Representation
    https://docs.unity3d.com/ScriptReference/GameObject.html
    We also append the transform properties (every gameobject has a transfrom)
    https://docs.unity3d.com/ScriptReference/Transform.html
    We also append the MeshFilter and MeshRenderer props (we only look at Mesh Objects)
    https://docs.unity3d.com/ScriptReference/MeshFilter.html '''
    def __init__(self):
        self.name_id = ""

        # Transform props
        self.parent_name = ""
        self.position = Vector3()
        self.rotation = Vector3()
        self.scale = Vector3()

        # Mesh props
        self.mesh_id = ""
        self.material_slots = 0

@dataclass
class Color:
    ''' Unity Color '''
    def __init__(self, r=1, g=1, b=1, a=1):
        self.r = r
        self.g = g
        self.b = b
        self.a = a

@dataclass
class Vector3:
    ''' Unity Vector3'''
    def __init__(self, x=0, y=0, z=0):
        self.x = x
        self.y = y
        self.z = z
        
@dataclass
class Vector2:
    ''' Unity Vector '''
    def __init__(self, x=0, y=0):
        self.x = x
        self.y = y