import bpy
from data import UGameObject, Vector3
import ublend

class CreateUBlend:
    ''' Entry Point for UBlend Serialisation'''
    def __init__(self):
        CreateUBlend.self = self
    @staticmethod
    def get_scene_meshes():
        ''' Gather all meshes in the file with at least 1 user.'''
        scene_meshes = []
        for mesh in bpy.data.meshes:
            if(mesh.users > 0):
                scene_meshes.append(mesh)
        return scene_meshes
    
    @staticmethod
    def get_scene_objects():
        ''' Gather all objects in the scene'''
        scene_objects = []
        for obj in bpy.data.objects:
            if obj.type == "MESH": # For now only get objects that are mesh types
                scene_objects.append(obj)
        return scene_objects;
            
    @staticmethod
    def create_ublend():
        ''' Construct the ublend object for serialisation'''
        u_blend = ublend.data.UBlendData()
        # Serialise Meshes
        meshes = CreateUBlend.get_scene_meshes()
        for mesh in meshes:
            u_mesh = MeshToUMesh.convert(mesh)
            u_blend.u_meshes.append(u_mesh)
            
        # Check for Images
        # Check for Materials

        # Assets collected, now gather objects and their components.
        objects = CreateUBlend.get_scene_objects()
        for obj in objects:
            u_obj = ObjectToUGameObject.convert(obj)
            u_blend.u_gameobjects.append(u_obj)
        
        return u_blend

class MeshToUMesh:
    ''' Converts a Blender Mesh to a Json Mesh'''
    def __init__(self):
        MeshToUMesh.self = self
        
    @staticmethod
    def get_vertices_and_normals(mesh):
        '''Does Stuff'''
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
    def convert(mesh):
        ''' Convert a Blender Mesh to a UMesh Class (Representation of Unity Mesh)'''
        u_mesh = ublend.data.UMesh()
        u_mesh.u_name = mesh.name

        # TODO: apply modifiers and create virtual copy of the mesh.
        b_mesh = mesh
        b_mesh.calc_loop_triangles()
        b_mesh.calc_normals_split()  # Split Normals are only accessible via loops (not verts)
        
        # VERTICES & NORMALS
        u_mesh.u_vertices  , u_mesh.u_normals = MeshToUMesh.get_vertices_and_normals(b_mesh)

        # SUBMESH TRIANGLES
        # Get the submesh count, then use that to initialise the submesh_triangles list.
        mat_num = len(b_mesh.materials)
        u_mesh.u_submesh_count = mat_num if mat_num != 0 else 1
        u_mesh.u_submesh_triangles = MeshToUMesh.get_submesh_triangles(b_mesh)

        # UV MAPS
        u_mesh.u_uvs = MeshToUMesh.get_uvs(b_mesh)
        
        return u_mesh

class ObjectToUGameObject:
    ''' Convert a Blender Object to a JGameObject'''
    def __init__(self):
        ObjectToUGameObject.self = self
    @staticmethod
    def convert(obj):
        ''' Returns a uGameObject from a blender object'''
        u_gameobject = ublend.data.UGameObject()
        u_gameobject.u_name = obj.name
        
        # Apply gauranteed transform.
        
        u_transform = ublend.data.UTransform()
        u_transform.u_position = Util.vec3(obj.location)
        u_transform.u_rotation = Util.vec3(obj.rotation_euler)
        u_transform.u_scale = Util.vec3(obj.scale)
        u_gameobject.u_components.append(u_transform)
        
        umesh_filter = ublend.data.UMeshFilter(obj.data.name)
        umesh_filter.u_mesh_ref = obj.data.name
        u_gameobject.u_components.append(umesh_filter) # We can add the mesh because we've already pre-filtered object for meshes.
        return u_gameobject
        
class Util:
    ''' Collection of utilites '''
    def __init__(self):
        self.self = self
    @staticmethod
    def vec3(vector3):
        return Vector3(vector3.x,vector3.y,vector3.z)
