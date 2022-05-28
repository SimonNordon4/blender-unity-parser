import bpy
from benchmarks.ops_bm import set_vertices_and_normals
import data
import settings


@staticmethod
def get_u_data():
    ''' Get the data from the Blender scene '''
    ublend = data.UData()
    set_u_meshes(ublend.u_meshes)
    set_u_materials(ublend.u_materials)
    set_u_gameobjects(ublend.u_gameobjects)
    return ublend


def set_u_meshes(u_meshes):
    ''' Get all meshes from the Blender scene '''
    for mesh in bpy.data.meshes:
        u_mesh = MeshToUMesh.convert(mesh)
        u_meshes.append(u_mesh)


def set_u_materials(u_materials):
    ''' Get all materials in the scene '''
    for material in bpy.data.materials:
        if material.node_tree is not None and material.node_tree.nodes["Principled BSDF"] is not None:
            # might have to make sure it has owners.
            u_material = MaterialToUMaterial.convert(material)
            u_materials.append(u_material)


def set_u_gameobjects(u_gameobjects):
    ''' Get All GameObjects
        get_u_transform should be replaced with get_u_components at some point'''
    for obj in bpy.data.objects:
        if obj.type == "MESH":

            go = data.UGameObject()
            go.name = obj.name

            # Set Transform
            #obj.rotation_mode = 'XYZ'
            go.parent_name = obj.parent.name if obj.parent else None
            pos = obj.location
            rot = obj.rotation_euler
            scale = obj.scale

            go.position = data.Vector3(pos.x, pos.z, pos.y)
            go.rotation = data.Vector3(rot.x*-1, rot.z*-1, rot.y*-1)
            go.scale = data.Vector3(scale.x, scale.z, scale.y)

            # Set Mesh Filter/Renderer
            go.mesh_name = obj.data.name  # We can do this because we are only looking at meshes
            # in blender mats are tied to meshes
            go.material_names = [mat.name for mat in obj.data.materials]
            u_gameobjects.append(go)


def set_u_meshfilter(u_components, obj):
    ''' Get the Mesh Filter component of a gameobject '''
    mesh_filter = data.UMeshFilter()
    mesh_filter.mesh_name = obj.data.name
    u_components.append(mesh_filter)


class MeshToUMesh:
    ''' Converts a Blender Mesh to a Json Mesh '''

    def __init__(self):
        MeshToUMesh.self = self

    @staticmethod
    def set_uvs(mesh, u_mesh):
        ''' Return up to the first 8 uv maps'''
        uv_layers_length = len(mesh.uv_layers)
        if uv_layers_length > 0:
            layer_uv = mesh.uv_layers[0]
            u_mesh.uv = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y

        if uv_layers_length > 1:
            layer_uv = mesh.uv_layers[1]
            u_mesh.uv2 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv2[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 2:
            layer_uv = mesh.uv_layers[2]
            u_mesh.uv3 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv3[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 3:
            layer_uv = mesh.uv_layers[3]
            u_mesh.uv4 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv4[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 4:
            layer_uv = mesh.uv_layers[4]
            u_mesh.uv5 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv5[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 5:
            layer_uv = mesh.uv_layers[5]
            u_mesh.uv6 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv6[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 6:
            layer_uv = mesh.uv_layers[6]
            u_mesh.uv7 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv7[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        if uv_layers_length > 7:
            layer_uv = mesh.uv_layers[7]
            u_mesh.uv8 = [data.Vector2() for i in range(len(layer_uv.data))]
            for j, _data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv8[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y

    @staticmethod
    def set_submeshes(loop_triangles, submeshes):
        ''' Set all relevent submeshes. note that tri.material_index maps to the'''
        for tri in loop_triangles:
            submesh = submeshes[tri.material_index]
            submesh.triangles.append(tri.loops[0])
            submesh.triangles.append(tri.loops[2])
            submesh.triangles.append(tri.loops[1])

    @staticmethod
    def convert(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.name = mesh.name

        # fixme: apply modifiers and create virtual copy of the mesh.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)

        # VERTICES & NORMALS
        # doing this in a functions doesn't work, probably because arrays are immutable but floats are not?
        loops = mesh.loops
        len_loops = len(loops)
        u_mesh.normals = [data.Vector3() for i in range(len_loops)]
        u_mesh.vertices = [data.Vector3() for i in range(len_loops)]
        mverts = mesh.vertices

        for i, loop in enumerate(loops):
            n = loop.normal
            u_mesh.normals[i].x = n.x
            u_mesh.normals[i].y = n.z
            u_mesh.normals[i].z = n.y
            v = (mverts[loop.vertex_index].co)
            u_mesh.vertices[i].x = v.x
            u_mesh.vertices[i].y = v.z
            u_mesh.vertices[i].z = v.y

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        mat_num = len(mesh.materials)
        if mat_num == 0:
            mat_num = 1
        for i in range(mat_num):
            submesh = data.USubMesh()
            u_mesh.submeshes.append(submesh)

        MeshToUMesh.set_submeshes(mesh.loop_triangles, u_mesh.submeshes)

        # UV MAPS
        MeshToUMesh.set_uvs(mesh, u_mesh)

        return u_mesh


class MaterialToUMaterial:
    ''' Converts a Blender Material to a JSON Material '''

    def __init__(self):
        MeshToUMesh.self = self

    @staticmethod
    def convert(material):
        ''' Convert a Blender Material to a JSON Material '''
        u_material = data.UMaterial()
        u_material.name = material.name
        u_material.shader = data.ShaderType.LIT.name
        u_material.rendertype = data.RenderType.OPAQUE.name
        bdsf = material.node_tree.nodes["Principled BSDF"]
        bc = bdsf.inputs[0].default_value
        u_material.base_color = data.Color(bc[0], bc[1], bc[2], bc[3])
        u_material.roughness = bdsf.inputs[9].default_value
        u_material.metallic = bdsf.inputs[6].default_value
        ec = bdsf.inputs[19].default_value
        u_material.emission_color = data.Color(ec[0], ec[1], ec[2], ec[3])
        return u_material
