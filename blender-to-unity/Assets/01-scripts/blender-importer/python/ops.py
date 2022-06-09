''' Operations '''
import meshes
import data
import bpy

@staticmethod
def get_blend_data():
    ''' Get Blend Data '''
    blend_data = data.BlendData()
    blend_data.blend_meshes = meshes.get_blend_meshes()
    blend_data.blend_gameobjects = get_blend_gameobjects()
    return blend_data

@staticmethod
def get_blend_gameobjects():
    ''' Get Blend GameObjects '''
    blend_gameobjects = []

    for obj in bpy.data.objects:
        if obj.type == "MESH":
            go = data.BlendGameObject()
            go.name_id = obj.name

            # Set Transform
            # obj.rotation_mode = 'XYZ'
            go.parent_id = obj.parent.name if obj.parent else None
            pos = obj.location
            rot = obj.rotation_euler
            scale = obj.scale

            go.position = data.Vector3(pos.x, pos.z, pos.y)
            go.rotation = data.Vector3(rot.x * -1, rot.z * -1, rot.y * -1)
            go.scale = data.Vector3(scale.x, scale.z, scale.y)

            # Set Mesh Filter/Renderer
            go.mesh_id = obj.data.name  # We can do this because we are only looking at meshes
            # in blender mats are tied to meshes
            go.material_slots = len(obj.material_slots)
            # go.material_ids = [mat.name for mat in obj.data.materials]
            # TODO: append mats including empty slots :)
            blend_gameobjects.append(go)

    return blend_gameobjects
