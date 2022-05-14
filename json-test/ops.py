from ublend import data

class mesh_to_unity_mesh:
    # Serialize a Blender Mesh into a Unity Friendly JSON
    def get_uv(mesh):
        uv_maps = []
        for i in range(len(mesh.uv_layers)):
            uv_layer = []
            for j in range(len(mesh.uv_layers[i].data)):
                uv = data.vector2(mesh.uv_layers[i].data[j].uv.x, mesh.uv_layers[i].data[j].uv.y)
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps
    
    def convert(obj):
        u_mesh = data.unity_mesh()
        u_mesh.name = obj.name + "_loops"
        u_mesh.vertices = []
        u_mesh.normals = []
        u_mesh.triangles = []
        u_mesh.uv = []

        # TODO: apply modifiers and create virtual copy of the mesh.
        # prep the mesh
        mesh = obj.data
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)

        for loop in mesh.loops:
            norm = data.vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = data.vector3(v.x, v.z, v.y)
            u_mesh.vertices.append(vert)
            u_mesh.normals.append(norm)

        for tri in mesh.loop_triangles:
            u_mesh.triangles.append(tri.loops[0])
            u_mesh.triangles.append(tri.loops[2])
            u_mesh.triangles.append(tri.loops[1])

        # Get UV Maps
        
        uv_maps = mesh_to_unity_mesh.get_uv(mesh)
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


# Each submesh is a subset of triangles, so we'll still get verts and normals as before, but triangles will have to be assigned on a per material basis.
class mesh_to_unity_submesh:
    # Serialize a Blender Mesh into a Unity Friendly JSON
    def get_vertices_and_normals(mesh):
        vertices = []
        normals = []
        for loop in mesh.loops:
            norm = data.vector3(loop.normal.x, loop.normal.z, loop.normal.y)
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = data.vector3(v.x, v.z, v.y)

    
    def get_uv(mesh):
        uv_maps = []
        for i in range(len(mesh.uv_layers)):
            uv_layer = []
            for j in range(len(mesh.uv_layers[i].data)):
                uv = data.vector2(mesh.uv_layers[i].data[j].uv.x, mesh.uv_layers[i].data[j].uv.y)
                uv_layer.append(uv)
            uv_maps.append(uv_layer)
        return uv_maps
    
    def get_sub_mesh_count(mesh):
        '''TODO: Impliment'''
        return 2

    def convert(obj):
        u_mesh = data.unity_mesh()
        u_mesh.name = obj.name + "_loops"
        u_mesh.vertices = []
        u_mesh.normals = []
        u_mesh.triangles = []
        u_mesh.uv = []

        # TODO: apply modifiers and create virtual copy of the mesh.
        # prep the mesh
        mesh = obj.data
        mesh.calc_loop_triangles()
        mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)


        # We'll need to do this for each sub mesh.
        for tri in mesh.loop_triangles:
            u_mesh.triangles.append(tri.loops[0])
            u_mesh.triangles.append(tri.loops[2])
            u_mesh.triangles.append(tri.loops[1])

        # Get UV Maps
        
        uv_maps = mesh_to_unity_submesh.get_uv(mesh)
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
            
        
        #sub meshes    
        u_mesh.subMeshCount = mesh_to_unity_submesh.get_sub_mesh_count(mesh)

        return u_mesh