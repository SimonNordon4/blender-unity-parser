from dataclasses import dataclass
@staticmethod
def set_vertices_and_normals(mesh,verts,norms):
    '''Set verts and normals with Vector3'''
    for loop in mesh.loops:
        norm = Vector3(loop.normal.x, loop.normal.z, loop.normal.y)
        # now we have to do a tricky by getting vertices multiple times to match the split normals.
        v = (mesh.vertices[loop.vertex_index].co)
        vert = Vector3(v.x,v.z,v.y)
        verts.append(vert)
        norms.append(norm)
        
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

@dataclass
class Vector3():
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z