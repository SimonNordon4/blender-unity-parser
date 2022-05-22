''' UBlend Data Classes'''
from dataclasses import dataclass
from sys import path
path.append('C:\\Users\\61426\\AppData\\Local\\Programs\\Python\\Python310\\lib\\site-packages\\orjson\\')
import orjson

@dataclass
class Data:
    ''' Data Conainer for entire UBlend file '''
    def __init__(self):
        self.assets = []
        self.gameobjects = []
        self.settings = []
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
       
@dataclass
class Objects:
    ''' Unity GameObject Representation'''
    def __init__(self):
        return None

@dataclass
class Settings:
    ''' Ublend File Settings'''
    def __init__(self):
        return None
    