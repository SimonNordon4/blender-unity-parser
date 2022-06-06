''' Operations for getting meshes from Blender '''
import bpy
import data

@staticmethod
def get_blend_meshes():
    ''' Get meshes from the scene '''
    blend_meshes = data.BlendMeshes()

    for obj in bpy.data.objects:
        if obj and obj.type == 'MESH':
            blend_mesh = convert(obj)
            blend_meshes.meshes.append(blend_mesh)

    return blend_meshes

@staticmethod
def convert(obj):
    ''' Convert a Blender obj mesh to a Unity mesh '''
    blend_mesh = data.BlendMesh()
    original_mesh = obj.data

    # set name
    blend_mesh.name_id = original_mesh.name

    # prepare mesh (apply modifiers, calc triangles)
    depsgraph = bpy.context.evaluated_depsgraph_get()
    evaluated_obj = obj.evaluated_get(depsgraph)
    evaluated_mesh = bpy.data.meshes.new_from_object(evaluated_obj)
    evaluated_mesh.calc_loop_triangles()
    evaluated_mesh.calc_normals_split()

    # get vertices
    loops = evaluated_mesh.loops
    len_loops = len(loops)
    blend_mesh.vertices = [float] * (len_loops * 3)
    blend_mesh.normals = [float] * (len_loops * 3)
    mverts = evaluated_mesh.vertices

    for i, loop in enumerate(loops):
        n = loop.normal
        blend_mesh.normals[i * 3] = n.x
        blend_mesh.normals[i * 3 + 1] = n.y
        blend_mesh.normals[i * 3 + 2] = n.z
        v = mverts[loop.vertex_index].co
        blend_mesh.vertices[i * 3] = v.x
        blend_mesh.vertices[i * 3 + 1] = v.y
        blend_mesh.vertices[i * 3 + 2] = v.z

    return blend_mesh
