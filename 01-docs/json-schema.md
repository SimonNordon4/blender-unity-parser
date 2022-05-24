# WHO'S JASON? 24/05/2022 

Json Schema will fit to EditorJsonUtility.ToJson() formatting.

It's python's duty to ensure it can be deserialized by Unity without additional plugins. General rules for serialization:

        Avoid [SerializedReference] as much as possible.
        Avoid deeply nested references as much as possible.

I've add some example of Serialized Unity Objects Below:

```json
{
    "GameObject": {
        "serializedVersion": "6",
        "m_Layer": 0,
        "m_Name": "Cube",
        "m_TagString": "Untagged",
        "m_Icon": {
            "instanceID": 0
        },
        "m_NavMeshLayer": 0,
        "m_StaticEditorFlags": 0,
        "m_IsActive": true
    }
}
```

```json
{
    "BoxCollider": {
        "m_Material": {
            "instanceID": 0
        },
        "m_IsTrigger": false,
        "m_Enabled": true,
        "serializedVersion": "2",
        "m_Size": {
            "x": 1.0,
            "y": 1.0,
            "z": 1.0
        },
        "m_Center": {
            "x": 0.0,
            "y": 0.0,
            "z": 0.0
        }
    }
}
```

```json
{
    "Transform": {
        "m_LocalRotation": {
            "x": 0.0,
            "y": 0.0,
            "z": 0.0,
            "w": 1.0
        },
        "m_LocalPosition": {
            "x": 0.0,
            "y": 0.0,
            "z": 0.0
        },
        "m_LocalScale": {
            "x": 1.0,
            "y": 1.0,
            "z": 1.0
        },
        "m_ConstrainProportionsScale": false,
        "m_RootOrder": 0,
        "m_LocalEulerAnglesHint": {
            "x": 0.0,
            "y": 0.0,
            "z": 0.0
        }
    }
}
```
We don't want to deserialize directly into Unity Objects (good luck deserializing untyped image binaries). Instead will use UBlend as an intermediate that contains data we can easily convert over. 

Getting JSON is easy, just serialize our intermediate UBlend Data type and ensure it's deserializable. Then just work in python to match the exact same format.