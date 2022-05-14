from ublend import data
import bpy

# Each submesh is a subset of triangles, so we'll still get verts and normals as before, but triangles will have to be assigned on a per material basis.
class mesh_to_unity_submesh:
    # Serialize a Blender Mesh into a Unity Friendly JSON
    def get_vertices_and_normals(mesh):
        print("Getting Verts & normals")
        vertices = []
        normals = []
        for loop in mesh.loops:
            norm = data.vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = data.vector3(v.x, v.z, v.y)
            vertices.append(vert)
            normals.append(vert)
        return vertices, normals

    def get_uv(mesh):
        uv_maps = []
        for i in range(len(mesh.uv_layers)):
            uv_layer = []
            for j in range(len(mesh.uv_layers[i].data)):
                uv = data.vector2(mesh.uv_layers[i].data[j].uv.x, mesh.uv_layers[i].data[j].uv.y)
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps
    
    
    def get_submesh_triangles(mesh):
        mat_num = len(mesh.materials)
        submesh_count = mat_num
        submesh_triangles = []
        for i in range(mat_num):
            submesh_triangles.append([])
        for tri in mesh.loop_triangles:
            submesh_triangles[tri.material_index].append(tri.loops[0])
            submesh_triangles[tri.material_index].append(tri.loops[2])
            submesh_triangles[tri.material_index].append(tri.loops[1])
        return mat_num,submesh_triangles
    
    def get_submesh_triangles_no_material(mesh):
        submesh_triangles = []
        submesh_triangles.append([])
        for tri in mesh.loop_triangles:
            submesh_triangles[0].append(tri.loops[0])
            submesh_triangles[0].append(tri.loops[2])
            submesh_triangles[0].append(tri.loops[1])
        return 1,submesh_triangles
        
    
    def convert(obj):
        u_mesh = data.unity_mesh()
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
        u_mesh.vertices  , u_mesh.normals = mesh_to_unity_submesh.get_vertices_and_normals(o_mesh)

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        if len(o_mesh.materials) <= 0:
           u_mesh.submesh_count, u_mesh.submesh_triangles = mesh_to_unity_submesh.get_submesh_triangles_no_material(o_mesh)
        else:
           u_mesh.submesh_count, u_mesh.submesh_triangles = mesh_to_unity_submesh.get_submesh_triangles(o_mesh)

        # UV MAPS
        uv_maps = mesh_to_unity_submesh.get_uv(o_mesh)
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