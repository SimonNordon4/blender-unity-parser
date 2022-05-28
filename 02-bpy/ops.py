import bpy
from benchmarks.ops_bm import set_vertices_and_normals
import data

@staticmethod
def get_u_data():
    ''' Get the data from the Blender scene '''
    ublend = data.UData()
    set_u_meshes(ublend.u_meshes)
    return ublend

def set_u_meshes(u_meshes):
    ''' Get all meshes from the Blender scene '''
    for mesh in bpy.data.meshes:
        u_mesh = MeshToUMesh.convert(mesh)
        u_meshes.append(u_mesh)


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

class MeshToUMesh:
    ''' Converts a Blender Mesh to a Json Mesh '''
    def __init__(self):
        MeshToUMesh.self = self
            
    @staticmethod
    def set_uvs(mesh,u_mesh):
        ''' Return up to the first 8 uv maps'''
        layer_uv = mesh.uv_layers[0]
        if layer_uv:
            u_mesh.uv = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[1]
        if layer_uv:
            u_mesh.uv2 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv2[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[2]
        if layer_uv:
            u_mesh.uv3 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv3[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[3]
        if layer_uv:
            u_mesh.uv4 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv4[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[4]
        if layer_uv:
            u_mesh.uv5 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv5[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[5]
        if layer_uv:
            u_mesh.uv6 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv6[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[6]
        if layer_uv:
            u_mesh.uv7 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv7[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
        layer_uv = mesh.uv_layers[7]
        if layer_uv:
            u_mesh.uv8 = [data.Vector2()]*len(layer_uv.data) #initialise Vector2 Array.
            for j,_data in enumerate(layer_uv.data):
                vec2 = u_mesh.uv8[j]
                _uv = _data.uv
                vec2.x = _uv.x
                vec2.y = _uv.y
            
    @staticmethod
    def set_uvs_old(mesh,u_mesh):
        ''' Return up to the first 8 uv maps'''
        if(mesh.uv_layers[0]):
            for d in mesh.uv_layers[0].data:
                u_mesh.uv.append(data.Vector2(d.uv.x,d.uv.y))

    @staticmethod
    def set_submeshes(loop_triangles,submeshes):
        ''' Set all relevent submeshes
        loop_triangles = b_mesh.loop_traingles, count = submeshcount, submeshes = submeshlist '''
        for tri in loop_triangles:
            submesh = submeshes[tri.material_index] # submesh is always related to the material index.
            submesh.triangles.append(tri.loops[0])
            submesh.triangles.append(tri.loops[2])
            submesh.triangles.append(tri.loops[1])
       
    @staticmethod
    def convert(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.name = mesh.name

        #fixme: apply modifiers and create virtual copy of the mesh.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
       
        # VERTICES & NORMALS
        # doing this in a functions doesn't work, probably because arrays are immutable but floats are not?
        loops = mesh.loops
        len_loops = len(loops)
        u_mesh.vertices = u_mesh.normals = [data.Vector3()]*len_loops
        mverts= mesh.vertices
        for i,loop in enumerate(loops):
            n = loop.normal
            u_mesh.normals[i].x = n.x
            u_mesh.normals[i].y = n.y
            u_mesh.normals[i].z = n.z
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mverts[loop.vertex_index].co)
            u_mesh.vertices[i].x = v.x
            u_mesh.vertices[i].y = v.y
            u_mesh.vertices[i].z = v.z


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
        MeshToUMesh.set_uvs(mesh,u_mesh)
        

        return u_mesh
    
     
    