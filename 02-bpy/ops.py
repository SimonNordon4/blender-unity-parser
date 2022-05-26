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
        u_mesh = MeshToUMesh.convert_new_vector3(mesh)
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
    def set_uvs(mesh,uv_maps):
        ''' Return up to the first 8 uv maps'''
        for uvlay in mesh.uv_layers:
            uv_layer = []
            for d in uvlay.data:
                uv = [d.uv.x, d.uv.y]
                uv_layer.append(uv)
            uv_maps.append(uv_layer)

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
    def convert_new_vector3(mesh):
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
        #MeshToUMesh.set_uvs(b_mesh,u_mesh.u_uvs)

        return u_mesh
    
     
    @staticmethod
    def convert_new_list(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.name = mesh.name

        #fixme: apply modifiers and create virtual copy of the mesh.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
       
        # VERTICES & NORMALS
        # doing this in a functions doesn't work, probably because arrays are immutable but floats are not?
        mverts = mesh.vertices
        for loop in mesh.loops:
            n = loop.normal
            v = (mverts[loop.vertex_index].co)
            u_mesh.vertices.append(v.x)
            u_mesh.vertices.append(v.z)
            u_mesh.vertices.append(v.y)
            u_mesh.normals.append(n.x)
            u_mesh.normals.append(n.z)
            u_mesh.normals.append(n.y)

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
        #MeshToUMesh.set_uvs(b_mesh,u_mesh.u_uvs)

        return u_mesh
    
    @staticmethod
    def convert_old_vector3(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.name = mesh.name

        #fixme: apply modifiers and create virtual copy of the mesh.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
       
        # VERTICES & NORMALS
        # doing this in a functions doesn't work, probably because arrays are immutable but floats are not?
        for loop in mesh.loops:
            norm = data.Vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            v = (mesh.vertices[loop.vertex_index].co)
            vert = data.Vector3(v.x,v.z,v.y)
            u_mesh.vertices.append(vert)
            u_mesh.normals.append(norm)

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
        #MeshToUMesh.set_uvs(b_mesh,u_mesh.u_uvs)

        return u_mesh
    
    @staticmethod
    def convert_old_list(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.name = mesh.name

        #fixme: apply modifiers and create virtual copy of the mesh.
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
       
        # VERTICES & NORMALS
        # doing this in a functions doesn't work, probably because arrays are immutable but floats are not?
        for loop in mesh.loops:
            norm = [loop.normal.x, loop.normal.z, loop.normal.y]
            v = (mesh.vertices[loop.vertex_index].co)
            vert = [v.x,v.z,v.y]
            u_mesh.vertices.append(vert)
            u_mesh.normals.append(norm)

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
        #MeshToUMesh.set_uvs(b_mesh,u_mesh.u_uvs)

        return u_mesh
    