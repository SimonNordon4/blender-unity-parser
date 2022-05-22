import bpy
import data

@staticmethod
def get_u_data():
    ''' Get the data from the Blender scene '''
    print("Getting Ublend Data")
    ublend_data = data.UData()
    set_u_assets(ublend_data.u_assets)
    set_u_objects(ublend_data.u_objects)
    return ublend_data


def set_u_assets(u_assets):
    ''' Get all Assets from the Blender scene'''
    set_u_meshes(u_assets.u_meshes)

def set_u_meshes(u_meshes):
    ''' Get all meshes from the Blender scene '''
    for mesh in bpy.data.meshes:
        u_mesh = MeshToUMesh.convert(mesh)
        u_meshes.append(u_mesh)


def set_u_objects(u_objects):
    ''' Get All Blender Objects'''
    # Convert Scene Objects to GameObjects. Remeber that the class type is the key
    set_u_gameobjects(u_objects.u_gameobjects)


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
    def set_vertices_and_normals(mesh,verts,norms):
        '''Does Stuff'''
        for loop in mesh.loops:
            norm = [loop.normal.x, loop.normal.z, loop.normal.y]
            # now we have to do a tricky by getting vertices multiple times to match the split normals.
            v = (mesh.vertices[loop.vertex_index].co)
            vert = [v.x, v.z, v.y]
            verts.append(vert)
            norms.append(norm)

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
    def set_submesh_triangles(mesh,submesh_triangles):
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
        for tris in triangles:
            submesh_triangles.append(tris)
       
    @staticmethod
    def convert(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = data.UMesh()
        u_mesh.u_name = mesh.name

        # TODO: apply modifiers and create virtual copy of the mesh.
        b_mesh = mesh
        b_mesh.calc_loop_triangles()
        b_mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
        
        # VERTICES & NORMALS
        MeshToUMesh.set_vertices_and_normals(b_mesh, u_mesh.u_vertices, u_mesh.u_normals)

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        mat_num = len(b_mesh.materials)
        u_mesh.u_submesh_count = mat_num if mat_num != 0 else 1
        MeshToUMesh.set_submesh_triangles(b_mesh,u_mesh.u_submesh_triangles)

        # UV MAPS
        MeshToUMesh.set_uvs(b_mesh,u_mesh.u_uvs)
        
        return u_mesh