## Benchmarking Serialising Blender into Json in Python (23/05/2022)

|Vertices|Create UBlend Class|Serialise Json|
|---|---|---|
| 4 | 0.0s | 0.0s |
| 1024 | 0.004s | 0.001s |
| 65,536 | 0.417s | 0.045s |
| 1,048,576 | 6.244s | 0.687s |
| 16,777,216 | 107.196s | 15.231s |

## Benchmarking Deserialisation speeds in Unity.

Deserialize 1,579,127 vertices as Vector3s from a Json file.

Serializer |Serialize | Write | Read | Deserialize |
|---|---|---|---|---|
| JsonUtility (Unity) | 3.293s | 0.446s | 0.652s | 1.755s |
| EditorJsonUtility (Unity) | 2.706s | 0.511s | 0.591s | 1.785s |
| Json.NET (NewtonSoft) | 15.48s | 0.426s | 0.527s | 33.981s |

## Benchmark Unity Deserialisation to Blender Mesh. (25/05/2022)

24,578 vertices

|Create |Serialize |Write | Read | Deserialize |
|---|---|---|---|---|
| 0.613 | 0.055s | 0.068s | 0.046 | 0.240 |
