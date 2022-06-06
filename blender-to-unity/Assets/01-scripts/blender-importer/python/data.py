''' Blender To Unity Data Classes '''
from dataclasses import dataclass


@dataclass
class BlendMeshes:
    '''Blender To Unity Data Class: BlendMeshes'''
    def __init__(self):
        self.meshes = list


@dataclass
class BlendMesh:
    ''' Blender To Unity Data Class: BlendMesh '''
    def __init__(self):
        self.name_id = str
        self.vertices = list
        self.normals = str
        self.uv = list
        self.uv1 = list
        self.uv2 = list
        self.uv3 = list
        self.uv4 = list
        self.uv5 = list
        self.uv6 = list
        self.uv7 = list
        self.uv8 = list
        self.sub_meshes = list

@dataclass
class BlendSubMesh:
    ''' Blender To Unity Data Class: BlendSubMesh '''
    def __init__(self):
        self.triangles = list
