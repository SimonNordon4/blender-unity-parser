using System;
using System.Linq;

namespace BlenderToUnity
{
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

        public FieldDefinition(string name, TypeDefinition type, StructureDNA sdna)
        {
            this.Name = name;
            this.Type = type;
            this.IsPrimitive = type.IsPrimitive;
            this.IsPointer = Name[0] == '*';
            this.IsArray = Name.Contains("[");
            IsInitialised=false;
        }

        public FieldDefinition(short nameIndex, short typeIndex, StructureDNA sdna)
        {
            this.Name = sdna.Names[nameIndex];
            this.Type = sdna.TypeDefintions[typeIndex];
            this.IsPrimitive = Type.IsPrimitive;
            this.IsPointer = Name[0] == '*';
            this.IsArray = Name.Contains("[");
            this.IsInitialised = false;

            if(Type.Name.Count(v => { return v == '['; }) > 2)
            {
                throw new Exception("A 3D array is present and this program is not set up to handle that.");
            }
        }

        // TODO: figure out what's happening here.
        public void InitializeStructure(StructureDNA sdna)
        {
            if(IsInitialised)
                throw new InvalidOperationException("Can't initialize a field's structure twice.");
            if(IsPrimitive)
                return;
            IsInitialised = true;

            string name = Type.Name; // can't use 'this'
            var structure = sdna.StructureDefinitions.Find(v => { return v.StructureTypeName == name; });
            structure.InitializeFields();

        }
    }
}
