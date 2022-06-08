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
