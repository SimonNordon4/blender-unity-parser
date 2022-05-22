''' UBlend Data Classes'''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class UData:
    ''' Data Conainer for entire UBlend file.
        Abstract Class for storing assets, objects and settings - these are implictly inferred in c#'''
    def __init__(self):
        self.u_assets = UAssets()
        self.u_objects = UObjects()
        self.u_settings = USettings()
    def tojson(self):
        ''' Convert the entire file into Json'''
        return orjson.dumps(self).decode("utf-8")
      
@dataclass
class UAssets:
    ''' Class for stroing all Asset Types'''
    def __init__(self):
        self.u_meshes = []
        self.u_materials = []
        self.u_textures = []

#region Objects
  
@dataclass
class UObjects:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.u_gameobjects = {UGameObject.__name__ : []}

@dataclass
class UGameObject:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.type = type(self).__name__
        self.name = ""
        self.u_components = {UComponent.__name__ :[]}
    
@dataclass
class UComponent:
    ''' Unity MonoBehaviour Representation'''
    def __init__(self):
        self.type = type(self).__name__
        
@dataclass
class UTransform:
    ''' Unity MonoBehaviour Representation'''
    def __init__(self):
        self.type = type(self).__name__
        self.position = [0,0,0]
        self.rotation = [0,0,0]
        self.scale = [1,1,1]
        self.parent_name = None
        
#endregion

@dataclass
class USettings:
    ''' Ublend File Settings'''
    def __init__(self):
        return None
    