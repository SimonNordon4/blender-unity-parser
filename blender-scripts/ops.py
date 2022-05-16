import numpy as np # we can't tojson a np array, find away to concatenate without it.
import ublend

# Each submesh is a subset of triangles, so we'll still get verts and normals as before, but triangles will have to be assigned on a per material basis.
class BMeshToUMesh:
    ''' Converts a Blender Mesh to a uMesh'''
    def __init__(self):
        BMeshToUMesh.self = self
        
    @staticmethod
    def get_vertices_and_normals(mesh):
        '''Does Stuff'''
        print("Getting Verts & normals")
        vertices = []
        normals = []
        for loop in mesh.loops:
            norm = ublend.data.Vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = ublend.data.Vector3(v.x, v.z, v.y)
            vertices.append(vert)
            normals.append(norm)
        return vertices, normals

    @staticmethod
    def get_uv(mesh):
        ''' Return up to the first 8 uv maps'''
        uv_maps = []
        for uvlay in mesh.uv_layers:
            uv_layer = []
            for d in uvlay.data:
                uv = ublend.data.Vector2(d.uv.x, d.uv.y) 
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps
    
    @staticmethod
    def get_triangles(mesh):
        ''' Returns the meshes triangles for each of the meshes materials in a single array'''
        mat_num = len(mesh.materials)
        if mat_num >= 1:
            submesh_triangles = []
            for i in range(mat_num): # instantiate triangle lists
                submesh_triangles.append([])
            for tri in mesh.loop_triangles: # append triangles to respective material id (trianlges) lists.
                submesh_triangles[tri.material_index].append(tri.loops[0])
                submesh_triangles[tri.material_index].append(tri.loops[2])
                submesh_triangles[tri.material_index].append(tri.loops[1])
            triangles = []
            for sm_tri in submesh_triangles: # add all the submesh triangles into a single list, in the correct order.
                triangles = np.concatenate([triangles,sm_tri])
        else: # if there's no materials we don't have to worry about submeshes.
            triangles = []
            for tri in mesh.loop_triangles:
                triangles.append(tri.loops[0])
                triangles.append(tri.loops[2])
                triangles.append(tri.loops[1])
        return triangles
    
    @staticmethod
    def convert(obj):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = ublend.data.UnityMesh()
        u_mesh.name = obj.data.name
        u_mesh.vertices = []
        u_mesh.normals = []
        u_mesh.uv = []

        # TODO: apply modifiers and create virtual copy of the mesh.
        # prep the mesh
        o_mesh = obj.data
        o_mesh.calc_loop_triangles()
        o_mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
        
        # VERTICES & NORMALS
        u_mesh.vertices  , u_mesh.normals = BMeshToUMesh.get_vertices_and_normals(o_mesh)

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        u_mesh.submesh_count = len(o_mesh.materials)
        u_mesh.submesh_triangles = BMeshToUMesh.get_triangles(o_mesh)

        # UV MAPS
        uv_maps = BMeshToUMesh.get_uv(o_mesh)
        if len(uv_maps) > 0:
            u_mesh.uv = uv_maps[0]
        if len(uv_maps) > 1:
            u_mesh.uv2 = uv_maps[1]
        if len(uv_maps) > 2:
            u_mesh.uv3 = uv_maps[2]
        if len(uv_maps) > 3:
            u_mesh.uv4 = uv_maps[3]
        if len(uv_maps) > 4:
            u_mesh.uv5 = uv_maps[4]
        if len(uv_maps) > 5:
            u_mesh.uv6 = uv_maps[5]
        if len(uv_maps) > 6:
            u_mesh.uv7 = uv_maps[6]
        if len(uv_maps) > 7:
            u_mesh.uv8 = uv_maps[7]
            
        return u_mesh