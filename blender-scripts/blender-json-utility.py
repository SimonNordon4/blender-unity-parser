# Clear console.
from operator import truediv
import os
os.system("CLS")

# Demonstration on how to use JSON for Blender.
import json

#define a dictionary.

dictionary = {
    "string": "a string 1",
    "int": 1,
    "float": 1.000,
    "isTrue": True,
    "isTrue2": False,
    "null": None,
    "D2":{
        "s2": "string two",
        "D3": {
            "s3": "string three",
            "D4": {
                "s4": "string four",
                "D5": {
                "s5": "string five",
                },
            },
        },
    },
    "stringList":["one","two","three"],
    "stringTuple":("one",1,1.000)
}

# Serialize to json.

jsonData = json.dumps(dictionary)

# Save the json in the curretn scripts directory.
scriptPath = os.path.realpath(__file__)
scriptDir = os.path.dirname(scriptPath)
jsonDataDir = scriptDir + r"\demoData.json"

# Convert Dictionary Data to Json (not saved)
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