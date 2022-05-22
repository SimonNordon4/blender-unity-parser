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
    ''' Get All Blender Objects'''
    print("Getting Objects")
    # Convert Scene Objects to GameObjects. Remeber that the class type is the key
    get_u_gameobjects(objects.u_gameobjects[data.UGameObject.__name__])


def get_u_gameobjects(u_gameobjects):
    ''' Get All GameObjects
        get_u_transform should be replaced with get_u_components at some point'''
    for obj in bpy.data.objects:
        if obj.type == "MESH":
            go = data.UGameObject()
            go.name = obj.name
            get_u_transform(go.u_components[data.UComponent.__name__], obj)
            u_gameobjects.append(go)


def get_u_transform(u_components, obj):
    ''' Get the transform component of a gameobject'''
    transform = data.UTransform()
    transform.position = [obj.location.x, obj.location.z, obj.location.y]
    transform.rotation = [obj.rotation_euler.x,
                          obj.rotation_euler.z, obj.rotation_euler.y]
    transform.scale = [obj.scale.x, obj.scale.z, obj.scale.y]
    transform.parent_name = obj.parent.name if obj.parent else None

    u_components.append(transform)
