''' Global Settings for Blender to Unity'''
AXIS_UP_Y = True
EMBED_TEXTURES = False

@staticmethod
def set_axis_up_y(set_bool):
    ''' Determines whether or not to swap the Y and Z axis to match Unity '''
    global AXIS_UP_Y
    AXIS_UP_Y = set_bool
    
@staticmethod
def set_embed_textures(set_bool):
    ''' Determines whether or not to embed textures in the Unity file '''
    global EMBED_TEXTURES
    EMBED_TEXTURES = set_bool