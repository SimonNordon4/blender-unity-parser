using System;

namespace BlenderToUnity
{
    /// <summary>
    /// A type as defined by SDNA.
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
        /// Indicates if this type is a primitive (non-primitive types are defined in the SDNA).
        /// </summary>
        public bool IsPrimitive;

        /// <summary>
        /// Creates a new type as defined by SDNA.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="typeSize">Size of the type in bytes.</param>
        /// <param name="sdna">Structure DNA for the type.</param>
        public TypeDefinition(string typeName, short typeSize, bool typeIsPrimitive)
        {
            this.Name = typeName;
            this.Size = typeSize;
            this.IsPrimitive = typeIsPrimitive;
        }
    }
}
