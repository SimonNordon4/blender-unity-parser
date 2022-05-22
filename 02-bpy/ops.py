import bpy
import data

@staticmethod
def get_u_data():
    ''' Get the data from the Blender scene '''
    print("Getting Ublend Data")
    ublend_data = data.UData()
    get_u_objects(ublend_data.u_objects)
    return ublend_data

def get_u_assets():
    ''' Not Implementd'''
    return None
def get_u_objects(objects):
    ''' Not Implementd'''
    print("Getting Objects")
    # Convert Scene Objects to GameObjects. Remeber that the class type is the key
    get_u_gameobjects(objects.u_gameobjects[data.UGameObject.__name__])

def get_u_gameobjects(u_gameobjects):
    for obj in bpy.data.objects:
        if obj.type == "MESH":
            go = data.UGameObject()
            go.name = obj.name
            u_gameobjects.append(go)
            