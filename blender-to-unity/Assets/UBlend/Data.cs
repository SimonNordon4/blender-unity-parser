using System;
using System.Collections.Generic;
using UnityEngine;

namespace UBlend{
    /// <summary>
    /// U Blend Data Class
    /// </summary>
    [Serializable]
    public class Data 
    {

    }

    #region Asset Defintions
    [Serializable]
    public class Assets
    {
        public List<Mesh> meshes;
        public List<Material> materials;
        public List<Texture> textures;
    }
    [Serializable]
    public class Mesh
    {

    }
    [Serializable]
    public class Material
    {

    }
    [Serializable]
    public class Texture
    {

    }

    #endregion

    #region Object Defintions
    [Serializable]
    public class Object
    {
        public List<GameObject> gameObjects;
    }
    [Serializable]
    public class GameObject
    {

    }

    #endregion
    [Serializable]
    public class Settings
    {

    }
}