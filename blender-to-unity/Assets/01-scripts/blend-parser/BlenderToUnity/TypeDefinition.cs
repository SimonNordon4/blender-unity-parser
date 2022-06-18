using System;

namespace BlenderToUnity
{
    /// <summary>
    /// A type as defined by DNA1.
    /// </summary>
    [System.Serializable]
    public struct TypeDefinition
    {
        /// <summary>
        /// Name of the type.
        /// </summary>
        public string Name;
        /// <summary>
        /// Size in bytes of the type.
        /// </summary>
        public short Size;
        /// <summary>
        /// Indicates if this type is a primitive (non-primitive types are defined in the DNA1 block as DNAStructs).
        /// </summary>
        public bool IsPrimitive;
        
        public TypeDefinition(string typeName, short typeSize, bool typeIsPrimitive)
        {
            this.Name = typeName;
            this.Size = typeSize;
            this.IsPrimitive = typeIsPrimitive;
        }
    }
}
