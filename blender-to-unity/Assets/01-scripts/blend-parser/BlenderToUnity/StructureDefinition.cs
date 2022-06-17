using System;
using System.Collections.Generic;

namespace BlenderToUnity
{
    /// <summary>
    /// Represents a structure as defined by SDNA. 
    /// </summary>
    [System.Serializable]
    public struct StructureDefinition
    {
        // // Don't know why we need private yet.
        // private List<FieldDefinition> _fields;
        // private short _sturctureIndexType;

        // Pretty sure the next 3 items are just for referencing and not critical.
        public TypeDefinition TypeDefinition;
        
        public string StructureTypeName;

        public short StructureTypeSize;

        public List<FieldDefinition> FieldDefinitions;

        private bool isInitialised;

        // private StructureDNA sdna;

        public StructureDefinition(TypeDefinition typeDefinition, List<FieldDefinition> fieldDefinitions)
        {
            isInitialised = false;
            
            this.TypeDefinition = typeDefinition;
            this.StructureTypeName = typeDefinition.Name;
            this.StructureTypeSize = typeDefinition.Size;
            this.FieldDefinitions = fieldDefinitions;
        }

        // So Initialize fields calls initialize structure on every field, which calls intialize fields, which call initialize structure etc until every structure / field has been initialized...
        // public void InitializeFields()
        // {
        //     if(isInitialised)
        //         throw new InvalidOperationException("Can't initialize a structure twice.");
        //     isInitialised = true;
        //     foreach(FieldDefinition f in _fields)
        //     {
        //         if(!f.IsPrimitive)
        //         {
        //             f.InitializeStructure(sdna);
        //         }
        //     }
        // }
    }
}
