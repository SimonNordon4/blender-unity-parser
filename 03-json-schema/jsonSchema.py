from sys import path
import orjson

class MainFile():
    def __init__(self):
        self.type = (type(self).__name__,"string")
        self.id = id(self)
        self.gameobjects = [GameObject.__name__,[]]
        
    def tojson(self):
        return orjson.dumps(self, default=lambda o: o.__dict__).decode("utf-8")
    

class GameObject():
    def __init__(self):
        self.type = type(self).__name__
        self.id = id(self)
        self.components = [Component.__name__,[]]
        

class Transform():
    def __init__(self):
        self.type = type(self).__name__
        self.id = id(self)
        self.parent_id = None
        self.position = None

class Component():
    def __init__(self):
        return None

class Vector3():
    def __init__(self):
        return None

# # Main Program

mf = MainFile()

go1 = GameObject()
t1 = Transform()
t1.position = [30,20,10]

go1.components.append(t1)

mf.gameobjects[1].append(go1)

json = mf.tojson()
print(json)