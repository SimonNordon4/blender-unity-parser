#!/usr/bin/env python

#based on 'export-sprites.py' and 'glsprite.py' from TCHOW Rainbow; code used is released into the public domain.

#Note: Script meant to be executed from within blender, as per:
#blender --background --python export-layer.py

#reads 'robot.blend' and writes 'robot.blob' from the first layer.

import sys

import bpy
import struct

print(sys.argv)

args = None
for arg in sys.argv:
	if arg == '--':
		args = []
	elif args != None:
		args.append(arg)

if args == None: args = []

if len(args) != 2:
	print("Usage:\n\tblender --background --python export-layer.py -- <in.blend> <out.v3n3c4>")
	exit(1)

infile = args[0]
outfile = args[1]

print("Reading '" + infile + "'")
bpy.ops.wm.open_mainfile(filepath=infile)

#data contains vertex, normal, and vertex color data from the meshes:
data_vertices = b''
data_normals = b''
data_colors = b''

vertex_count = 0
for obj in bpy.data.objects:
	if obj.type != 'MESH': continue;
	if obj.layers[0] == False: continue
	print("Writing '" + obj.name + "'...")
	bpy.ops.object.mode_set(mode='OBJECT') #get out of edit mode (just in case)

	obj.data = obj.data.copy() #make mesh single user, just in case it is shared with another object the script needs to write later.

	#make sure object is on a visible layer:
	bpy.context.scene.layers = obj.layers
	#select the object and make it the active object:
	bpy.ops.object.select_all(action='DESELECT')
	obj.select = True
	bpy.context.scene.objects.active = obj

	#subdivide object's mesh into triangles:
	bpy.ops.object.mode_set(mode='EDIT')
	bpy.ops.mesh.select_all(action='SELECT')
	bpy.ops.mesh.quads_convert_to_tris(quad_method='BEAUTY', ngon_method='BEAUTY')
	bpy.ops.object.mode_set(mode='OBJECT')

	#compute normals (respecting face smoothing):
	mesh = obj.data
	mesh.calc_normals_split()

	matrix = obj.matrix_world
	normal_matrix = matrix.to_3x3()
	normal_matrix.transpose()
	normal_matrix.invert()

	colors = mesh.vertex_colors.active.data

	#write the mesh:
	for poly in mesh.polygons:
		assert(len(poly.loop_indices) == 3)
		for i in range(0,3):
			assert(mesh.loops[poly.loop_indices[i]].vertex_index == poly.vertices[i])
			loop = mesh.loops[poly.loop_indices[i]]
			color = colors[poly.loop_indices[i]].color
			vertex = matrix * mesh.vertices[loop.vertex_index].co
			normal = normal_matrix * loop.normal
			for x in vertex:
				data_vertices += struct.pack('f', x)
			for x in normal:
				data_normals += struct.pack('f', x)
			data_colors += struct.pack('BBBB',
				int(color.r * 255),
				int(color.g * 255),
				int(color.b * 255),
				255
			)

	vertex_count += len(mesh.polygons) * 3

#write the data chunk and index chunk to an output blob:
blob = open(outfile, 'wb')
#first chunk: the data
blob.write(data_vertices)
blob.write(data_normals)
blob.write(data_colors)

print("Wrote " + str(blob.tell()) + " bytes to '" + outfile + "'.")