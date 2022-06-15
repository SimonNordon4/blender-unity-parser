using System;
using System.IO;
using System.Collections.Generic;

namespace BlenderToUnity
{
    /// <summary>
    /// The DNA1Block of the FileBlock. 
    /// </summary>
    public class DNA1Block : FileBlock
    {
        /// <summary>
        /// Names of all possible Fields for within the Blend File. (*id, filter_type, render etc)
        /// </summary>
        public List<string> Names { get; private set; }

        /// <summary>
        /// Name of the Types of all possible Fields within the Blend File. ( short, vec3f, Mesh, etc)
        /// </summary>
        public List<string> NameTypes { get; private set; }

        /// <summary>
        /// The Byte Size of the Types of all possible Fields within the Blend File. (1, 2, 4, 8, etc)
        /// </summary>
        public List<short> TypeSizes { get; private set; }

        /// <summary>
        /// The Index of the Type which the Structure is based on.
        /// <remarks>
        /// For Example assume we wanted to know what type Structures[42] is.
        /// typeof(Structures[42]).ToString() == NameTypes[StructureTypeIndices[42]]
        /// </remarks>
        /// </summary>
        public List<short> StructureTypeIndices { get; private set; }
        public List<StructureType> StructureTypes { get; private set; }

        public DNA1Block(FileBlock block)
        {
            this.BlockStartPosition = block.BlockStartPosition;
            this.Code = block.Code;
            this.LenBody = block.LenBody;
            this.OldMemoryAddress = block.OldMemoryAddress;
            this.SDNAIndex = block.SDNAIndex;
            this.Count = block.Count;
            this.Body = block.Body;
        }

        public static DNA1Block ReadDNA1Block(BlenderFile blendFile)
        {
            var reader = blendFile.Reader;

            // The raw data block (second last block)
            var rawBlock = blendFile.FileBlocks[blendFile.FileBlocks.Count - 2];

            // Reset the stream to the start of the body of the block (skipping the header)
            var rawStartPosition = rawBlock.BlockStartPosition;
            var rawBlockHeaderSize = blendFile.Header.PointerSize == 4 ? 20 : 24;
            blendFile.Reader.BaseStream.Position = rawStartPosition + rawBlockHeaderSize;

            // Read the block


            var sdna = new string(reader.ReadChars(4));

            if (sdna != "SDNA")
            {
                f.printError($"Expected SDNA but got: {sdna} at position {reader.BaseStream.Position}");
                return null;
            }

            var dna1Block = new DNA1Block(rawBlock);

            // Get Names.
            dna1Block.Names = ReadNames(reader);
            if (dna1Block.Names is null)
            {
                f.printError("Failed to get name list.");
                return null;
            }

            // Get Type Names.
            dna1Block.NameTypes = ReadTypeNames(reader);
            if (dna1Block.NameTypes is null)
            {
                f.printError("Failed to get type list.");
                return null;
            }

            // Get Type Lengths.
            dna1Block.TypeSizes = ReadTypeLengths(reader, dna1Block.NameTypes.Count);
            if (dna1Block.TypeSizes is null)
            {
                f.printError("Failed to get type length list.");
                return null;
            }

            // Read The Structure types (Field and Indices)
            dna1Block.StructureTypes = ReadStructureTypeIndicesAndFields(reader);
            if (dna1Block.StructureTypes is null)
            {
                f.printError("Failed to get structure type fields.");
                return null;
            }

            return dna1Block;
        }



        private static List<string> ReadNames(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "NAME")
            {
                f.printError($"Failed reading SDNA, Name could not be read at {reader.BaseStream.Position}");
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

        private static List<string> ReadTypeNames(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "TYPE")
            {
                f.printError($"Failed reading SDNA, TYPE could not be read at {reader.BaseStream.Position}");
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

        private static List<short> ReadTypeLengths(BinaryReader reader, int numberOfTypes)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "TLEN")
            {
                f.printError($"Failed reading SDNA, TLEN could not be read at {reader.BaseStream.Position}");
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

        /// <summary>
        /// Reads the StructureTypeIndices and StructureTypeFields. We do this at the same time for speed reasons, hence the tuple.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>A tuple containing the structure type indices and the structure type fields.</returns>
        private static List<StructureType> ReadStructureTypeIndicesAndFields(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "STRC")
            {
                f.printError($"Failed reading SDNA, STRC could not be read at {reader.BaseStream.Position}");
                return null;
            }

            int numberOfStructures = reader.ReadInt32();

            var structureTypes = new List<StructureType>(numberOfStructures);

            for (int i = 0; i < numberOfStructures; i++)
            {
                short structureTypeIndex = reader.ReadInt16();

                short numberOfFields = reader.ReadInt16();

                var structueTypeFields = GetStructureTypeFields(reader, numberOfFields);

                var structureType = new StructureType(structureTypeIndex, structueTypeFields);

                structureTypes.Add(structureType);
            }

            return structureTypes;
        }

        private static List<StructureTypeField> GetStructureTypeFields(BinaryReader reader, int numberOfFields)
        {
            var structureTypeFields = new List<StructureTypeField>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {
                var structureTypeField = new StructureTypeField();
                short typeOfField = reader.ReadInt16();
                short name = reader.ReadInt16();
                structureTypeField.TypeOfField = typeOfField;
                structureTypeField.Name = name;

                structureTypeFields.Add(structureTypeField);
            }

            return structureTypeFields;
        }
    }

    /// <summary>
    /// Contains all fields of a particular structure type.
    /// </summary>
    [System.Serializable]
    public struct StructureType
    {
        public StructureType(short structureTypeIndex, List<StructureTypeField> structureTypeFields)
        {
            StructureTypeIndex = structureTypeIndex;
            StructureTypeFields = structureTypeFields;
        }
        public short StructureTypeIndex;
        public List<StructureTypeField> StructureTypeFields;
    }

    /// <summary>
    /// Contains information about a particular field of a particular structure type.
    /// </summary>
    [System.Serializable]
    public struct StructureTypeField
    {
        public StructureTypeField(short type, short name)
        {
            TypeOfField = type;
            Name = name;
        }
        public short TypeOfField;
        public short Name;
    }
}
