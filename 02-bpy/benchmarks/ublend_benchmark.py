import importlib
from sys import path
import bpy
import bmesh
import timeit
import inspect
path.append('E:\\repos\\blender-to-unity\\02-bpy')
path.append('E:\\repos\\blender-to-unity\\02-bpy\\benchmarks')
from data import USubMesh
import ublend
import benchmarks.ops_bm as ops_bm
importlib.reload(ublend.ops)
importlib.reload(ublend.data)
importlib.reload(ops_bm)

# SPEED TESTS

RUN_TIMES = 10 # *8 vertices

def benchmark(func, *args):
    ''' Simpler way to benchmark a function, but very hacky hehe '''
    frame = inspect.currentframe()
    frame = inspect.getouterframes(frame)[1]
    string = inspect.getframeinfo(frame[0]).code_context[0].strip()
    args = string[string.find('(') + 1:-1].split(',')
    
    names = []
    for i in args:
        if i.find('=') != -1:
            names.append(i.split('=')[1].strip())
        else:
            names.append(i)
    
    timeit_func = ""
    if names == 1:
        timeit_func = names[0]
    else:
        timeit_func = names[0] + "("
        for i in range(len(names)):
            if i > 0 and i != len(names) - 1:
                timeit_func += names[i] + ","
            if i == len(names) - 1:
                timeit_func += names[i] + ")"
    
    print("Benchmarking " +func.__name__ +   ": " + str(timeit.timeit(timeit_func, number=RUN_TIMES,globals=globals())))

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

#benchmark(ops_bm.set_vertices_and_normals_list, mesh,u_mesh.vertices,u_mesh.normals)
#benchmark(ops_bm.set_vertices_and_normals, mesh,u_mesh.vertices,u_mesh.normals) #1.116


benchmark(ublend.ops.MeshToUMesh.convert_new_vector3, bpy.data.meshes[0]) 
benchmark(ublend.ops.MeshToUMesh.convert_new_list, bpy.data.meshes[0]) 
benchmark(ublend.ops.MeshToUMesh.convert_old_vector3, bpy.data.meshes[0]) 
benchmark(ublend.ops.MeshToUMesh.convert_old_list, bpy.data.meshes[0]) 

bpy.data.meshes.remove(mesh)

