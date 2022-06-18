using System;
using System.Linq;

namespace BlenderToUnity
{
    public enum FieldKind
    {
        Primitive,
        Pointer,
        PointerToPointer,
        PointerToPointerToPointer,
        Array,
        ArrayOfArray,
        ArrayOfArrayOfArray,
        Struct
    }
    /// <summary>
    /// A field of a structure as defined by SDNA.
    /// </summary>
    [System.Serializable]
    public struct FieldDefinition
    {
        public string Name;

        public TypeDefinition Type;

        public bool IsPrimitive;

        public bool IsPointer;

        public bool IsArray;

        public bool IsInitialised;

        public FieldDefinition(string name, TypeDefinition type)
        {
            this.Name = name;
            this.Type = type;
            this.IsPrimitive = type.IsPrimitive;
            this.IsPointer = Name[0] == '*';
            this.IsArray = Name.Contains("[");
            
            IsInitialised=false;
        }
    }
}
