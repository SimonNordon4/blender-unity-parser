using System.IO;
using System.Collections.Generic;
using System;

namespace BlenderToUnity
{
    [System.Serializable]
    public class StructureDNA : FileBlock
    {
        /// <summary>
        /// List of all the names contained in SDNA.
        /// Example: [x, y, z, quat[4], *description] etc.
        /// </summary>
        public List<string> Names = new List<string>();

        /// <summary>
        /// List of all the names of the Types in the SDNA.
        /// Example: [Camera, Image, Scene, Cloth] etc
        /// </summary>
        public List<string> Types = new List<string>();

        /// <summary>
        /// index matched byte length for each type in Types.
        /// </summary>
        public List<short> TypeLengths = new List<short>();

        /// <summary>
        /// List of all DNA Types be index in Types / TypeDefinitions.
        /// </summary>
        public List<short> StructureTypeIndices = new List<short>();

        public List<StructureTypeField> StructureTypeFields = new List<StructureTypeField>();

        /// <summary>
        /// Parse the "DNA1" FileBlock into a StructureDNA. Assume the reader is set to position 0.
        /// </summary>
        public static StructureDNA CreateStructureDNA(BinaryReader reader)
        {
            var sdna = new string(reader.ReadChars(4));

            if (sdna != "SDNA")
            {
                f.printError($"Expected SDNA but got: {sdna} at position {reader.BaseStream.Position}");
                return null;
            }

            var structureDNA = new StructureDNA();

            // Get Names.
            structureDNA.Names = ReadNames(reader);
            if (structureDNA.Names is null)
            {
                f.printError("Failed to get name list.");
                return null;
            }

            // Get Type Names.
            structureDNA.Types = ReadTypeNames(reader);
            if (structureDNA.Types is null)
            {
                f.printError("Failed to get type list.");
                return null;
            }

            // Get Type Lengths.
            structureDNA.TypeLengths = ReadTypeLengths(reader, structureDNA.Types.Count);
            if (structureDNA.TypeLengths is null)
            {
                f.printError("Failed to get type length list.");
                return null;
            }

            // Read Structure Type Indices and Structure Type Fields.
            var tuple = ReadStructureTypeIndicesAndFields(reader);
            structureDNA.StructureTypeIndices = tuple.Item1;
            if(structureDNA.StructureTypeIndices is null)
            {
                f.printError("Failed to get structure type indices.");
                return null;
            }
            structureDNA.StructureTypeFields = tuple.Item2;
            if(structureDNA.StructureTypeFields is null)
            {
                f.printError("Failed to get structure type fields.");
                return null;
            }

            return structureDNA;
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
            
            for(int i = 0; i < numberOfTypes; i++)
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
        private static Tuple<List<short>, List<StructureTypeField>> ReadStructureTypeIndicesAndFields(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "STRC")
            {
                f.printError($"Failed reading SDNA, STRC could not be read at {reader.BaseStream.Position}");
                return null;
            }

            int numberOfStructures = reader.ReadInt32();

            var structureTypeIndices = new List<short>();
            var structureTypeFields = new List<StructureTypeField>();

            for (int i = 0; i < numberOfStructures; i++)
            {
                short structureTypeIndex = reader.ReadInt16();
                short numberOfFields = reader.ReadInt16();
                
                var structureTypeField = new StructureTypeField();

                for(int j = 0; j < numberOfFields; j++)
                {
                    short typeOfField = reader.ReadInt16();
                    short name = reader.ReadInt16();
                    structureTypeField.TypeOfField = typeOfField;
                    structureTypeField.Name = name;
                }
                structureTypeIndices.Add(structureTypeIndex);
                structureTypeFields.Add(structureTypeField);
            }

            return Tuple.Create(structureTypeIndices, structureTypeFields);
        }
    }

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