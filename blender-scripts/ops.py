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
    def get_uvs(mesh):
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
    def get_submesh_triangles(mesh):
        ''' Returns a list of submesh triangles for each material slot on the object. Materials returns the correct number of material slots, even if no material is defined.'''
        mat_num = len(mesh.materials)
        if mat_num >= 1:
            triangles = []
            for i in range(mat_num): # instantiate triangle lists
                triangles.append([])
            for tri in mesh.loop_triangles: # append triangles to respective material id (trianlges) lists.
                triangles[tri.material_index].append(tri.loops[0])
                triangles[tri.material_index].append(tri.loops[2])
                triangles[tri.material_index].append(tri.loops[1])
        else: # if there's no materials we don't have to worry about submeshes, but we still need to append a list of lists to ensure correct serialisation.
            triangles = []
            triangles.append([])
            for tri in mesh.loop_triangles:
                triangles[0].append(tri.loops[0])
                triangles[0].append(tri.loops[2])
                triangles[0].append(tri.loops[1])
        submesh_triangles = []
        for tris in triangles:
            submesh_triangles.append(tris)
        return submesh_triangles
    
    @staticmethod
    def convert(obj):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = ublend.data.UnityMesh()
        u_mesh.name = obj.data.name

        # TODO: apply modifiers and create virtual copy of the mesh.
        o_mesh = obj.data
        o_mesh.calc_loop_triangles()
        o_mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
        
        # VERTICES & NORMALS
        u_mesh.vertices  , u_mesh.normals = BMeshToUMesh.get_vertices_and_normals(o_mesh)

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        mat_num = len(o_mesh.materials)
        u_mesh.submesh_count = mat_num if mat_num != 0 else 1
        u_mesh.submesh_triangles = BMeshToUMesh.get_submesh_triangles(o_mesh)

        # UV MAPS
        u_mesh.uvs = BMeshToUMesh.get_uvs(o_mesh)
        
        return u_mesh