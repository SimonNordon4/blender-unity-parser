import ublend
import importlib
from sys import path
import time
path.append('E:\\repos\\blender-to-unity\\02-bpy')
importlib.reload(ublend.ops)
importlib.reload(ublend.data)

FILE_NAME = "blender_export.ublend"
UNITY_PROJECT = 'E:\\repos\\blender-to-unity\\blender-to-unity\\'
UNITY_EXPORT = UNITY_PROJECT + 'Assets\\01-scripts\\ublend\\' + FILE_NAME


class File():
    @staticmethod
    def save_to_unity(filepath, ublend_json):
        ''' Save the UBlend Json to Unity '''
        with open(filepath, 'w') as f:
            f.write(ublend_json)
        return


start_time = time.time()
ublenddata = ublend.ops.get_u_data()
creation_time = time.time() - start_time
json = ublenddata.tojson()
serialize_time = time.time() - start_time - creation_time
File.save_to_unity(UNITY_EXPORT, ublenddata.tojson())
write_time = time.time() - start_time - serialize_time - creation_time

print("Creation Time: " + str(creation_time))
print("Serialize Time: " + str(serialize_time))
print("Write Time: " + str(write_time))
