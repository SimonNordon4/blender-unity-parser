using System;
using System.Linq;

namespace BlenderToUnity
{
    /// <summary>
    /// A field of a structure as defined by SDNA.
    /// </summary>
    public struct FieldDefinition
    {
        public string Name;

        public TypeDefinition Type;

        public bool IsPrimitive;

        public bool IsPointer;

        public bool IsArray;

        public FieldDefinition(string name, TypeDefinition type, StructureDNA sdna)
        {
            this.Name = name;
            this.Type = type;
            this.IsPrimitive = type.IsPrimitive;
            this.IsPointer = Name[0] == '*';
            this.IsArray = Name.Contains("[");
        }
    }
}
