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
        public List<DNAStruct> StructureTypes { get; private set; }

        public DNA1Block(BlenderFile file)
        {
            var rawBlock = file.FileBlocks[file.FileBlocks.Count - 2];

            this.BlockStartPosition = rawBlock.BlockStartPosition;
            this.Code = rawBlock.Code;
            this.LenBody = rawBlock.LenBody;
            this.OldMemoryAddress = rawBlock.OldMemoryAddress;
            this.SDNAIndex = rawBlock.SDNAIndex;
            this.Count = rawBlock.Count;
            this.Body = rawBlock.Body;

            var reader = file.Reader;
            var rawStartPosition = rawBlock.BlockStartPosition;
            var rawBlockHeaderSize = file.Header.PointerSize == 4 ? 20 : 24;
            file.Reader.BaseStream.Position = rawStartPosition + rawBlockHeaderSize;

            var sdna = new string(reader.ReadChars(4));

            // Get Names.
            this.Names = ReadNames(reader);

            // Get Type Names.
            this.NameTypes = ReadTypeNames(reader);

            // Get Type Lengths.
            this.TypeSizes = ReadTypeLengths(reader, this.NameTypes.Count);

            // Read The Structure types (Field and Indices)
            this.StructureTypes = ReadStructureTypeIndicesAndFields(reader);
        }

        private List<string> ReadNames(BinaryReader reader)
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

        private List<string> ReadTypeNames(BinaryReader reader)
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

        private List<short> ReadTypeLengths(BinaryReader reader, int numberOfTypes)
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
        private List<DNAStruct> ReadStructureTypeIndicesAndFields(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "STRC")
            {
                f.printError($"Failed reading SDNA, STRC could not be read at {reader.BaseStream.Position}");
                return null;
            }

            int numberOfStructures = reader.ReadInt32();

            var structureTypes = new List<DNAStruct>(numberOfStructures);

            for (int i = 0; i < numberOfStructures; i++)
            {
                short typeIndex = reader.ReadInt16();
                short numberOfFields = reader.ReadInt16();
                var fields = GetStructureTypeFields(reader, numberOfFields);

                var typeName = this.NameTypes[typeIndex];

                var structureType = new DNAStruct(typeIndex, typeName, fields);

                structureTypes.Add(structureType);
            }

            return structureTypes;
        }

        private List<DNAField> GetStructureTypeFields(BinaryReader reader, int numberOfFields)
        {
            var structureTypeFields = new List<DNAField>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {
                
                short typeIndex = reader.ReadInt16();
                short nameIndex = reader.ReadInt16();

                string typeName = this.NameTypes[typeIndex];
                string name = this.Names[nameIndex];

                var dnaField = new DNAField(typeIndex, nameIndex, typeName, name);

                structureTypeFields.Add(dnaField);
            }

            return structureTypeFields;
        }
    }

    /// <summary>
    /// Contains all fields of a particular structure type.
    /// </summary>
    public struct DNAStruct
    {
        public short TypeIndex;
        public string TypeName;
        public List<DNAField> Fields;

        public DNAStruct(short typeIndex,string typeName, List<DNAField> fields)
        {
            this.TypeIndex = typeIndex;
            this.TypeName = typeName;
            this.Fields = fields;
        }
    }

    /// <summary>
    /// Contains information about a particular field of a particular structure type.
    /// </summary>
    [System.Serializable]
    public struct DNAField
    {
        public short TypeIndex;
        public short NameIndex;
        public string Type;
        public string Name;
        public DNAField(short typeIndex, short nameIndex,string typeName, string name)
        {
            TypeIndex = typeIndex;
            NameIndex = nameIndex;
            Type = typeName;
            Name = name;
        }
    }
}
