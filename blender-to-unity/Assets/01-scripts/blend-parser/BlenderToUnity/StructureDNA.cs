using System.IO;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

namespace BlenderToUnity
{
    /// <summary>
    /// Contains data from a the special "DNA1" File Block. This block contains all the important information about the other file blocks in the .blend file.
    /// </summary>
    [System.Serializable]
    public class StructureDNA
    {
        #region Raw Data
        [Title("Raw Data")]
        /// <summary>
        /// A reference to the Original File Block that this structure was parsed from.
        /// </summary>
        public FileBlock OriginalFileBlock = null;

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
        public List<short> TypeSizes = new List<short>();

        /// <summary>
        /// List of all DNA Types by index in Types / TypeDefinitions.
        /// </summary>
        public List<short> StructureTypeIndices = new List<short>();



        /// <summary>
        /// Usually this would be a List<List<StructureTypeField>> but Unity wouldn't be able to serialize that, so we use a container class.
        /// </summary>
        /// <typeparam name="StructureTypeFieldContainer">Serializable class containg a list of StructureTypeField</typeparam>
        /// <returns></returns>
        public List<StructureTypeFieldContainer> StructureTypeFieldContainers = new List<StructureTypeFieldContainer>();

        #endregion

        #region Generated Data
        [Title("Generated Data")]
        public List<TypeDefinition> TypeDefintions = new List<TypeDefinition>();

        public List<StructureDefinition> StructureDefinitions = new List<StructureDefinition>();
        #endregion

        /// <summary>
        /// Parse the "DNA1" FileBlock into a StructureDNA. Assume the reader is set to the start of the DNA1 block.
        /// </summary>
        public static StructureDNA ReadStructureDNA(BinaryReader reader)
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
            structureDNA.TypeSizes = ReadTypeLengths(reader, structureDNA.Types.Count);
            if (structureDNA.TypeSizes is null)
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
            structureDNA.StructureTypeFieldContainers = tuple.Item2;
            if(structureDNA.StructureTypeFieldContainers is null)
            {
                f.printError("Failed to get structure type fields.");
                return null;
            }

            return structureDNA;
        }


        public bool SetFileBlock(List<FileBlock> fileBlocks)
        {
            var dna1 = fileBlocks[fileBlocks.Count-2];
            if(dna1.Code != "DNA1")
            {
                f.printError("Expected DNA1 but got: " + dna1.Code + " at position " + dna1.BlockStartPosition);
                return false;
            }

            this.OriginalFileBlock = dna1;
            return true;
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
                var name = new string(tempCharList.ToArray());

                nameList.Add(name);
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
                var typeName = new string(tempCharList.ToArray());


                typeNameList.Add(typeName);
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
        private static Tuple<List<short>, List<StructureTypeFieldContainer>> ReadStructureTypeIndicesAndFields(BinaryReader reader)
        {
            var type = new string(reader.ReadChars(4));
            if (type != "STRC")
            {
                f.printError($"Failed reading SDNA, STRC could not be read at {reader.BaseStream.Position}");
                return null;
            }

            int numberOfStructures = reader.ReadInt32();

            var structureTypeIndices = new List<short>(numberOfStructures);
            var structureTypeFieldContainers = new List<StructureTypeFieldContainer>(numberOfStructures);

            for (int i = 0; i < numberOfStructures; i++)
            {
                short structureTypeIndex = reader.ReadInt16();
                short numberOfFields = reader.ReadInt16();
               
                var structureTypeFieldContainer = new StructureTypeFieldContainer(new List<StructureTypeField>(numberOfFields));

                for(int j = 0; j < numberOfFields; j++)
                {
                    var structureTypeField = new StructureTypeField();
                    short typeOfField = reader.ReadInt16();
                    short name = reader.ReadInt16();
                    structureTypeField.TypeOfField = typeOfField;
                    structureTypeField.Name = name;
                    structureTypeFieldContainer.StructureTypeFields.Add(structureTypeField);
                }
                
                structureTypeIndices.Add(structureTypeIndex);
                structureTypeFieldContainers.Add(structureTypeFieldContainer);
                
            }

            return Tuple.Create(structureTypeIndices, structureTypeFieldContainers);
        }
    }

    /// <summary>
    /// Contains all fields of a particular structure type.
    /// </summary>
    [System.Serializable]
    public struct StructureTypeFieldContainer
    {
        public StructureTypeFieldContainer(List<StructureTypeField> structureTypeFields)
        {
            StructureTypeFields = structureTypeFields;
        }
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