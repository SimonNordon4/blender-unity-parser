import importlib
from sys import path
import bpy
import bmesh
import timeit
from data import USubMesh
path.append('E:\\repos\\blender-to-unity\\02-bpy')
import ublend
importlib.reload(ublend.ops)
importlib.reload(ublend.data)

# SPEED TESTS

RUN_TIMES = 10000 # *8 vertices

# Mesh ops.

bm = bmesh.new()
bmesh.ops.create_cube(bm)
mesh = bpy.data.meshes.new("Cube")
bm.to_mesh(mesh)
bm.free()

mesh.calc_loop_triangles()
mesh.calc_normals_split()

u_mesh = ublend.data.UMesh()
u_mesh.submeshes.append(ublend.data.USubMesh())

print("convert mesh to umesh " + str(timeit.timeit("ublend.ops.MeshToUMesh.convert(mesh)", number=RUN_TIMES, globals=globals())))
print("set_vertices_and_normals: " + str(timeit.timeit("ublend.ops.MeshToUMesh.set_vertices_and_normals(mesh, u_mesh.vertices, u_mesh.normals)", number=RUN_TIMES, globals=globals())))
print("set_submeshes: " + str(timeit.timeit("ublend.ops.MeshToUMesh.set_submeshes(mesh.loop_triangles, u_mesh.submeshes)", number=RUN_TIMES, globals=globals())))

bpy.data.meshes.remove(mesh)
