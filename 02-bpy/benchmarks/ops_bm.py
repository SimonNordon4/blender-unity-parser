from dataclasses import dataclass
import enum
@staticmethod
def set_vertices_and_normals(mesh,verts,norms):
    '''Set verts and normals with Vector3'''
    # mverts = mesh.vertices
    # for loop in mesh.loops:
    #     og_normal = loop.normal
    #     norm = Vector3(og_normal.x, og_normal.z, og_normal.y)
    #     v = (mverts[loop.vertex_index].co)
    #     vert = Vector3(v.x,v.z,v.y)
    #     verts.append(vert)
    #     #norms.append(norm)
    norms = [[loop.normal.x,loop.normal.y,loop.normal.z] for loop in mesh.loops]
        
        
@staticmethod
def set_vertices_and_normals_list(mesh,verts,norms):
    '''Set verts and normals with Vector3'''
    for loop in mesh.loops:
        norm = [loop.normal.x, loop.normal.z, loop.normal.y]
        # now we have to do a tricky by getting vertices multiple times to match the split normals.
        v = (mesh.vertices[loop.vertex_index].co)
        vert = [v.x,v.z,v.y]
        verts.append(vert)
        norms.append(norm)

'''https://stackoverflow.com/questions/53388451/how-to-speed-up-python-instance-initialization-for-millions-of-objects'''
@dataclass
class Vector3():
    __slots__ = ('x', 'y', 'z')
    def __init__(self, x=0, y=0, z=0):
        self.x = x
        self.y = y
        self.z = z
        

        