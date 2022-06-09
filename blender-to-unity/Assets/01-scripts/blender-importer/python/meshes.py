''' Operations for getting meshes from Blender '''
import bpy
import data
import settings

@staticmethod
def get_blend_meshes():
    ''' Get meshes from the scene '''
    blend_meshes = []

    for obj in bpy.data.objects:
        if obj and obj.type == 'MESH':
            blend_mesh = convert_mesh(obj)
            blend_meshes.append(blend_mesh)

    return blend_meshes


# TODO: Implement
@staticmethod
def clip(float_value):
    ''' Clip a float value to the vec_precision '''
    p = 1 / settings.vec_precision
    return int(float_value * p) / p

@staticmethod
def get_vertices_and_normals(mesh, blend_mesh):
    loops = mesh.loops
    len_loops = len(loops)
    blend_mesh.vertices = [float] * (len_loops * 3)
    blend_mesh.normals = [float] * (len_loops * 3)
    mverts = mesh.vertices

    for i, loop in enumerate(loops):
        n = loop.normal
        index = i * 3
        blend_mesh.normals[index] = n.x
        blend_mesh.normals[index + 1] = n.z  # zy flip
        blend_mesh.normals[index + 2] = n.y
        v = mverts[loop.vertex_index].co
        blend_mesh.vertices[index] = v.x
        blend_mesh.vertices[index + 1] = v.z # zy flip)
        blend_mesh.vertices[index + 2] = v.y

@staticmethod
def get_uvs(mesh,blend_mesh):
    ''' Return up to the first 8 uv maps'''
    uv_layers_length = len(mesh.uv_layers)
    if uv_layers_length > 0:
        layer_uv = mesh.uv_layers[0]
        blend_mesh.uv = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 1:
        layer_uv = mesh.uv_layers[1]
        blend_mesh.uv2 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 2:
        layer_uv = mesh.uv_layers[2]
        blend_mesh.uv3 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 3:
        layer_uv = mesh.uv_layers[3]
        blend_mesh.uv4 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 4:
        layer_uv = mesh.uv_layers[4]
        blend_mesh.uv5 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 5:
        layer_uv = mesh.uv_layers[5]
        blend_mesh.uv6 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 6:
        layer_uv = mesh.uv_layers[6]
        blend_mesh.uv7 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

    if uv_layers_length > 7:
        layer_uv = mesh.uv_layers[7]
        blend_mesh.uv8 = [float] * (len(layer_uv.data) * 2)
        for j, _data in enumerate(layer_uv.data):
            _uv = _data.uv
            index = j * 2
            blend_mesh.uv[index] = _uv.x
            blend_mesh.uv[index + 1] = _uv.y

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
    get_vertices_and_normals(evaluated_mesh, blend_mesh)

    # get uvs
    get_uvs(evaluated_mesh, blend_mesh)

    # get triangles
    number_of_materials = len(evaluated_mesh.materials)
    if(number_of_materials == 0):
        number_of_materials = 1
    for i in range(number_of_materials):
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
