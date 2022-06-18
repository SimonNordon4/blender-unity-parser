using System;
using UnityEngine;

namespace BlenderToUnity
{
    /// <summary>
    /// A type as defined by DNA1.
    /// </summary>
    [System.Serializable]
    public struct DNAType
    {
        [field:SerializeField]
        public int TypeIndex {get; set;}

        [field:SerializeField]
        public string TypeName {get; set;}

        [field:SerializeField]
        public short Size {get;set;}


        // TODO Replace with enum (Void, Primitive, Struct)

        [field:SerializeField]
        public bool IsPrimitive {get;set;}

        [field: SerializeField]
        public DNAStruct DnaStruct{get;set;}

    }
}
