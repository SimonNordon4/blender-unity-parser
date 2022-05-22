''' UBlend Data Classes'''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class Data:
    ''' Data Conainer for entire UBlend file.
        Abstract Class for storing assets, objects and settings - these are implictly inferred in c#'''
    def __init__(self):
        self.assets = Assets()
        self.objects = Objects()
        self.settings = Settings()
    def tojson(self):
        ''' Convert the entire file into Json'''
        return orjson.dumps(self).decode("utf-8")
      
@dataclass
class Assets:
    ''' Class for stroing all Asset Types'''
    def __init__(self):
        self.meshes = []
        self.materials = []
        self.textures = []

#region Objects
  
@dataclass
class Objects:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.gameobjects = {GameObject.__name__ : []}

@dataclass
class GameObject:
    ''' Unity GameObject Representation'''
    def __init__(self):
        self.type = type(self).__name__
        self.id = id(self)
        self.name = ""
    
#endregion

@dataclass
class Settings:
    ''' Ublend File Settings'''
    def __init__(self):
        return None
    