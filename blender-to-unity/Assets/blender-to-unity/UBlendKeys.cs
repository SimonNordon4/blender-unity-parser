using BlenderToUnity;



public class UBlendDataKey : UBlendData
{
    new public static string uGameObjects = "u_gameobjects";
    new public static string uMeshes = "u_meshes";
}

public class UMeshKey : UMesh
{

}

public class UGameObjectKey : UGameObject
{
    new public static string uName = "u_name";
    new public static string uComponents = "u_components";
}
