blend_path = ""
blend_name = ""
vec_precision = 0.00001

@staticmethod
def set_blend_path(path):
    global blend_path
    blend_path = path

@staticmethod
def get_blend_path():
    return blend_path

@staticmethod
def set_blend_name(name):
    global blend_name
    blend_name = name

@staticmethod
def get_blend_name():
    return blend_name

@staticmethod
def set_vec_precision(precision):
    global vec_precision
    vec_precision = float(precision)

@staticmethod
def get_vec_precision():
    return vec_precision
