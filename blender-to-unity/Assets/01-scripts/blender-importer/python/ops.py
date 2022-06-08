''' Operations '''
import meshes
import data

@staticmethod
def get_blend_data():
    ''' Get Blend Data '''
    blend_data = data.BlendData()
    blend_data.blend_meshes = meshes.get_blend_meshes()
    return blend_data
