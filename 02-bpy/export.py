import importlib
from sys import path
path.append('E:\\repos\\blender-to-unity\\02-bpy')
import time
import ublend
from ublend import settings

importlib.reload(ublend)
importlib.reload(ublend.ops)
importlib.reload(ublend.data)
importlib.reload(ublend.settings)

FILE_NAME = "blender_export.ublend"
UNITY_PROJECT = 'E:\\repos\\blender-to-unity\\blender-to-unity\\'
UNITY_EXPORT = UNITY_PROJECT + 'Assets\\01-scripts\\ublend\\' + FILE_NAME

settings.set_embed_textures(True)

# settings
@staticmethod
def save_to_unity(filepath, ublend_json):
    ''' Save the UBlend Json to Unity '''
    with open(filepath, 'w') as f:
        f.write(ublend_json)


startTime = time.time()
ublenddata = ublend.ops.get_u_data()

json = ublenddata.tojson()
save_to_unity(UNITY_EXPORT, ublenddata.tojson())
endTime = time.time()
print("Export Completed in " + str(endTime - startTime) + " seconds")