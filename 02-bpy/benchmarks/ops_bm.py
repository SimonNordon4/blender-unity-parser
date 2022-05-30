from dataclasses import dataclass
import enum
import base64
import struct
import zlib
import orjson
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
            
class BMImage:
    def __init(self):
        BMImage.self = self
        
    @staticmethod 
    def img_to_png(blender_image):
        width = blender_image.size[0]
        height = blender_image.size[1]
        buf = bytearray([int(p * 255) for p in blender_image.pixels])

        # reverse the vertical line order and add null bytes at the start
        width_byte_4 = width * 4
        raw_data = b''.join(b'\x00' + buf[span:span + width_byte_4]
                            for span in range((height - 1) * width_byte_4, -1, - width_byte_4))

        def png_pack(png_tag, data):
            chunk_head = png_tag + data
            return (struct.pack("!I", len(data)) +
                    chunk_head +
                    struct.pack("!I", 0xFFFFFFFF & zlib.crc32(chunk_head)))

        png_bytes = b''.join([
            b'\x89PNG\r\n\x1a\n',
            png_pack(b'IHDR', struct.pack("!2I5B", width, height, 8, 6, 0, 0, 0)),
            png_pack(b'IDAT', zlib.compress(raw_data, 9)),
            png_pack(b'IEND', b'')])

        return 'data:image/png;base64,' + base64.b64encode(png_bytes).decode()
    @staticmethod
    def direct_save(image,path):
        image.save_render(path + image.name + "." + image.file_format.lower())
    @staticmethod
    def save_and_encode(image,path):
        file_path = path + image.name + "." + image.file_format.lower()
        image.save_render(file_path)
        with open(file_path, "rb") as image_file:
            image64 = base64.b64encode(image_file.read())
            return str(image64)
    @staticmethod 
    def encode_in_file(image):
        image64 = BMImage.img_to_png(image)
        return image64
    @staticmethod
    def get_pixels(image):
        pixels = [float(i) for i in image.pixels]
        return pixels
@dataclass
class BMTexture:
    def __init__(self):
        self.pixels = []
        self.image64 = ""
    def tojson(self):
        return orjson.dumps(self).decode("utf-8")
            
@staticmethod
def testjson(image,path,encode_type):
    bmtext = BMTexture()
    if encode_type == 0:
        BMImage.direct_save(image,path)
    if encode_type == 1:
        bmtext.image64 = BMImage.save_and_encode(image,path)
        json = bmtext.tojson()
    if encode_type == 2:
        bmtext.pixels = BMImage.get_pixels(image)
        json = bmtext.tojson()
    if encode_type == 3:
        bmtext.image64 = BMImage.encode_in_file(image)
        json = bmtext.tojson()
        