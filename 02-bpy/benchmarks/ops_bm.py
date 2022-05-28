from dataclasses import dataclass
import enum
@staticmethod
def set_vertices_and_normals(mesh,verts,norms):
    '''Set verts and normals with floats (This is about 0.01% faster than the vector method)'''
    mverts = mesh.vertices
    for loop in mesh.loops:
        n = loop.normal
        v = (mverts[loop.vertex_index].co)
        verts.append(v.x)
        verts.append(v.z)
        verts.append(v.y)
        norms.append(n.x)
        norms.append(n.z)
        norms.append(n.y)
        
@staticmethod
def set_vertices_and_normals_list(mesh,verts,norms):
    '''Set verts and normals with Vector3'''
    loops = mesh.loops
    len_loops = len(loops)
    verts = [Vector3]*len_loops
    norms = [Vector3]*len_loops
    for i,loop in enumerate(loops):
        n = loop.normal
        # now we have to do a tricky by getting vertices multiple times to match the split normals.
        v = (mesh.vertices[loop.vertex_index].co)
        verts[i].x = v.x
        verts[i].y = v.y
        verts[i].z = v.z
        norms[i].x = n.x
        norms[i].y = n.y
        norms[i].z = n.z

class Vector3:
    def __init__(self,x=0,y=0,z=0):
        self.x = x
        self.y = y
        self.z = z
        
class Vector2:
    def __init__(self,x=0,y=0):
        self.x = x
        self.y = y
        
@staticmethod
def set_uvs(mesh,u_mesh):
    ''' Return up to the first 8 uv maps'''
    if(mesh.uv_layers[0]):
        u_mesh.uv = [Vector2]*len(mesh.uv_layers[0].data) #initialise Vector2 Array.
        for i,d in enumerate(mesh.uv_layers[0].data):
            u_mesh.d[i] = Vector2(d.uv[0],d.uv[1])