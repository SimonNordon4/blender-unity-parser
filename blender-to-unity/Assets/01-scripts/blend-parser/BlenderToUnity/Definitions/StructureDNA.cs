using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace BlenderToUnity
{
    /// <summary>
    /// Contains data from a the special "DNA1" File Block. This block contains all the important information about the other file blocks in the .blend file.
    /// </summary>
    [System.Serializable]
    public class StructureDNA
    {
        // DEBUG

        public StructureDNADebug Debug;
        public FileBlock DNA1Block { get; private set; }

        /// <summary>List of Field names read directly from the blend file.</summary>
        public List<string> FieldNames { get; private set; }

        /// <summary>List of Types (name of the type) read directly from the blend file.</summary>
        public List<string> TypeNames { get; private set; }
        /// <summary>List of Type Sizes (in bytes) read directly from the blend file.</summary>
        public List<short> TypeSizes { get; private set; }

        /// <summary>List of DNAStructs read directly from the blend file.</summary>
        [field: SerializeField]
        public List<DNAStruct> DNAStructs { get; private set; }

        /// <summary>List of all data types infered from the blend file.</summary>
        [field: SerializeField]
        public List<DNAType> DNATypes { get; private set; }

        /// <summary>List of all types that are primitives.</summary>
        [field: SerializeField]
        public List<DNAType> DNAPrimitives { get; private set; } = new List<DNAType>();

        /// <summary>
        /// List of all structs that don't exist in this blender file, including the void type itself
        /// <remarks>
        /// For example, a field may usually reference a struct, but an instance of that struct doesn't exist in the blend file.
        /// </remarks>
        /// </summary>
        [field: SerializeField]
        public List<DNAType> DNAVoids { get; private set; } = new List<DNAType>();



        /// <summary>
        /// Populates the StructureDNA with the data from the "DNA1" File Block in the BlendFile.
        /// </summary>
        /// <param name="blendFile">Blend File Being Parsed. Expects a Binary Reader.</param>
        public void ReadBlenderFile(BlenderFile blendFile)
        {
            var fileBlocks = blendFile.FileBlocks;
            DNA1Block = fileBlocks[fileBlocks.Count - 2];

            // Reset the reader to the start of the DNA1 Block
            var reader = blendFile.Reader;
            var rawStartPosition = DNA1Block.BlockStartPosition;
            var rawBlockHeaderSize = blendFile.Header.PointerSize == 4 ? 20 : 24;
            blendFile.Reader.BaseStream.Position = rawStartPosition + rawBlockHeaderSize;

            reader.ReadBytes(4); // Burn the SDNA tag

            // Get Names.
            this.FieldNames = ReadNames(reader);

            // Get Type Names.
            this.TypeNames = ReadTypeNames(reader);

            // Get Type Lengths.
            int numberOfTypeNames = this.TypeNames.Count;
            this.TypeSizes = ReadTypeLengths(reader, numberOfTypeNames);

            // We get the DNAstructs first because we have to read them from the file.
            this.DNAStructs = ReadDNAStructs(blendFile);

            // Now we can generate our DNATypes.
            this.DNATypes = GetDNATypes();

            

            // TODO. Figure out which Types are VOID by referncing their Size.
            // TODO. Figure out which Types are Primitive if they're not void and they're not a struct.
            // TODO. Updated all Fields based on whether they point to a void, or a primitive type.

            Debug = new StructureDNADebug(blendFile);

            return;
        }

        private List<string> ReadNames(BinaryReader reader)
        {
            // Check we're reading the NAME block.
            var type = new string(reader.ReadChars(4));
            if (type != "NAME")
            {
                f.printError($"Failed reading SDNA, Name could not be read at {reader.BaseStream.Position}. instead got {type}");
                return null;
            }

            int numberOfNames = reader.ReadInt32();

            // burn bytes to get to the next field.
            while (reader.BaseStream.Position % 4 != 0) { reader.ReadByte(); }

            var nameList = new List<string>(numberOfNames);
            var tempCharList = new List<char>();

            for (int i = 0; i < numberOfNames; i++)
            {
                char c;
                do
                {
                    c = reader.ReadChar();
                    tempCharList.Add(c);
                }
                while (c != '\0');
                tempCharList.RemoveAt(tempCharList.Count - 1); // removes terminating zero
                nameList.Add(new string(tempCharList.ToArray()));
                tempCharList.Clear();
            }

            while (reader.BaseStream.Position % 4 != 0) { reader.ReadByte(); }
            return nameList;
        }

        private List<string> ReadTypeNames(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "TYPE")
            {
                f.printError($"Failed reading SDNA, TYPE could not be read at {reader.BaseStream.Position} instead got {type}");
                return null;
            }

            int numberOfTypes = reader.ReadInt32();

            // burn bytes to get to the next field.
            while (reader.BaseStream.Position % 4 != 0) { reader.ReadByte(); }

            var typeNameList = new List<string>(numberOfTypes);
            var tempCharList = new List<char>();

            // Read each type name and add to the list. end of type name reached at '\0'.
            for (int i = 0; i < numberOfTypes; i++)
            {
                char c;
                do
                {
                    c = reader.ReadChar();
                    tempCharList.Add(c);
                }
                while (c != '\0');
                tempCharList.RemoveAt(tempCharList.Count - 1); // removes terminating zero
                typeNameList.Add(new string(tempCharList.ToArray()));
                tempCharList.Clear();
            }
            // burn bytes to get to the next field.
            while (reader.BaseStream.Position % 4 != 0) { reader.ReadByte(); }
            return typeNameList;
        }

        private List<short> ReadTypeLengths(BinaryReader reader, int numberOfTypes)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "TLEN")
            {
                f.printError($"Failed reading SDNA, TLEN could not be read at {reader.BaseStream.Position} instead got {type}");
                return null;
            }

            List<short> typeLengthList = new List<short>(numberOfTypes);

            for (int i = 0; i < numberOfTypes; i++)
            {
                var len = reader.ReadInt16();
                typeLengthList.Add(len);
            }
            return typeLengthList;
        }

        private List<DNAStruct> ReadDNAStructs(BlenderFile file)
        {
            var reader = file.Reader;
            var type = new string(reader.ReadChars(4));

            // Check we're reading the STRC section.
            if (type != "STRC")
            {
                f.printError($"Failed reading SDNA, STRC could not be read at {reader.BaseStream.Position}");
                return null;
            }

            int numberOfStructures = reader.ReadInt32();

            var dnaStructs = new List<DNAStruct>(numberOfStructures);

            for (int i = 0; i < numberOfStructures; i++)
            {
                short typeIndex = reader.ReadInt16();
                short numberOfFields = reader.ReadInt16();
                var fields = ReadDNAStructFields(file, numberOfFields);

                string typeName = this.TypeNames[typeIndex];
                short typeSize = this.TypeSizes[typeIndex];

                var dnaStruct = new DNAStruct(typeIndex, typeName, typeSize, numberOfFields, fields);

                dnaStructs.Add(dnaStruct);
            }

            return dnaStructs;
        }

        // TODO we want to do everything this is doing, only this doesn't work at the moment.
        private List<DNAField> ReadDNAStructFields(BlenderFile file, int numberOfFields)
        {
            var reader = file.Reader;

            var dnaFields = new List<DNAField>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {
                // Read the type index of the field.
                short typeIndex = reader.ReadInt16();
                // Read the index of the field name.
                short fieldNameIndex = reader.ReadInt16();

                // Get the type name, field name and field size from the existings lists.
                string typeName = this.TypeNames[typeIndex];
                string fieldName = this.FieldNames[fieldNameIndex];
                int typeSize = this.TypeSizes[typeIndex];


                // Field is void if it has 0 memory allocated to it.
                bool isVoid = typeSize == 0;

                int pointerDepth = fieldName.Count(c => c == '*');
 
                bool isPointer = pointerDepth > 0;

                // get the dimensions of the array. 0 if it's not an array.
                int arrayDepth = fieldName.Count(c => c == '[');

                bool isArray = arrayDepth > 0;

                int[] arrayLengths = new int[arrayDepth];
                if (isArray)
                {
                    // e.g. "someObj[3][2][4]" => ["3],"2],"4]"] => int[]{3,2,4}
                    string[] arrayName = fieldName.Split('[');

                    arrayLengths = new int[arrayName.Length - 1];
                    for (int j = 1; j < arrayName.Length; j++)
                    {
                        // "2]" => "2" => 2
                        string arrayLengthChar = arrayName[j].TrimEnd(']');
                        int arrayLength = int.Parse(arrayLengthChar);

                        // subtract one because arrayName[1] == arrayLength[0]
                        arrayLengths[j - 1] = arrayLength;
                    }
                }

                // Default fieldSize is the typeSize.
                int fieldSize = typeSize;
                // If field is a pointer, but isn't an array of pointers, then the fieldSize is always the file Headers PointerSize. 
                if(isPointer && !isArray)
                {
                    fieldSize = file.Header.PointerSize;
                }

                // If it's array it's the total number of elements * typeSize.
                else if(isArray && !isPointer)
                {
                    int totalTypesInArray = 1;
                    for (int j = 0; j < arrayLengths.Length; j++)
                    {
                        // take the array [3][2][4] of shorts. => 3 * 2 * 4 = 24 types
                        totalTypesInArray *= arrayLengths[j];
                    }
                    // 24 types * typesize (2 bytes) = 48 bytes
                    fieldSize = totalTypesInArray * typeSize;
                }

                // If it's an array of pointers, it's the total number of elements * fileHeadersPointerSize. (I think)
                else if(isArray && isPointer)
                {
                    int totalTypesInArray = 1;
                    for (int j = 0; j < arrayLengths.Length; j++)
                    {
                        totalTypesInArray *= arrayLengths[j];
                    }
                    fieldSize = totalTypesInArray * file.Header.PointerSize;
                }

                // Create the Field.
                var dnaField = new DNAField();
                dnaField.TypeIndex = typeIndex;
                dnaField.FieldNameIndex = fieldNameIndex;
                dnaField.TypeName = typeName;
                dnaField.FieldName = fieldName;
                dnaField.FieldSize = fieldSize;
                dnaField.IsVoid = isVoid;
                dnaField.IsPointer = isPointer;
                dnaField.PointerDepth = pointerDepth;
                dnaField.IsArray = isArray;
                dnaField.ArrayDepth = arrayDepth;
                dnaField.ArrayLengths = arrayLengths;

                dnaFields.Add(dnaField);
            }

            return dnaFields;
        }

        private List<DNAType> GetDNATypes()
        {
            int numberOfTypes = this.TypeNames.Count;
            var dnaTypes = new List<DNAType>(numberOfTypes);

            for (int i = 0; i < numberOfTypes; i++)
            {
                var dnaType = new DNAType();
                dnaType.TypeIndex = i;
                dnaType.TypeName = this.TypeNames[i];
                dnaType.Size = this.TypeSizes[i];

                // type size is 0 means it's void. Nothing to read, no pointer, or value.
                var isVoid = dnaType.Size <= 0;
                dnaType.IsVoid = isVoid;

                // primitive if it doesn't exist in the structs, and is not a void.
                var isStruct = DNAStructs.Any(dnaStruct => dnaStruct.TypeIndex == i);
                dnaType.IsPrimitive = !isStruct && !isVoid;

                // assign it's structure type if it is a struct.
                dnaType.DnaStruct = isStruct ? DNAStructs.First(dnaStruct => dnaStruct.TypeIndex == i) : new DNAStruct();

                if(isVoid) DNAVoids.Add(dnaType);
                if(!isStruct && !isVoid) DNAPrimitives.Add(dnaType);
                dnaTypes.Add(dnaType);
            }

            return dnaTypes;
        }
    
        // We can only set a field as a primitive after we've gathered all Types and PrimitiveTypes.
        // Because fields are created alongside their structs, we can't check this during.
        private void RecalculateDNAFields()
        {
            for(int i = 0; i < DNAStructs.Count; i++)
            {
                var dnaStruct = DNAStructs[i];
                for(int j = 0; j < dnaStruct.DnaFields.Count; j++)
                {
                    var dnaField = dnaStruct.DnaFields[j];
                    dnaField.IsPrimitive = DNAPrimitives.Any(dnaType => dnaType.TypeIndex == dnaField.TypeIndex);
                }
            }
        }
    }


    [System.Serializable]
    public class StructureDNADebug
    {
        [Header("Types")]
        public List<DNAType> DNAPrimitives = new List<DNAType>();

        public List<DNAType> DNAVoidTypes = new List<DNAType>();

        [Header("Fields")]
        public List<DNAField> DNAFields = new List<DNAField>();

        public List<DNAField> DNAFieldsWithPointers = new List<DNAField>();

        public List<DNAField> DNAFieldsWithPointers2 = new List<DNAField>();

        public List<DNAField> DNAFieldsWithArray = new List<DNAField>();

        public List<DNAField> DNAFieldsWithVoid = new List<DNAField>();

        public StructureDNADebug(BlenderFile file)
        {
            var dna = file.StructureDNA;
            // get primitive or void structs.

            // TYPES
            foreach (var dnaType in dna.DNATypes)
            {
                if (dnaType.IsPrimitive)
                {
                    DNAPrimitives.Add(dnaType);
                }

                if (dnaType.IsVoid)
                {
                    DNAVoidTypes.Add(dnaType);
                }
            }

            // FIELDS

            // group all fields, pointer fields and array fields.

            foreach (var structDna in dna.DNAStructs)
            {
                foreach (var field in structDna.DnaFields)
                {
                    DNAFields.Add(field);

                    if (field.IsPointer)
                    {
                        DNAFieldsWithPointers.Add(field);

                        if (field.PointerDepth > 1)
                        {
                            DNAFieldsWithPointers2.Add(field);
                        }
                    }
                    if (field.IsArray)
                    {
                        DNAFieldsWithArray.Add(field);
                    }

                    if (field.IsVoid)
                    {
                        DNAFieldsWithVoid.Add(field);
                    }
                }
            }
        }
    }
}
