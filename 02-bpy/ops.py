import bpy
import data


@staticmethod
def get_u_data():
    ''' Get the data from the Blender scene '''
    print("Getting Ublend Data")
    ublend_data = data.UData()
    set_u_objects(ublend_data.u_objects)
    return ublend_data


def set_u_assets():
    ''' Not Implementd'''
    return None


def set_u_objects(objects):
    ''' Get All Blender Objects'''
    print("Getting Objects")
    # Convert Scene Objects to GameObjects. Remeber that the class type is the key
    set_u_gameobjects(objects.u_gameobjects)


def set_u_gameobjects(u_gameobjects):
    ''' Get All GameObjects
        get_u_transform should be replaced with get_u_components at some point'''
    for obj in bpy.data.objects:
        if obj.type == "MESH":
            go = data.UGameObject()
            go.name = obj.name
            set_u_transform(go.u_transform, obj)
            set_u_meshfilter(go.u_components, obj)
            u_gameobjects.append(go)


def set_u_transform(u_transform, obj):
    ''' Get the transform component of a gameobject'''
    u_transform.position = [obj.location.x, obj.location.z, obj.location.y]
    u_transform.rotation = [obj.rotation_euler.x,
                          obj.rotation_euler.z, obj.rotation_euler.y]
    u_transform.scale = [obj.scale.x, obj.scale.z, obj.scale.y]
    u_transform.parent_name = obj.parent.name if obj.parent else None
   
def set_u_meshfilter(u_components,obj):
    ''' Get the Mesh Filter component of a gameobject '''
    mesh_filter = data.UMeshFilter();
    mesh_filter.mesh_name = obj.data.name
    u_components.append(mesh_filter)
