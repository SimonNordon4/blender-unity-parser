# Clear console.
from operator import truediv
import os
os.system("CLS")

# Demonstration on how to use JSON for Blender.
import json

# smaller nested dictionaries can be serialized
smallerDictionary = {
    "string": "hello",
    "string2": "world"
}

#define a dictionary.
dictionary = {
    "string": "a string 1",
    "int": 1,
    "float": 1.000,
    "isTrue": True,
    "isTrue2": False,
    "null": None,
    "smalleDictionary": smallerDictionary, # We can nest dictionaries inside as well.
    "stringList":["one","two","three"],
    "stringTuple":("one",1,1.000)
}

# Serialize to json.

jsonData = json.dumps(dictionary)

# Save the json in the current scripts directory.
scriptPath = os.path.realpath(__file__)
scriptDir = os.path.dirname(scriptPath)
jsonDataDir = scriptDir + r"\demoData.json"

## Convert Dictionary Data to Json (not saved) This isn't that usable as is however, because JSON can be "double serialized" which messes it up.
# jsonData = json.dumps(dictionary)

# Write to File

with open(jsonDataDir, "w") as f:
    json.dump(dictionary, f,ensure_ascii=False,indent=4)

f.close()

# Open the File & Load it's contents.

jsonLoadFile = open(jsonDataDir, "r")
jsonData = json.load(jsonLoadFile)

print(jsonData)

jsonLoadFile.close()