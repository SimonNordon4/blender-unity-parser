''' Global Settings for Blender to Unity'''
AXIS_UP_Y = True

@staticmethod
def set_axis_up_y(set_bool):
    ''' Determines whether or not to swap the Y and Z axis to match Unity '''
    global AXIS_UP_Y
    AXIS_UP_Y = set_bool