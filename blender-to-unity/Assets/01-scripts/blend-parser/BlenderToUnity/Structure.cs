using System;
using System.Collections.Generic;
using System.Linq;

namespace BlenderToUnity
{
    public class Structure
    {
        public IField Parent { get; private set; }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public string TypeName { get; private set; }

        public short Size { get; private set; }

        public Structure(byte[] body, StructureDefinition definition)
        {
            Name = Value = TypeName = definition.StructureTypeName;
            Size = definition.StructureTypeSize;

            Parent = null;

            List<IField> fields = new List<IField>();

            int pos = 0;
            ParseStructureFields(this, definition, body, ref pos);

        }

        public static Structure[] ParseFileBlock(FileBlock block, int increment, StructureDNA sdna)
        {
            if (block.Count == 0 || block.Code == "DNA1") return null; // We are not parsing a valid data block.

            if (block.Body.Length != sdna.StructureDefinitions[block.SDNAIndex].StructureTypeSize * block.Count)
            {
                // generally, these are things like raw data; packed files, preview images, and arrays of pointers that are themselves pointed to.
                // I have no idea what TEST and REND do.
                f.printError($"File Block {increment} with code {block.Code} and count {block.Count} has a body length ({block.LenBody}) which doesn't match it's sturcture defintiion size ( sdnaIndex;{block.SDNAIndex}) (size: {sdna.StructureDefinitions[block.SDNAIndex].StructureTypeSize})");
                return null;
            }

            Structure[] structures = new Structure[block.Count];

            if (block.Count == 1)
            {
                var newStructure = new Structure(block.Body, sdna.StructureDefinitions[block.SDNAIndex]);
            }

            return structures;

        }

        private List<IField> ParseStructureFields(Structure parent, StructureDefinition parentType, byte[] body, ref int pos)
        {
            List<IField> fields = new List<IField>();

            foreach (FieldDefinition fieldDef in parentType.FieldDefinitions)
            {
                if (fieldDef.IsPointer)
                {
                    if (fieldDef.IsArray)
                    {
                        int height = 1; // array 'height'
                        int width = GetIntFromArrayName(fieldDef.Name); // array 'width'

                        if (fieldDef.Name.Count(v => { return v == '['; }) == 1)
                        {
                            //var newField = new Field<ulong[]>(toPointerArray(subArray(data, position, pointerSize * height)),fieldDef.Name, fieldDef.Type.Name, (short)pointerSize, parent, pointerSize));
                        }
                    }

                }

            }

            return fields;
        }

        // helper function to get array size from field name
        private int GetIntFromArrayName(string name)
        {
            int start = name.IndexOf('[');
            int end = name.IndexOf(']');
            string numberString = name.Substring(start + 1, end - 1 - start);
            int number = int.Parse(numberString);
            return number;
        }
    }
}