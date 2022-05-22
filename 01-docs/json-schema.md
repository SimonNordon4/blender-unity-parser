# Naming Convention

Before we talk through the JSON Component, let's go over naming. We should do our best to have consistent naming across python, json and c#. This means adopting snake_case for all fields, and camelCase for all classes. 

    All UBlend Classes should be abbreviated with 'U'. (eg. UMesh)

    All UBlend Objects Fields and Object Array Fields should be abbreviated with 'u_'. (eg. u_meshes)

    Any string, number or boolean field can be left as is. This includes lists containting this type. (eg. u_mesh.name)

Blender and Unity share many similarities. Example we could in theory have 3 mesh types: 

    Blender.Mesh, UBlend.Mesh, Unity.Mesh

This explicit naming will help avoid confusion.


# Schema

We will be using a verbose JSON schema in order to accurately transfer python data to c#.

Json Already Supports the following data types:

    - string
    - number
    - boolean
    - null
    - array

and so these types can be inferred from the data. In the case of objects however, we will want more strict casting. As such, all object types must contain the Class Type as a property.

```json 
{
    "type": "UGameObject",
    "u_properties" : "etc..."
}
```

Going further than that, we also want to be able to define the type of the property if it is an Object or array of Objects using a Dictionary with the type as the key.

```json
{
    "type": "UGameObject",
    "u_components" : {"UComponent":[]}
}
```
To ensure correct serialisation, the python code will look like:

```python
    class Component():
        def __init__(self):
            self.type = type(self).__name__

    class GameObject():
        def __init__(self):
            self.type = type(self).__name__
            self.u_components = {Component.__name__: []}
```

When it comes to C#, we'll be using a JObject (Dictionary) to deserialize the Json ourselves. We achieve this by ensuring python - C# parity. For example, we can access a Json propert as follows:

```csharp

    jObject = JObject.Parse(uBlendFileContents);
    // Todo show how to access keys value pairs using the property name itself


```