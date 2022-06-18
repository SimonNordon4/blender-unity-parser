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
        // Helper Fields

        public FileBlock DNA1Block { get; private set; }

        public List<string> FieldNames { get; private set; }

        public List<string> TypeNames { get; private set; }

        public List<short> TypeSizes { get; private set; }


        public List<DNAStruct> DNAStructs { get; private set; }

        [field: SerializeField]
        public List<DNAType> DNATypes { get; private set; }

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
            this.DNAStructs = ReadDNAStructs(reader);

            // Now we can generate our DNATypes.
            this.DNATypes = GetDNATypes();

            return;
        }

        private List<string> ReadNames(BinaryReader reader)
        {
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

        private List<DNAStruct> ReadDNAStructs(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
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
                var fields = ReadDNAStructFields(reader, numberOfFields);

                var typeName = this.TypeNames[typeIndex];

                var dnaStruct = new DNAStruct();
                dnaStruct.TypeIndex = typeIndex;
                dnaStruct.TypeName = typeName;
                dnaStruct.NumberOfFields = numberOfFields;
                dnaStruct.Fields = fields;

                dnaStructs.Add(dnaStruct);
            }

            return dnaStructs;
        }

        private List<DNAField> ReadDNAStructFields(BinaryReader reader, int numberOfFields)
        {
            var DNAFields = new List<DNAField>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {

                short typeIndex = reader.ReadInt16();
                short fieldNameIndex = reader.ReadInt16();

                string typeName = this.TypeNames[typeIndex];
                string fieldName = this.FieldNames[fieldNameIndex];

                var dnaField = new DNAField(typeIndex,fieldNameIndex,typeName,fieldName);

                DNAFields.Add(dnaField);
            }

            return DNAFields;
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

                var isPrimitve = !DNAStructs.Any(dnaStruct => dnaStruct.TypeIndex == i);
                dnaType.IsPrimitive = isPrimitve;
            
                dnaType.DnaStruct = isPrimitve ? new DNAStruct(i, this.TypeNames[i], 0, null) : DNAStructs.First(dnaStruct => dnaStruct.TypeIndex == i);

                dnaTypes.Add(dnaType);
            }

            return dnaTypes;
        }

        // private List<TypeDefinition> GetTypeDefintiions(DNA1Block dna1)
        // {
        //     int numberOfTypes = dna1.TypeNames.Count;

        //     var typeDefintions = new List<TypeDefinition>(numberOfTypes);
        //     for (int i = 0; i < numberOfTypes; i++)
        //     {
        //         var typeName = dna1.TypeNames[i];
        //         var typeSize = dna1.TypeSizes[i];

        //         // type is primitive if it doesn't exist in the DNAstructures
        //         var isStructureType = dna1.DNAStructs.Any(st => st.TypeIndex == (short)i);
        //         var typeIsPrimitive = !isStructureType;

        //         var typeDefinition = new TypeDefinition();

        //         typeDefintions.Add(typeDefinition);
        //     }

        //     return typeDefintions;
        // }

        // private List<StructureDefinition> GetStructureDefinitions(DNA1Block dna1)
        // {
        //     int numberOfStructures = dna1.DNAStructs.Count;

        //     var structureDefinitions = new List<StructureDefinition>(numberOfStructures);

        //     for (int i = 0; i < numberOfStructures; i++)
        //     {
        //         var typeIndex = dna1.DNAStructs[i].TypeIndex;
        //         var structureTypeDefintion = this.TypeDefinitions[typeIndex];
        //         List<FieldDefinition> fields = CreateFieldDefinitions(i, dna1);
        //         var structureDefinition = new StructureDefinition(structureTypeDefintion, fields);
        //         structureDefinitions.Add(structureDefinition);
        //     }

        //     return structureDefinitions;
        // }

        // /// <summary>
        // /// Create FieldDefinitions for a given structure.
        // /// </summary>
        // /// <param name="index">The index of the Structure being assessed.</param>
        // /// <returns>List of generated FieldDefinitions for that particular structure at index</returns>
        // private List<FieldDefinition> CreateFieldDefinitions(int structureDefintionIndex, DNA1Block dna1)
        // {
        //     DNAStruct structureType = dna1.DNAStructs[structureDefintionIndex];
        //     int numberOfFields = structureType.Fields.Count;

        //     List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>(numberOfFields);

        //     for (int i = 0; i < numberOfFields; i++)
        //     {
        //         var structureField = structureType.Fields[i];
        //         var fieldName = dna1.FieldNames[structureField.NameIndex];
        //         var fieldType = this.TypeDefinitions[structureField.TypeIndex];
        //         var fieldDefinition = new FieldDefinition(fieldName, fieldType);
        //         fieldDefinitions.Add(fieldDefinition);
        //     }

        //     return fieldDefinitions;
        // }

    }
}
