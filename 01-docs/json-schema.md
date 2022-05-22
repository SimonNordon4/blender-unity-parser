# Schema

We will be using a verbose JSON schema in order to accurately transfer python data to c#.

Json Already Supports the following data types:

    - string
    - number
    - boolean
    - null
    - array

and so these types can be inferred from the data.

In the case of objects however, we will want more strict casting. As such, all object types must contain the Class Type as a property.

```json 
{
    "type": "GameObject",
    "properties" : "etc..."
}
```

Going further than that, we also want to be able to define the type of the property.

```json
{
    "type": "GameObject",
    "components" : {"Component":[]}
}
```
To ensure correct serialisation, the python code will look like:

```python
    class Component():
        def __init__(self):
            self.type = "Component"

    class GameObject():
        def __init__(self):
            self.type = type(self).__name__
            self.components = {Component.__name__: []}
```

// TODO

// Add C# Component.