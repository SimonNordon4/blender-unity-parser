''' Operations for getting meshes from Blender '''
import bpy
import data
import settings

@staticmethod
def get_blend_meshes():
    ''' Get meshes from the scene '''
    blend_meshes = data.BlendMeshes()

    for obj in bpy.data.objects:
        if obj and obj.type == 'MESH':
            blend_mesh = convert_mesh(obj)
            blend_meshes.meshes.append(blend_mesh)

    return blend_meshes


# TODO: Implement
@staticmethod
def clip(float_value):
    ''' Clip a float value to the vec_precision '''
    p = 1 / settings.vec_precision
    return int(float_value * p) / p

@staticmethod
def convert_mesh(obj):
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
        blend_mesh.normals[i * 3 + 1] = n.z  # zy flip
        blend_mesh.normals[i * 3 + 2] = n.y
        v = mverts[loop.vertex_index].co
        blend_mesh.vertices[i * 3] = v.x
        blend_mesh.vertices[i * 3 + 1] = v.z # zy flip)
        blend_mesh.vertices[i * 3 + 2] = v.y

    # get triangles
    mat_num = len(evaluated_mesh.materials)
    if(mat_num == 0):
        mat_num = 1
    for i in range(mat_num):
        sub_mesh = data.BlendSubMesh()
        blend_mesh.sub_meshes.append(sub_mesh)

    loop_triangles = evaluated_mesh.loop_triangles
    sub_meshes = blend_mesh.sub_meshes
    for tri in loop_triangles:
        sub_mesh = sub_meshes[tri.material_index]
        sub_mesh.triangles.append(tri.loops[0])
        sub_mesh.triangles.append(tri.loops[2])
        sub_mesh.triangles.append(tri.loops[1])

    # Clean Up
    bpy.data.meshes.remove(evaluated_mesh)

    return blend_mesh
