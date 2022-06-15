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
        /// A list of all Types of Structures defined in the DNA1 Block. Each Structure contains and index to it's Type, as well as a list of it's Fields names and types.
        /// </summary>
        public List<StructureType> StructureTypes = new List<StructureType>();

        #endregion

        #region Generated Data
        [Title("Generated Data")]
        public List<TypeDefinition> TypeDefintions = new List<TypeDefinition>();

        public List<StructureDefinition> StructureDefinitions = new List<StructureDefinition>();
        #endregion

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
    
       
    }
}
