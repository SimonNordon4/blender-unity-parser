import bpy
import data

@staticmethod
def get_ublend():
    ''' Get the data from the Blender scene '''
    print("Getting Ublend Data")
    ublend_data = data.Data()
    get_objects(ublend_data.objects)
    return ublend_data

def get_assets():
    ''' Not Implementd'''
    return None
def get_objects(objects):
    ''' Not Implementd'''
    print("Getting Objects")
    # Convert Scene Objects to GameObjects. Remeber that the class type is the key
    get_gameobjects(objects.gameobjects[data.GameObject.__name__])

def get_gameobjects(gameobjects):
    for obj in bpy.data.objects:
        if obj.type == "MESH":
            go = data.GameObject()
            go.name = obj.name
            gameobjects.append(go)
            